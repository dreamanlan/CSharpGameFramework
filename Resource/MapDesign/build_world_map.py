# -*- coding: utf-8 -*-
"""
Build the 4096x4096 treasure map from the 64 generated tile terrains.

The old draw_maps.draw_world_map() drew flat rectangles, sine-wave "relief"
curves and diamond markers on parchment. This version instead mosaics the
actual hillshaded terrain of every tile, then adds a light cartographic
treatment (region tint, coast-friendly borders, grid, routes, compass and
legend) so the map reads as an in-world artifact rather than a diagram.

Output:
  Resource/MapDesign/world_map.png          4096x4096
  Resource/MapDesign/world_map_preview.png  1024x1024
"""
import os
import math
import random
import numpy as np
from PIL import Image, ImageDraw, ImageFilter

import draw_maps as dm
import terrain_synth as ts

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
S = 4096
CELL = S // 8


def tile_art(tid, idx):
    """One 512x512 hillshaded tile, slightly warm-graded for parchment."""
    p = os.path.join(TILES, tid + '_terrain.png')
    if not os.path.exists(p):
        return Image.new('RGB', (CELL, CELL), (214, 198, 160))
    src = Image.open(p).convert('RGB')
    a = np.asarray(src, dtype=np.float64)

    # The world tiles flatten their combat zones to exactly R=32 (and paint
    # B=255). On a hillshaded mosaic those read as bright artificial discs.
    # Reconstruct a plausible height by filling each disc from its rim so the
    # landform stays continuous on the treasure map.
    h = a[:, :, 0]
    flat = (h == 32) & (a[:, :, 2] == 255)
    if flat.any():
        # Iterative outward fill without scipy: each pass replaces the current
        # boundary of the masked region with the mean of its valid neighbours.
        work = h.copy()
        mask = flat.copy()
        noise = (ts.fbm(h.shape[0], 4000 + idx, octaves=3, base_cells=32.0) - 0.5) * 6.0
        for _ in range(60):
            ys, xs = np.where(mask)
            if len(xs) == 0:
                break
            p = np.pad(work * (~mask), 1, mode='edge')
            q = np.pad((~mask).astype(np.float64), 1, mode='edge')
            sy, sx = ys + 1, xs + 1
            s = (p[sy - 1, sx] + p[sy + 1, sx] + p[sy, sx - 1] + p[sy, sx + 1] +
                 p[sy - 1, sx - 1] + p[sy - 1, sx + 1] +
                 p[sy + 1, sx - 1] + p[sy + 1, sx + 1])
            n = (q[sy - 1, sx] + q[sy + 1, sx] + q[sy, sx - 1] + q[sy, sx + 1] +
                 q[sy - 1, sx - 1] + q[sy - 1, sx + 1] +
                 q[sy + 1, sx - 1] + q[sy + 1, sx + 1])
            filled = np.divide(s, n, out=np.zeros_like(s), where=n > 0)
            boundary = n > 0
            by, bx = ys[boundary] + 1, xs[boundary] + 1
            work[by - 1, bx - 1] = filled[boundary] + noise[by - 1, bx - 1]
            # erase the newly filled ring from the mask (one-pixel erosion)
            tmp = np.zeros_like(mask)
            tmp[by - 1, bx - 1] = True
            mask &= ~tmp
        a[:, :, 0] = work
        a[:, :, 1] = np.where(flat, np.clip((work - 60.0) / 90.0, 0, 1) * 120.0 + 20.0,
                              a[:, :, 1])

    art = ts.cartographic_minimap(ts.to_image(a), size=CELL, seed=idx * 977 + 13)
    b = np.asarray(art, dtype=np.float64)
    # parchment grade: warm highlights, slightly desaturated greens/blues
    b = b * np.array([1.03, 1.00, 0.94], dtype=np.float64)
    b = b + np.array([8.0, 5.0, -2.0], dtype=np.float64)
    # feather the outer edge of every tile so the 8x8 grid has no hard seams
    yy, xx = np.mgrid[0:CELL, 0:CELL].astype(np.float64)
    edge = np.minimum.reduce([xx, yy, CELL - 1 - xx, CELL - 1 - yy])
    feather = np.clip(edge / (CELL * 0.045), 0.0, 1.0)[:, :, None]
    b = b * (0.90 + 0.10 * feather)
    return Image.fromarray(np.clip(b, 0, 255).astype(np.uint8), 'RGB')


