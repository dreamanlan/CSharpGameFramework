# -*- coding: utf-8 -*-
"""
Shared procedural terrain synthesis for the 8x8 world tiles and the 640
battlefields.

Design goals (design doc 43):
  * replace the analytic sin/cos height fields with fractal, ridged and
    domain-warped noise so the land reads as geology rather than a graph;
  * carve rivers with variable width and downcutting, add approximate
    hydraulic/thermal erosion and deposition;
  * derive the G material band from height + slope + moisture (continuous,
    per pixel) instead of four hard one-hot steps;
  * keep the existing RGB channel contract so BuildTerrain / BuildStory /
    WorldEvent / areas.txt keep working unchanged:
        R = height   0..255  -> 0..70 world units
        G = material 0..255  -> 4 TerrainLayers (continuous value is fine,
                              BuildTerrain quantises it with soft weights)
        B = walkable 0/128/255

The module is deliberately pure numpy + Pillow so all 640 battlefields can be
regenerated in one pass. Every function is deterministic for a given seed.
"""
import math
import numpy as np
from PIL import Image, ImageFilter

# --------------------------------------------------------------------------
# Deterministic value-noise / fBm
# --------------------------------------------------------------------------

def _hash_grid(nx, ny, seed):
    """Integer lattice -> deterministic pseudo-random floats in [0,1)."""
    h = (np.arange(nx, dtype=np.uint32)[:, None] * np.uint32(374761393) +
         np.arange(ny, dtype=np.uint32)[None, :] * np.uint32(668265263) +
         np.uint32(seed & 0xFFFFFFFF))
    h = (h ^ (h >> np.uint32(13))) * np.uint32(1274126177)
    h = h ^ (h >> np.uint32(16))
    return (h & np.uint32(0x00FFFFFF)).astype(np.float64) / float(0x01000000)


def _smooth(t):
    return t * t * (3.0 - 2.0 * t)


def value_noise(size, cells, seed):
    """
    Smooth value noise on a `cells x cells` lattice.

    Uses Pillow's C-level BICUBIC resampling of a small random lattice. This
    avoids the seam artifacts of a hand-rolled bilinear lookup (integer
    lattice indices jump at cell boundaries) and keeps every octave cheap.
    """
    n = max(2, int(round(cells))) + 1
    g = _hash_grid(n, n, seed)
    small = Image.fromarray((g * 255.0).clip(0, 255).astype(np.uint8), 'L')
    big = small.resize((size, size), Image.BICUBIC)
    return np.asarray(big, dtype=np.float64) / 255.0


def fbm(size, seed, octaves=6, base_cells=3.0, lacunarity=2.0, gain=0.5,
        ridged=False):
    """Fractal Brownian motion; ridged=True produces sharp mountain spines."""
    total = np.zeros((size, size), dtype=np.float64)
    amp, cells, norm = 1.0, float(base_cells), 0.0
    for o in range(octaves):
        n = value_noise(size, cells, seed + o * 7919)
        if ridged:
            n = 1.0 - np.abs(2.0 * n - 1.0)
            n = n * n
        total += n * amp
        norm += amp
        amp *= gain
        cells *= lacunarity
    return total / max(1e-6, norm)


def domain_warp(size, seed, strength=0.18):
    """Two low-frequency warp fields used to break up grid-aligned noise."""
    wx = fbm(size, seed + 101, octaves=3, base_cells=2.0) - 0.5
    wy = fbm(size, seed + 202, octaves=3, base_cells=2.0) - 0.5
    return wx * strength, wy * strength


def sample_warped(field, wx, wy):
    """Bilinearly resample `field` at coordinates shifted by a warp field."""
    h, w = field.shape
    yy, xx = np.mgrid[0:h, 0:w].astype(np.float64)
    sx = np.clip(xx + wx * w, 0.0, w - 1.001)
    sy = np.clip(yy + wy * h, 0.0, h - 1.001)
    x0 = np.floor(sx).astype(np.int32)
    y0 = np.floor(sy).astype(np.int32)
    dx = sx - x0
    dy = sy - y0
    a = field[y0, x0]
    b = field[y0, np.minimum(x0 + 1, w - 1)]
    c = field[np.minimum(y0 + 1, h - 1), x0]
    d = field[np.minimum(y0 + 1, h - 1), np.minimum(x0 + 1, w - 1)]
    return (a * (1 - dx) + b * dx) * (1 - dy) + (c * (1 - dx) + d * dx) * dy


# --------------------------------------------------------------------------
# Rivers, erosion, slope
# --------------------------------------------------------------------------

