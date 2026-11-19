# -*- coding: utf-8 -*-
"""
Stage 2 art generator: crop each battlefield's 32x32 world-space
neighbourhood out of its tile's 256x256 design PNG, magnify x16 into a
256x256 source PNG for the (still 512x512) Unity terrain, and add
deterministic fine detail so 640 battlefields do not look identical.

Channel contract (same as draw_all_tiles.py gen_terrain):
  R = height (0..255 -> 0..TerrainHeight)
  G = alphamap material band
  B = walkability mask

Outputs, per battlefield grid_key ({tid}_s{slot:02d}):
  tiles/{grid_key}_terrain.png          256x256 RGB magnified neighbourhood
  tiles/{grid_key}_minimap.png          512x512 RGB coarse overview
  tiles/minimap640/{grid_key}_minimap.png
  MapData/{grid_key}_areas.txt          battlefield-local area hints

The 16x16 source pixels carry only the broad shape. Three octaves of
value noise (seeded from grid_key, rendered with PIL's C-level resize)
supply the "more detail" the design asks for. Amplitude stays within
+/-9/255 on R so the world silhouette, relief and walkability survive.
"""
import os
import random
from PIL import Image, ImageChops, ImageDraw

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
CLIENT = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'MapData')
MINIMAP_DIR = os.path.join(TILES, 'minimap640')
SLOTS = os.path.join(DESIGN, 'battle_slots.txt')

SRC = 256                          # design PNG resolution, 1 px = 2 world units
PX_PER_UNIT = SRC / 512.0
NEIGHBOURHOOD = 32.0
WINDOW = int(NEIGHBOURHOOD * PX_PER_UNIT)   # 16 source pixels
MAGNIFY = 16                       # 32 world units -> 512 world units


def slots():
    out = []
    with open(SLOTS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            out.append(dict(level_id=int(p[0]), tid=p[1], slot=int(p[2]),
                            grid_key=p[3], era=p[4], kind=p[5],
                            x=float(p[6]), z=float(p[7]), title=p[11]))
    return out


def seed_of(key):
    h = 2166136261
    for ch in key:
        h ^= ord(ch)
        h = (h * 16777619) & 0xFFFFFFFF
    return h


def detail_noise(seed):
    """Three smoothed octaves, each upscaled by PIL (C level), combined to 0..18."""
    rnd = random.Random(seed)
    acc = None
    for cell, weight in ((8, 0.55), (24, 0.30), (64, 0.15)):
        n = SRC // cell + 2
        small = Image.frombytes('L', (n, n),
                                bytes(rnd.randrange(256) for _ in range(n * n)))
        big = small.resize((SRC, SRC), Image.BILINEAR)
        acc = big if acc is None else Image.blend(acc, big, weight)
    lut = [int(round(i / 255.0 * 18.0)) for i in range(256)]
    return acc.point(lut)


def build_terrain(src_im, s):
    w, h = src_im.size
    cx = s['x'] * PX_PER_UNIT
    cy = s['z'] * PX_PER_UNIT
    x0 = int(max(0, min(w - WINDOW, cx - WINDOW / 2.0)))
    y0 = int(max(0, min(h - WINDOW, cy - WINDOW / 2.0)))
    window = src_im.crop((x0, y0, x0 + WINDOW, y0 + WINDOW))
    big = window.resize((SRC, SRC), Image.BILINEAR)
    r, g, b = big.split()
    noise = detail_noise(seed_of(s['grid_key']) ^ (0x9E37 * (s['slot'] + 1)))
    r = ImageChops.add(r, noise, scale=1.0, offset=-9)
    return Image.merge('RGB', (r, g, b))


def build_minimap(terrain):
    im = terrain.resize((512, 512), Image.BILINEAR).convert('RGB')
    px = im.load()
    for y in range(512):
        for x in range(512):
            r, g, b = px[x, y]
            if r < 70:
                c = (60, 96, 130)
            elif r < 110:
                c = (96, 118, 96)
            elif r < 155:
                c = (140, 128, 92)
            else:
                c = (168, 162, 150)
            px[x, y] = (int(c[0] * 0.5 + r * 0.5),
                        int(c[1] * 0.5 + g * 0.5),
                        int(c[2] * 0.5 + b * 0.5))
    d = ImageDraw.Draw(im)
    d.ellipse((248, 248, 264, 264), fill=(230, 200, 70), outline=(60, 50, 20))
    return im


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
        src = os.path.join(TILES, s['tid'] + '_terrain.png')
        if not os.path.exists(src):
            print('MISS_SRC', s['tid'])
            continue
        if s['tid'] not in cache:
            cache[s['tid']] = Image.open(src).convert('RGB')
        terrain = build_terrain(cache[s['tid']], s)
        terrain.save(os.path.join(TILES, s['grid_key'] + '_terrain.png'))
        mini = build_minimap(terrain)
        mini.save(os.path.join(TILES, s['grid_key'] + '_minimap.png'))
        mini.save(os.path.join(MINIMAP_DIR, s['grid_key'] + '_minimap.png'))
        write_battle_areas(s)
        n += 1
    print('BATTLEFIELD_ART battlefields=%d out=%dx%d window=%dpx magnify=x%d' %
          (n, SRC, SRC, WINDOW, MAGNIFY))
    print('terrain_png=%d minimap_png=%d areas=%d' % (n, n, n))


if __name__ == '__main__':
    main()