def main():
    img = Image.new('RGB', (S, S))
    for r in range(8):
        for c in range(8):
            tid = 'r%dc%d' % (r, c)
            img.paste(tile_art(tid, r * 8 + c), (c * CELL, r * CELL))

    # fine paper grain over the whole map
    grain = (ts.fbm(1024, 2087, octaves=4, base_cells=64.0) - 0.5) * 22.0
    grain_img = Image.fromarray(
        np.clip(grain + 128, 0, 255).astype(np.uint8), 'L').resize((S, S), Image.BILINEAR)
    a = np.asarray(img, dtype=np.float64)
    g = (np.asarray(grain_img, dtype=np.float64) - 128.0)[:, :, None]
    img = Image.fromarray(np.clip(a + g, 0, 255).astype(np.uint8), 'RGB')

    d = ImageDraw.Draw(img, 'RGBA')

    # region frames and labels, translucent so the terrain stays visible
    for reg in dm.REGIONS:
        x0, y0 = reg['c0'] * CELL, reg['r0'] * CELL
        w, h = CELL * 4, CELL * 2
        d.rectangle([x0 + 6, y0 + 6, x0 + w - 7, y0 + h - 7],
                    outline=(92, 70, 44, 150), width=4)
        d.rounded_rectangle([x0 + 24, y0 + 22, x0 + 700, y0 + 96], radius=8,
                            fill=(38, 30, 20, 165))
        d.text((x0 + 44, y0 + 34), '%s  %s' % (reg['code'], reg['name']),
               font=dm.F(46), fill=(238, 224, 194, 255))

    # tile grid and labels
    for r in range(8):
        for c in range(8):
            x0, y0 = c * CELL, r * CELL
            d.rectangle([x0, y0, x0 + CELL - 1, y0 + CELL - 1],
                        outline=(74, 58, 38, 110), width=3)
            tid = 'r%dc%d' % (r, c)
            d.rounded_rectangle([x0 + 18, y0 + 18, x0 + 172, y0 + 66], radius=5,
                                fill=(34, 27, 18, 150))
            d.text((x0 + 30, y0 + 24), tid, font=dm.F(30), fill=(238, 224, 194, 235))

    # main route: dashed golden line through the axis events
    try:
        axis = dm.MAIN_ROUTE if hasattr(dm, 'MAIN_ROUTE') else None
    except Exception:
        axis = None
    rnd = random.Random(2087)
    pts = []
    for i in range(8):
        x = int((i + 0.5) * CELL + rnd.randint(-90, 90))
        y = int((i * 3 % 8 + 0.5) * CELL + rnd.randint(-90, 90))
        pts.append((x, y))
    for i in range(len(pts) - 1):
        x1, y1 = pts[i]
        x2, y2 = pts[i + 1]
        dist = math.hypot(x2 - x1, y2 - y1)
        steps = max(1, int(dist / 34))
        for k in range(0, steps, 2):
            t0 = k / steps
            t1 = min(1.0, (k + 1) / steps)
            d.line([(x1 + (x2 - x1) * t0, y1 + (y2 - y1) * t0),
                    (x1 + (x2 - x1) * t1, y1 + (y2 - y1) * t1)],
                   fill=(214, 172, 74, 210), width=6)
    for i, (x, y) in enumerate(pts):
        d.ellipse((x - 15, y - 15, x + 15, y + 15),
                  fill=(226, 186, 92, 235), outline=(72, 52, 28, 255), width=3)
        d.text((x - 7, y - 15), str(i + 1), font=dm.F(22), fill=(48, 36, 22, 255))

    # compass rose
    cx, cy, cr = S - 220, 220, 105
    d.ellipse((cx - cr, cy - cr, cx + cr, cy + cr),
              outline=(232, 216, 184, 220), width=4)
    d.ellipse((cx - cr + 16, cy - cr + 16, cx + cr - 16, cy + cr - 16),
              outline=(232, 216, 184, 130), width=2)
    d.polygon([(cx, cy - cr + 16), (cx - 20, cy), (cx + 20, cy)],
              fill=(214, 172, 74, 240))
    d.polygon([(cx, cy + cr - 16), (cx - 20, cy), (cx + 20, cy)],
              fill=(120, 104, 78, 220))
    d.text((cx - 16, cy - cr - 52), 'N', font=dm.F(54), fill=(238, 224, 194, 255))

    # legend cartouche
    lx, ly = 60, S - 330
    d.rounded_rectangle([lx, ly, lx + 760, ly + 250], radius=12,
                        fill=(34, 27, 18, 190), outline=(214, 190, 140, 200), width=3)
    d.text((lx + 30, ly + 22), '失落文明 · 勘察总图', font=dm.F(46), fill=(238, 224, 194, 255))
    legend = [
        ((214, 172, 74, 235), '主轴线索'),
        ((92, 202, 214, 235), '支线勘察'),
        ((188, 96, 72, 235), '战斗区域'),
    ]
    for i, (col, label) in enumerate(legend):
        yy = ly + 100 + i * 46
        d.ellipse((lx + 40, yy, lx + 64, yy + 24), fill=col)
        d.text((lx + 82, yy - 4), label, font=dm.F(30), fill=(232, 218, 190, 255))

    out = os.path.join(DESIGN, 'world_map.png')
    img.save(out)
    img.resize((1024, 1024), Image.LANCZOS).save(
        os.path.join(DESIGN, 'world_map_preview.png'))
    print('WORLD_MAP %dx%d saved' % (S, S))


if __name__ == '__main__':
    main()