def river_mask(size, seed, count, meander_seed=0):
    """
    Variable-width meandering river courses. Returns (distance, width) maps in
    pixel units; distance is absolute distance from the course centreline.
    """
    rnd_y = np.linspace(0.0, size - 1.0, size, dtype=np.float64)[:, None]
    x = np.linspace(0.0, size - 1.0, size, dtype=np.float64)[None, :]
    dist = np.full((size, size), 1e6, dtype=np.float64)
    width_map = np.zeros((size, size), dtype=np.float64)
    for k in range(count):
        base = size * (0.24 + 0.31 * k + 0.05 * math.sin(seed * 0.001 + k))
        amp = size * (0.075 + 0.02 * k)
        freq = 1.6 + 0.9 * k + 0.003 * (seed % 97)
        phase = (seed * 0.0137 + k * 2.1 + meander_seed) % (2 * math.pi)
        centre = base + amp * np.sin(x / size * math.pi * freq + phase)
        centre += amp * 0.35 * np.sin(x / size * math.pi * (freq * 2.3) + phase * 1.7)
        d = np.abs(rnd_y - centre)
        # width grows slightly downstream and meanders with the bank noise
        w = size * (0.020 + 0.010 * k) * (1.0 + 0.35 * x / size)
        w = np.broadcast_to(w, (size, size))
        take = d < dist
        dist = np.where(take, d, dist)
        width_map = np.where(take, w, width_map)
    return dist, width_map


def hydraulic_erosion(h, seed, passes=3, strength=0.55):
    """
    Cheap approximation of hydraulic + thermal erosion: iterate a local
    Laplacian smoothing weighted by slope, then re-inject the large-scale
    shape so valleys sharpen without flattening the whole map.
    """
    out = h.copy()
    orig = h.copy()
    for i in range(passes):
        p = np.pad(out, 1, mode='edge')
        lap = (p[:-2, 1:-1] + p[2:, 1:-1] + p[1:-1, :-2] + p[1:-1, 2:]) * 0.25 - out
        slope = np.sqrt(
            (p[2:, 1:-1] - p[:-2, 1:-1]) ** 2 +
            (p[1:-1, 2:] - p[1:-1, :-2]) ** 2)
        # steep areas erode faster; flat areas receive sediment
        k = strength * np.clip(slope / 12.0, 0.0, 1.0)
        out = out + lap * (0.25 + 0.55 * k)
        # small deterministic gully carving
        gully = fbm(h.shape[0], seed + 3000 + i * 131, octaves=3, base_cells=12.0)
        out -= 1.2 * k * (gully - 0.5)
    # preserve the broad silhouette while keeping carved detail
    return 0.35 * orig + 0.65 * out


def slope_map(h):
    """Gradient magnitude in height units per pixel."""
    p = np.pad(h, 1, mode='edge')
    gx = (p[1:-1, 2:] - p[1:-1, :-2]) * 0.5
    gy = (p[2:, 1:-1] - p[:-2, 1:-1]) * 0.5
    return np.sqrt(gx * gx + gy * gy)


# --------------------------------------------------------------------------
# Height -> RGB channels
# --------------------------------------------------------------------------

