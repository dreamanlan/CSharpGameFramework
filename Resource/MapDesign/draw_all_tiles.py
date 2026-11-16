# -*- coding: utf-8 -*-
"""
按 regions.txt 为 8x8 = 64 格批量生成输入图（阶段 0 铺开）。

每格产出：
  {tid}_terrain.png             物理图 R 高度 / G 材质 / B 功能 mask
  {tid}_layout.png              现代调查层（2087）布局图
  {tid}_layout_{era}.png        该格主时代层布局图（同一地形、不同区域）
  {tid}_minimap.png             现代层小地图静态底图
  {tid}_minimap_{era}.png       主时代层小地图静态底图
  tiles_check.txt               逐格自检与汇总

设计意图全部画在图里；生成器只做通道翻译。传送点固定在 (30,35) = 世界 (60,70)。
"""
import math
import os
import random
from collections import deque
from PIL import Image, ImageDraw
import draw_maps as dm

W = dm.W
TILES = dm.TILES
ROOT = dm.ROOT

ERA = {
    'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
    'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
}

# 地貌配方：base 基底高度 / relief 起伏 / 特征开关
PROFILE = {
    # bias 抬低材质基线：+1 偏岩雪，-1 偏草，用来拉开区域观感
    'ARR': dict(base=96, relief=105, rivers=0, craters=0, dunes=0, terraces=0, islands=0, bias=1),
    'ORB': dict(base=68, relief=78,  rivers=0, craters=2, dunes=0, terraces=0, islands=0, bias=1),
    'LEV': dict(base=62, relief=92,  rivers=1, craters=0, dunes=0, terraces=1, islands=0, bias=0),
    'MES': dict(base=24, relief=62,  rivers=2, craters=0, dunes=0, terraces=0, islands=0, bias=-1),
    'EGY': dict(base=46, relief=80,  rivers=1, craters=0, dunes=2, terraces=0, islands=0, bias=0),
    'ABZ': dict(base=72, relief=90,  rivers=0, craters=3, dunes=0, terraces=0, islands=0, bias=1),
    'AMR': dict(base=56, relief=118, rivers=1, craters=0, dunes=0, terraces=2, islands=0, bias=-1),
    'PAC': dict(base=20, relief=70,  rivers=0, craters=0, dunes=0, terraces=0, islands=3, bias=0),
}

# 材质 G：低地 / 中地 / 高地 / 雪线
MATERIAL = {
    'ARR': (34, 96, 170, 232), 'ORB': (30, 150, 175, 226),
    'LEV': (30, 96, 160, 220), 'MES': (34, 96, 150, 226),
    'EGY': (30, 95, 150, 220), 'ABZ': (30, 100, 170, 226),
    'AMR': (34, 80, 150, 220), 'PAC': (30, 95, 155, 220),
}


def seed_of(tid):
    return sum((i + 1) * ord(ch) for i, ch in enumerate(tid))


def parse_regions():
    rows = []
    with open(os.path.join(ROOT, 'regions.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            rows.append(dict(tid=p[0], code=p[1], name=p[2], era_text=p[3],
                             route=p[4], pts=int(p[5]), src=p[6],
                             diff=int(p[7]), seq=int(p[8])))
    return rows


def gen_points(row, era_seed):
    """返回 areas / events / rewards / portal / roads；道路保证从传送点连通所有节点。"""
    rnd = random.Random(seed_of(row['tid']) * 7 + era_seed)
    portal = (30, 35)
    n_area = 3 if row['route'] == '主线' else 2
    n_event = 3 if row['route'] == '主线' else 2
    n_reward = 4 if row['route'] == '主线' else 3

    def placed(count, min_gap, pad):
        out = []
        for _ in range(count):
            for _try in range(60):
                x = rnd.randint(pad, W - pad)
                y = rnd.randint(pad, W - pad)
                if (x - portal[0]) ** 2 + (y - portal[1]) ** 2 < 42 ** 2:
                    continue
                if any((x - a[0]) ** 2 + (y - a[1]) ** 2 < min_gap ** 2 for a in out):
                    continue
                out.append((x, y))
                break
            else:
                out.append((rnd.randint(pad, W - pad), rnd.randint(pad, W - pad)))
        return out

    area_pts = placed(n_area, 66, 62)
    areas = [(x, y, rnd.randint(19, 25)) for x, y in area_pts]
    events = placed(n_event, 40, 45)
    rewards = placed(n_reward, 30, 40)

    nodes = [portal] + [(a[0], a[1]) for a in areas] + events + rewards
    roads = []
    connected = [portal]
    for n in nodes[1:]:
        near = min(connected, key=lambda q: (q[0] - n[0]) ** 2 + (q[1] - n[1]) ** 2)
        roads.append((near, n))
        connected.append(n)
    return areas, events, rewards, portal, roads


