# -*- coding: utf-8 -*-
"""
Generate tangent-space normal maps for the four TerrainLayer diffuse textures
used by Assets/Terrain/Level2.asset. No external art asset is required: the
normal is derived from the luminance gradient of the existing 512x512 albedo,
which is exactly the kind of micro-relief a terrain normal map should carry.

Outputs (Unity imports them as Texture2D with type=NormalMap via the patcher):
  Unity3d/Assets/Resources/TerrainNormals/<stem>_normal.png
"""
import os
import numpy as np
from PIL import Image, ImageFilter

ROOT = r'D:\GitHub\CSharpGameFramework'
BRUSH = os.path.join(ROOT, 'Unity3d', 'Assets', 'Brush')
OUT = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'TerrainNormals')

LAYERS = [
    'LvZhouCaoDi_01',
    'ChengZhen_DiZhuan_02',
    'LvZhouSha_01',
    'YiZhan_HuangTuDi_02',
]


def sobel_normal(lum, strength=2.4):
    """Height (luminance) -> tangent-space normal, RGB8."""
    p = np.pad(lum, 1, mode='edge')
    gx = (p[1:-1, 2:] - p[1:-1, :-2]) * 0.5
    gy = (p[2:, 1:-1] - p[:-2, 1:-1]) * 0.5
    nx = -gx * strength / 255.0
    ny = -gy * strength / 255.0
    nz = np.ones_like(nx)
    ln = np.sqrt(nx * nx + ny * ny + nz * nz)
    nx, ny, nz = nx / ln, ny / ln, nz / ln
    rgb = np.stack([(nx * 0.5 + 0.5) * 255.0,
                    (ny * 0.5 + 0.5) * 255.0,
                    (nz * 0.5 + 0.5) * 255.0], axis=-1)
    return Image.fromarray(np.clip(rgb, 0, 255).astype(np.uint8), 'RGB')


def main():
    os.makedirs(OUT, exist_ok=True)
    made = 0
    for stem in LAYERS:
        src = None
        for ext in ('.jpg', '.png', '.tga'):
            p = os.path.join(BRUSH, stem + ext)
            if os.path.exists(p):
                src = p
                break
        if src is None:
            print('MISS_DIFFUSE', stem)
            continue
        im = Image.open(src).convert('RGB')
        # slight blur removes single-pixel noise that would read as glitter
        lum = np.asarray(im.convert('L').filter(ImageFilter.GaussianBlur(0.6)),
                         dtype=np.float64)
        normal = sobel_normal(lum)
        dst = os.path.join(OUT, stem + '_normal.png')
        normal.save(dst)
        made += 1
        print('NORMAL', stem, im.size, '->', os.path.basename(dst))
    print('TERRAIN_NORMALS made=%d' % made)


if __name__ == '__main__':
    main()
