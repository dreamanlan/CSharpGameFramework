# -*- coding: utf-8 -*-
"""
Stage 2+ battlefield art generator (design doc 43).

Each of the 640 battlefields is a 512x512 world-unit scene authored at the
fixed battlefield origin (SceneOrigin.BattleOffset). Instead of cropping a
16x16 window out of its tile's design PNG and magnifying it x16 (which only
produced smooth gradient blocks), we now synthesise a full 256x256 terrain
procedurally:

  * a tile-level low-frequency field keeps the 10 battlefields of one tile
    geographically related;
  * a slot-level fBm + ridged + domain-warped field gives every battlefield
    its own ridges, gullies and material variation;
  * the shared compose() pipeline carves rivers, erodes, and derives the
    material band from height/slope/moisture;
  * a soft, irregularly shaped combat plateau around the fixed spawn point
    (world 60,70 -> pixel 30,35) keeps the two-wave encounter playable.

Channel contract (unchanged):
  R = height (0..255 -> 0..TerrainHeight)
  G = alphamap material band (continuous)
  B = walkability mask

Outputs per grid_key ({tid}_s{slot:02d}):
  tiles/{grid_key}_terrain.png          256x256 RGB
  tiles/{grid_key}_minimap.png          512x512 hillshaded map
  tiles/minimap640/{grid_key}_minimap.png
  MapData/{grid_key}_areas.txt          battlefield-local area hints

build_story.py reads the spawn height from {grid_key}_terrain.png, so it must
be re-run after this generator (and dslcopy.bat afterwards).
"""
import os
import math
import numpy as np
from PIL import Image, ImageDraw, ImageFilter

import terrain_synth as ts
from draw_all_tiles import PROFILE

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
CLIENT = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'MapData')
MINIMAP_DIR = os.path.join(TILES, 'minimap640')
SLOTS = os.path.join(DESIGN, 'battle_slots.txt')

SRC = 256                          # design PNG resolution, 1 px = 2 world units
SPAWN_PX = (30, 35)                # world (60, 70) / 2
PLATFORM_RADIUS = 26.0             # px = 52 world units; covers wave-2 advance
PLATFORM_FALLOFF = 26.0            # wider blend so the plateau reads as a
                                   # natural clearing instead of a disc
PLATFORM_HEIGHT = 62.0             # R value, ~17 world units