def compose(height, seed, base, relief, bias=0, rivers=0, craters=0,
            islands=0, dunes=0, terraces=0, boundary=True, water_level=17.0,
            target_water=None):
    """
    Build a full RGB design image from a normalised noise field.

    `height` is expected in 0..1; the function applies the region profile,
    carves features, then returns a 0..255 float RGB stack.
    """
    size = height.shape[0]
    h = height * 255.0
    h = (h - 128.0) * (relief / 100.0) + base
    # ridged component -> mountain spines rather than rounded blobs
    ridge = fbm(size, seed + 4242, octaves=6, base_cells=4.0, ridged=True)
    h += (ridge - 0.42) * relief * 0.55
    # domain warp on a second fBm, applied as a perturbation
    wx, wy = domain_warp(size, seed + 555, strength=0.10)
    warp = sample_warped(fbm(size, seed + 666, octaves=4, base_cells=6.0), wx, wy)
    h += (warp - 0.5) * relief * 0.22

    yy, xx = np.mgrid[0:size, 0:size].astype(np.float64)
    cx = cy = (size - 1) * 0.5
    rr = np.hypot(xx - cx, yy - cy)

    # ---- river downcutting -------------------------------------------------
    if rivers:
        d, width = river_mask(size, seed, rivers)
        t = np.clip(1.0 - d / np.maximum(width, 1.0), 0.0, 1.0)
        # U-shaped valley: smooth banks, flat-ish bed
        carved = h - t * (34.0 + 26.0 * ridge)
        h = np.minimum(h, carved + (1.0 - t) * 0.0)
        # bank deposition just outside the channel
        bank = np.clip((d - width) / np.maximum(width * 0.8, 1.0), 0.0, 1.0)
        h += np.exp(-bank * bank * 4.0) * (d < width * 2.2) * 5.0

    # ---- craters / shafts --------------------------------------------------
    for k in range(craters):
        ang = (seed * 0.017 + k * 2.399) % (2 * math.pi)
        rad = size * (0.13 + 0.05 * ((seed >> (k + 2)) % 5))
        px = cx + math.cos(ang) * size * 0.26
        py = cy + math.sin(ang) * size * 0.26
        d = np.hypot(xx - px, yy - py)
        inner = np.clip(1.0 - d / rad, 0.0, 1.0)
        rim = np.exp(-((d - rad) / (rad * 0.18)) ** 2)
        h -= inner ** 1.6 * 46.0
        h += rim * 20.0

    # ---- islands -----------------------------------------------------------
    for k in range(islands):
        ang = (seed * 0.011 + k * 2.7) % (2 * math.pi)
        px = cx + math.cos(ang) * size * 0.22
        py = cy + math.sin(ang) * size * 0.22
        rad = size * (0.10 + 0.03 * k)
        d = np.hypot(xx - px, yy - py)
        h += np.clip((rad - d) / rad, 0.0, 1.0) ** 1.3 * 48.0

    # ---- dunes / terraces --------------------------------------------------
    if dunes:
        dune = np.sin(xx * 0.11 + yy * 0.043 + seed * 0.003) * 0.5 + 0.5
        dune *= 0.7 + 0.3 * np.sin(yy * 0.021 - xx * 0.017)
        h += dune * 13.0 * dunes
    if terraces:
        ring = np.floor(rr / (size * 0.086)) % 4
        h += ring * 6.0 * min(2, terraces)

    # ---- perimeter ridge, feathered so the tile edge is not a box ----------
    if boundary:
        edge = np.minimum.reduce([xx, yy, size - 1 - xx, size - 1 - yy])
        ridge_w = size * 0.055
        t = np.clip((ridge_w - edge) / ridge_w, 0.0, 1.0)
        irregular = 0.65 + 0.35 * value_noise(size, 9.0, seed + 777)
        h += t * t * 62.0 * irregular
        outside = np.clip((size * 0.39 - rr) * -1.0 / (size * 0.12), 0.0, 1.0)
        h += outside * 30.0 * irregular

    h = hydraulic_erosion(h, seed, passes=3, strength=0.55)

    # ---- sea-level normalisation -------------------------------------------
    # Low-baseline regions (river deltas, archipelagos) otherwise drown almost
    # entirely once river downcutting is applied. Shift the whole field so the
    # water fraction matches the region's intent, then gently compress the
    # extreme highs instead of hard-clipping them into flat plateaus.
    if target_water is None:
        target_water = 0.42 if islands else 0.11
    q = float(np.percentile(h, target_water * 100.0))
    h = h + (water_level + 2.0 - q)
    hi = np.maximum(h - 208.0, 0.0)
    h = np.minimum(h, 208.0) + 47.0 * (1.0 - np.exp(-hi / 47.0))
    h = np.clip(h, 0.0, 255.0)

    # ---- material band from height + slope + moisture ---------------------
    slope = slope_map(h)
    moisture = fbm(size, seed + 888, octaves=4, base_cells=5.0)
    # low & wet -> grass / sand; high & steep -> rock; very high -> snow
    g = np.zeros_like(h)
    g += np.clip((h - 60.0) / 90.0, 0.0, 1.0) * 120.0       # height ramp
    g += np.clip((slope - 1.6) / 7.0, 0.0, 1.0) * 75.0       # steep -> rock
    g -= (moisture - 0.5) * 34.0                              # wet -> lower band
    g += bias * 16.0
    g = np.clip(g, 8.0, 250.0)

    # ---- channels ---------------------------------------------------------
    R = np.clip(h, 0.0, 255.0)
    water = R < water_level
    B = np.where(R < 78.0, 255.0, 128.0)
    B = np.where(water, 0.0, B)
    if boundary:
        edge = np.minimum.reduce([xx, yy, size - 1 - xx, size - 1 - yy])
        B = np.where(edge < size * 0.025, 0.0, B)
    B = np.where(R > 205.0, 0.0, B)
    B = np.where(slope > 13.0, 0.0, B)

    return np.stack([R, g, B], axis=-1)


def to_image(rgb):
    return Image.fromarray(np.clip(rgb, 0, 255).astype(np.uint8), 'RGB')