def gen_terrain(row, all_areas):
    """R 高度 / G 材质 / B 功能 mask；战斗区内部高度恒定、边界封边。"""
    code = row['code']
    p = PROFILE[code]
    m = MATERIAL[code]
    rnd = random.Random(seed_of(row['tid']))
    ph = rnd.random() * 6.283
    img = Image.new('RGB', (W, W))
    px = img.load()
    diff = row['diff'] / 5.0

    rivers = []
    for k in range(p['rivers']):
        rivers.append((60 + 62 * k + rnd.randint(-12, 12), 0.022 + 0.008 * k, rnd.random() * 6.28))
    craters = [(rnd.randint(50, 206), rnd.randint(50, 206), rnd.randint(22, 40))
               for _ in range(p['craters'])]
    islands = [(rnd.randint(50, 206), rnd.randint(50, 206), rnd.randint(28, 46))
               for _ in range(p['islands'])]

    for y in range(W):
        for x in range(W):
            nx, ny = x / W, y / W
            h = p['base'] + p['relief'] * 0.30 * (
                math.sin(nx * 7.0 + ph) * math.cos(ny * 6.0 + ph * 0.7) +
                0.5 * math.sin(nx * 15.0 - ny * 11.0 + ph * 1.7))
            # 河道 / 干河床
            for (cy, freq, phase) in rivers:
                riv = abs(y - (cy + 26 * math.sin(x * freq + phase)))
                if riv < 9:
                    h = min(h, 8 + riv * 0.6)
            # 陨坑 / 矿坑 / 竖井
            for (cx, cy, rad) in craters:
                d = math.hypot(x - cx, y - cy)
                if d < rad:
                    h -= (1 - d / rad) * 42
                elif d < rad * 1.35:
                    h += (1 - abs(d - rad * 1.15) / (rad * 0.35)) * 22
            # 群岛
            if p['islands']:
                near = min(math.hypot(x - cx, y - cy) - rad for cx, cy, rad in islands)
                if near > 0:
                    h = max(4, 16 - near * 0.9)
                else:
                    h += min(52, -near * 1.4)
            # 沙丘
            if p['dunes']:
                h += 18 * (0.5 + 0.5 * math.sin(x * 0.09 + y * 0.035 + ph))
            # 梯田
            if p['terraces']:
                h += (math.floor(math.hypot(x - 128, y - 128) / 22) % 4) * 11
            # 外围山脊
            d = math.hypot(x - 128, y - 128)
            if d > 98:
                h += min(78 * diff, (d - 98) * 1.05) * (0.6 + 0.4 * math.sin(x * 0.06 + y * 0.04))
            # 边界封边：缓坡 + 不规则起伏，避免整齐的方形高墙
            edge = min(x, y, W - 1 - x, W - 1 - y)
            boundary = edge < 14
            if boundary:
                h = max(h, 118 + (14 - edge) * 3.2 +
                        16 * math.sin(x * 0.09) * math.cos(y * 0.075))
            R = int(max(0, min(255, h)))
            idx = 0 if R < 24 else 1 if R < 80 else 2 if R < 155 else 3
            idx = max(0, min(3, idx + p['bias']))
            G = m[2] if boundary else m[idx]      # 边界用岩，不用雪白
            B = 255 if R < 85 else 128
            if R < 18:            # 水面不可通行；跨水只能走布局图里的道路/桥
                B = 0
            if boundary or R > 175:
                B = 0
            px[x, y] = (R, G, B)

    # 两套时代层的所有战斗区都要落在可走平台上（物理层共用）。
    # 只压平高度、保留原地材质，避免战斗区变成突兀的纯色圆盘。
    for cx, cy, rad in all_areas:
        for yy in range(max(0, int(cy - rad)), min(W, int(cy + rad + 1))):
            for xx in range(max(0, int(cx - rad)), min(W, int(cx + rad + 1))):
                if (xx - cx) ** 2 + (yy - cy) ** 2 <= rad * rad:
                    g0 = px[xx, yy][1]
                    px[xx, yy] = (32, g0, 255)
    return img