def slots():
    out = []
    with open(SLOTS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            out.append(dict(level_id=int(p[0]), tid=p[1], slot=int(p[2]),
                            grid_key=p[3], era=p[4], kind=p[5],
                            x=float(p[6]), z=float(p[7]), title=p[11],
                            code=p[12], diff=int(p[15])))
    return out


def seed_of(key):
    h = 2166136261
    for ch in key:
        h ^= ord(ch)
        h = (h * 16777619) & 0xFFFFFFFF
    return h


def tile_seed(tid):
    return seed_of('tile:' + tid)


def build_terrain(s, cache):
    size = SRC
    tseed = tile_seed(s['tid'])
    sseed = seed_of(s['grid_key'])

    # Cache the tile-level low-frequency field: 10 slots share it.
    if s['tid'] not in cache:
        cache[s['tid']] = ts.fbm(size, tseed, octaves=4, base_cells=2.5)
    low = cache[s['tid']]

    # Slot-specific detail. The (x,z) slot centre phases the field so nearby
    # slots are related without looking identical.
    detail = ts.fbm(size, sseed, octaves=6, base_cells=4.0)
    phase = (s['x'] + s['z']) / 1024.0
    detail = 0.82 * detail + 0.18 * np.roll(
        np.roll(detail, int(phase * size * 0.35), axis=1),
        int(phase * size * 0.21), axis=0)
    field = 0.48 * low + 0.52 * detail

    code = s['code'] if s['code'] in PROFILE else 'ARR'
    p = PROFILE[code]
    relief = p['relief'] * (0.80 + 0.20 * s['diff'] / 5.0)
    rgb = ts.compose(
        field, sseed,
        base=p['base'], relief=relief, bias=p['bias'],
        rivers=p['rivers'], craters=p['craters'], islands=p['islands'],
        dunes=p['dunes'], terraces=p['terraces'], boundary=True,
        target_water=0.42 if p['islands'] else 0.09)

    # ---- soft combat plateau ---------------------------------------------
    # The story always spawns the party at world (60,70) and advances ~20
    # world units in wave 2. Blend the terrain towards a walkable, gently
    # undulating plateau with an irregular boundary (no visible hard disc).
    yy, xx = np.mgrid[0:size, 0:size].astype(np.float64)
    d = np.hypot(xx - SPAWN_PX[0], yy - SPAWN_PX[1])
    wobble = (ts.value_noise(size, 7.0, sseed + 31) - 0.5) * 12.0
    t = np.clip((d - (PLATFORM_RADIUS + wobble)) / PLATFORM_FALLOFF, 0.0, 1.0)
    t = t * t * (3.0 - 2.0 * t)
    micro = (ts.fbm(size, sseed + 77, octaves=3, base_cells=24.0) - 0.5) * 7.0
    plateau = PLATFORM_HEIGHT + micro
    rgb[:, :, 0] = rgb[:, :, 0] * t + plateau * (1.0 - t)
    rgb[:, :, 2] = np.where(t < 0.55, 255.0, rgb[:, :, 2])
    # keep the material band coherent with the new height
    slope = ts.slope_map(rgb[:, :, 0])
    g = np.clip((rgb[:, :, 0] - 60.0) / 90.0, 0.0, 1.0) * 120.0
    g += np.clip((slope - 1.6) / 7.0, 0.0, 1.0) * 75.0
    g -= (ts.fbm(size, sseed + 888, octaves=4, base_cells=5.0) - 0.5) * 34.0
    g += p['bias'] * 16.0
    rgb[:, :, 1] = rgb[:, :, 1] * t + np.clip(g, 8.0, 250.0) * (1.0 - t)
    return ts.to_image(rgb)


def build_minimap(terrain, s):
    """Hillshaded battlefield map with a subtle spawn marker."""
    img = ts.cartographic_minimap(terrain, size=512, seed=seed_of(s['grid_key']))
    d = ImageDraw.Draw(img)
    # spawn ring at (60,70) world -> 512px map centre offset
    px, py = int(SPAWN_PX[0] * 2), int(SPAWN_PX[1] * 2)
    r = 11
    d.ellipse((px - r, py - r, px + r, py + r), outline=(238, 214, 128), width=2)
    d.ellipse((px - 2, py - 2, px + 2, py + 2), fill=(248, 226, 140))
    # compass rose in a corner, drawn small and unobtrusive
    cx, cy, cr = 512 - 34, 34, 15
    d.ellipse((cx - cr, cy - cr, cx + cr, cy + cr), outline=(226, 214, 188), width=1)
    d.line((cx, cy - cr + 3, cx, cy + cr - 3), fill=(226, 214, 188), width=1)
    d.line((cx - cr + 3, cy, cx + cr - 3, cy), fill=(226, 214, 188), width=1)
    d.polygon([(cx, cy - cr + 2), (cx - 3, cy), (cx + 3, cy)], fill=(238, 214, 128))
    return img


def write_battle_areas(s):
    """Battlefield-local hints: one combat zone, the event, a reward, a portal."""
    p = os.path.join(CLIENT, s['grid_key'] + '_areas.txt')
    lines = [
        '# battlefield-local area hints (design doc 35)',
        '# type | worldX | worldZ | radius | intensity(G) | trigger(B) | pixels',
        '60 | 256.0 | 256.0 | 45.0 | 100 | 255 | 1595',
        '220 | 256.0 | 256.0 | 8.9 | 120 | 160 | 113',
        '240 | 320.0 | 320.0 | 10.2 | 150 | 80 | 81',
        '250 | 60.0 | 70.0 | 13.8 | 0 | 0 | 149',
    ]
    with open(p, 'w', encoding='utf-8') as f:
        f.write('\n'.join(lines) + '\n')


def main():
    os.makedirs(MINIMAP_DIR, exist_ok=True)
    os.makedirs(CLIENT, exist_ok=True)
    rows = slots()
    cache = {}
    n = 0
    for s in rows:
        terrain = build_terrain(s, cache)
        terrain.save(os.path.join(TILES, s['grid_key'] + '_terrain.png'))
        mini = build_minimap(terrain, s)
        mini.save(os.path.join(TILES, s['grid_key'] + '_minimap.png'))
        mini.save(os.path.join(MINIMAP_DIR, s['grid_key'] + '_minimap.png'))
        write_battle_areas(s)
        n += 1
    print('BATTLEFIELD_ART battlefields=%d out=%dx%d mode=procedural' % (n, SRC, SRC))
    print('terrain_png=%d minimap_png=%d areas=%d' % (n, n, n))


if __name__ == '__main__':
    main()