# --------------------------------------------------------------------------
# Hillshaded, continuous-colour cartography for minimaps
# --------------------------------------------------------------------------

def hillshade(height, az_deg=315.0, alt_deg=48.0, exaggeration=2.2):
    """Return a 0..1 lighting map from a height field."""
    h = np.asarray(height, dtype=np.float64) * exaggeration
    p = np.pad(h, 1, mode='edge')
    dzdx = (p[1:-1, 2:] - p[1:-1, :-2]) * 0.5
    dzdy = (p[2:, 1:-1] - p[:-2, 1:-1]) * 0.5
    az = math.radians(az_deg)
    alt = math.radians(alt_deg)
    # surface normal (-dzdx, -dzdy, 1)
    nx, ny, nz = -dzdx, -dzdy, np.ones_like(h)
    ln = np.sqrt(nx * nx + ny * ny + nz * nz)
    nx, ny, nz = nx / ln, ny / ln, nz / ln
    lx = math.cos(alt) * math.sin(az)
    ly = math.cos(alt) * math.cos(az)
    lz = math.sin(alt)
    light = nx * lx + ny * ly + nz * lz
    return np.clip(0.28 + 0.72 * np.clip(light, 0.0, 1.0), 0.0, 1.0)


# Art-directed elevation ramp: water -> shore -> grass -> dry grass -> rock -> snow
_RAMP = [
    (0.00, (48, 92, 128)),
    (0.07, (74, 122, 150)),
    (0.11, (196, 184, 140)),   # sand shoreline
    (0.18, (108, 138, 84)),    # grass
    (0.38, (138, 148, 92)),
    (0.58, (164, 146, 106)),   # dry earth
    (0.74, (138, 128, 112)),   # rock
    (0.88, (176, 176, 172)),
    (1.00, (232, 236, 238)),   # snow
]


def ramp(t):
    """Vectorised colour lookup over the elevation ramp."""
    t = np.clip(t, 0.0, 1.0)
    out = np.zeros(t.shape + (3,), dtype=np.float64)
    for i in range(len(_RAMP) - 1):
        a, ca = _RAMP[i]
        b, cb = _RAMP[i + 1]
        m = (t >= a) & (t < b)
        if not m.any():
            continue
        u = ((t[m] - a) / max(1e-6, b - a))[:, None]
        out[m] = np.asarray(ca, dtype=np.float64) * (1 - u) + np.asarray(cb, dtype=np.float64) * u
    out[t >= _RAMP[-1][0]] = np.asarray(_RAMP[-1][1], dtype=np.float64)
    return out


def contour_lines(height, spacing=18.0):
    """Soft 1px contour mask for a cartographic look."""
    q = np.mod(height, spacing)
    d = np.minimum(q, spacing - q)
    return np.clip(1.0 - d / 1.15, 0.0, 1.0)