def fast_minimap(terrain, layout, path, title):
    small = Image.new('RGB', (W, W))
    sp, tp, lp = small.load(), terrain.load(), layout.load()
    for y in range(W):
        for x in range(W):
            R, G, B = tp[x, y]
            lr = lp[x, y][0]
            if lr in (60, 80, 100, 120):
                base = (190, 84, 68)
            elif lr == 200:
                base = (196, 168, 108)
            elif lr == 220:
                base = (232, 196, 66)
            elif lr == 240:
                base = (70, 186, 200)
            elif lr == 250:
                base = (222, 92, 176)
            else:
                # G 决定材质色相，R 决定明度：不同区地貌直接体现在小地图上
                if G < 64:      mat = (86, 132, 78)      # 草
                elif G < 128:   mat = (198, 172, 106)    # 沙
                elif G < 192:   mat = (150, 138, 118)    # 岩
                else:           mat = (222, 224, 220)    # 雪
                t = max(0.0, min(1.0, R / 190.0))
                shade = 0.60 + 0.55 * t
                base = tuple(int(min(255, c * shade)) for c in mat)
                if B == 0 and R > 100:
                    base = (96, 92, 86)                 # 边界 / 陡坡
                elif R < 18:
                    base = (86, 140, 176)               # 河道 / 海（B=0 但不是边界）
            sp[x, y] = base
    img = small.resize((512, 512), Image.NEAREST)
    d = ImageDraw.Draw(img)
    d.rectangle([8, 8, 420, 52], fill=(28, 26, 22))
    d.text((18, 12), title, font=dm.F(24), fill=(238, 228, 200))
    img.save(os.path.join(TILES, path))


def check_layout(path):
    img = Image.open(os.path.join(TILES, path)).convert('RGB')
    px = img.load()
    counts = {}
    walkable = set()
    for y in range(W):
        for x in range(W):
            r = px[x, y][0]
            if r > 0:
                counts[r] = counts.get(r, 0) + 1
                walkable.add((x, y))
    portals = [(x, y) for y in range(W) for x in range(W) if px[x, y][0] == 250]
    seen = set()
    if portals:
        q = deque([portals[0]])
        seen.add(q[0])
        while q:
            x, y = q.popleft()
            for n in ((x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1)):
                if n in walkable and n not in seen:
                    seen.add(n)
                    q.append(n)
    area_ids = [k for k in counts if 60 <= k <= 140]
    n_area_px = sum(counts[k] for k in area_ids)
    n_event = counts.get(220, 0)
    n_reward = counts.get(240, 0)
    n_portal = counts.get(250, 0)
    n_road = counts.get(200, 0)
    reach = len(seen & walkable) / max(1, len(walkable)) * 100
    ok = (len(area_ids) >= 2 and n_event > 0 and n_reward > 0 and
          n_portal > 0 and n_road > 0 and reach > 99.0)
    return ok, ('areas=%d(px=%d) events=%d rewards=%d portal=%d road=%d reach=%.1f%%' %
                (len(area_ids), n_area_px, n_event, n_reward, n_portal, n_road, reach))


def main():
    rows = parse_regions()
    log = ['# 64 格输入图自检', '# 格ID | 区 | 路线 | 现代层 | 时代层 | 结论']
    passed = 0
    for row in rows:
        tid, code = row['tid'], row['code']
        era = ERA[code]
        p_modern = gen_points(row, 11)
        p_era = gen_points(row, 97)
        all_areas = p_modern[0] + p_era[0]
        terrain = gen_terrain(row, all_areas)
        terrain.save(os.path.join(TILES, '%s_terrain.png' % tid))

        strengths = [100 + 20 * i for i in range(len(p_modern[0]))]
        l1 = dm.draw_layout('%s_layout.png' % tid, p_modern[0], p_modern[1],
                            p_modern[2], p_modern[3], p_modern[4], strengths)
        strengths2 = [120 + 20 * i for i in range(len(p_era[0]))]
        l2 = dm.draw_layout('%s_layout_%s.png' % (tid, era), p_era[0], p_era[1],
                            p_era[2], p_era[3], p_era[4], strengths2)
        fast_minimap(terrain, l1, '%s_minimap.png' % tid,
                     '%s %s · 2087 调查层' % (tid, row['name']))
        fast_minimap(terrain, l2, '%s_minimap_%s.png' % (tid, era),
                     '%s %s · %s' % (tid, row['name'], row['era_text'].split(' / ')[0]))

        ok1, s1 = check_layout('%s_layout.png' % tid)
        ok2, s2 = check_layout('%s_layout_%s.png' % (tid, era))
        ok = ok1 and ok2
        passed += 1 if ok else 0
        log.append('%s | %s | %s | %s | %s | %s' %
                   (tid, code, row['route'], s1, s2, 'PASS' if ok else 'FAIL'))
    # 保持第 13 节文档列出的样板格 preview 文件名与新图一致
    import shutil
    shutil.copy_file(os.path.join(TILES, 'r2c4_minimap.png'),
                    os.path.join(TILES, 'r2c4_preview_modern.png'))
    shutil.copy_file(os.path.join(TILES, 'r2c4_minimap_sumer.png'),
                    os.path.join(TILES, 'r2c4_preview_sumer.png'))
    log.append('TOTAL=%d PASS=%d FAIL=%d' % (len(rows), passed, len(rows) - passed))
    with open(os.path.join(ROOT, 'tiles_check.txt'), 'w', encoding='utf-8') as f:
        f.write('\n'.join(log) + '\n')
    print(log[-1])
    for line in log:
        if 'FAIL' in line:
            print(line)


if __name__ == '__main__':
    main()