def cartographic_minimap(terrain, size=512, seed=0, title=None, overlays=None):
    """
    Continuous hillshaded minimap with elevation ramp, contours and a subtle
    paper grain. `terrain` is a 256x256 RGB design image (R = height).
    """
    src = np.asarray(terrain.convert('RGB'), dtype=np.float64)
    if src.shape[0] != size:
        im = terrain.resize((size, size), Image.LANCZOS)
        src = np.asarray(im.convert('RGB'), dtype=np.float64)
    h = src[:, :, 0]
    g = src[:, :, 1]
    b = src[:, :, 2]

    t = np.clip((h - 10.0) / 205.0, 0.0, 1.0)
    col = ramp(t)
    # material band nudges hue slightly so grass/rock regions stay distinct
    warm = np.clip((g - 128.0) / 128.0, -1.0, 1.0)[:, :, None]
    col = col + warm * np.array([7.0, 3.0, -5.0], dtype=np.float64)

    light = hillshade(h / 255.0, exaggeration=2.6)[:, :, None]
    col = col * (0.62 + 0.55 * light)

    # water: deeper blue + soft shoreline
    water = (h < 17.0)
    depth = np.clip((17.0 - h) / 17.0, 0.0, 1.0)[:, :, None]
    water_col = np.array([62.0, 104.0, 138.0]) - depth * np.array([18.0, 26.0, 24.0])
    col = np.where(water[:, :, None], water_col, col)
    shore = (h >= 17.0) & (h < 26.0)
    col = np.where(shore[:, :, None], col * 0.90 + np.array([18.0, 16.0, 10.0]), col)

    # contour lines + steep-slope darkening
    contours = contour_lines(h, spacing=16.0)[:, :, None]
    col = col * (1.0 - 0.16 * contours)
    steep = np.clip((slope_map(h) - 3.0) / 12.0, 0.0, 1.0)[:, :, None]
    col = col * (1.0 - 0.20 * steep)

    # subtle paper grain (deterministic)
    grain = fbm(size, seed + 9191, octaves=3, base_cells=48.0) - 0.5
    col = col + grain[:, :, None] * 7.0

    out = Image.fromarray(np.clip(col, 0, 255).astype(np.uint8), 'RGB')

    # semantic overlays from the layout PNG (roads / zones / markers)
    if overlays is not None:
        from PIL import ImageDraw, ImageFilter
        lay = np.asarray(overlays.convert('RGB').resize((size, size), Image.NEAREST))
        lr = lay[:, :, 0]
        base = np.asarray(out, dtype=np.float64)

        # Roads: thin translucent parchment ribbon rather than a hard bar.
        road = (lr == 200).astype(np.float64)
        road_blur = np.asarray(
            Image.fromarray((road * 255).astype(np.uint8), 'L')
            .filter(ImageFilter.GaussianBlur(1.1)), dtype=np.float64) / 255.0
        a = (road_blur * 0.55)[:, :, None]
        base = base * (1 - a) + np.array([214.0, 190.0, 140.0]) * a

        # Combat zones: soft warm wash + a dashed ring, not a solid disc.
        zone = np.isin(lr, [60, 80, 100, 120]).astype(np.uint8)
        wash = np.asarray(
            Image.fromarray((zone * 255).astype(np.uint8), 'L')
            .filter(ImageFilter.GaussianBlur(7.0)), dtype=np.float64) / 255.0
        a = (wash * 0.30)[:, :, None]
        base = base * (1 - a) + np.array([186.0, 96.0, 72.0]) * a

        # Small point markers (event / reward / portal): keep crisp but small.
        point_colors = [(220, (240, 206, 92)), (240, (92, 202, 214)), (250, (230, 112, 188))]
        for val, c in point_colors:
            m = (lr == val).astype(np.float64)
            if m.max() <= 0:
                continue
            glow = np.asarray(
                Image.fromarray((m * 255).astype(np.uint8), 'L')
                .filter(ImageFilter.GaussianBlur(3.0)), dtype=np.float64) / 255.0
            a = (glow * 0.42)[:, :, None]
            base = base * (1 - a) + np.asarray(c, dtype=np.float64) * a

        out = Image.fromarray(np.clip(base, 0, 255).astype(np.uint8), 'RGB')

        # dashed rings around combat zones, drawn from connected blob centres
        d = ImageDraw.Draw(out)
        zone_img = Image.fromarray((zone * 255).astype(np.uint8), 'L')
        # approximate each zone by its bounding box of the largest blobs
        try:
            import cv2  # optional
        except ImportError:
            cv2 = None
        if cv2 is None:
            # cheap fallback: use the erosion boundary of the whole zone mask
            edge = np.asarray(zone_img.filter(ImageFilter.MaxFilter(5)), dtype=np.int16) - \
                   np.asarray(zone_img.filter(ImageFilter.MinFilter(5)), dtype=np.int16)
            ys, xs = np.where(edge > 0)
            step = max(1, len(xs) // 2600)
            for (xx, yy) in zip(xs[::step], ys[::step]):
                if (int(xx) + int(yy)) % 9 < 5:
                    d.point((int(xx), int(yy)), fill=(206, 168, 132))
        else:
            n, labels, stats, cents = cv2.connectedComponentsWithStats(zone, 8)
            for i in range(1, n):
                cx, cy, w, h, area = stats[i]
                if area < 120:
                    continue
                r = int(max(w, h) * 0.52)
                for deg in range(0, 360, 12):
                    import math as _m
                    ang = _m.radians(deg)
                    d.ellipse((cx + w * 0.5 + r * _m.cos(ang) - 1,
                               cy + h * 0.5 + r * _m.sin(ang) - 1,
                               cx + w * 0.5 + r * _m.cos(ang) + 1,
                               cy + h * 0.5 + r * _m.sin(ang) + 1),
                              fill=(206, 168, 132))

    if title:
        from PIL import ImageDraw
        import draw_maps as dm
        d = ImageDraw.Draw(out)
        # compact parchment cartouche instead of a hard black bar
        w = int(d.textlength(title, font=dm.F(24))) + 28
        d.rounded_rectangle([12, 12, 12 + w, 58], radius=6,
                            fill=(28, 26, 22, 220) if out.mode == 'RGBA' else (28, 26, 22))
        d.text((26, 18), title, font=dm.F(24), fill=(238, 228, 200))
    return out
