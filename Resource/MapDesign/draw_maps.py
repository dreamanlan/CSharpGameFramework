# -*- coding: utf-8 -*-
"""
失落的文明 — 阶段 0 设计数据画笔

产出（全部是设计数据，不是美术稿；构建期读取，运行时不读 PNG）：
  world_map.png            4096x4096 藏宝图（8x8 分区 / 藏宝点 / 主线）
  world_map_preview.png    1024 缩略，便于核对
  regions.txt              64 格索引
  tiles/r2c4_terrain.png   256x256 物理图  R 高度 / G 材质 / B 功能 mask
  tiles/r2c4_layout.png    256x256 布局图（2087 现代调查层）
  tiles/r2c4_layout_sumer.png 256x256 布局图（苏美尔层）
  tiles/r2c4_minimap.png   512x512 小地图静态底图
  tiles/r2c4_preview_*.png 512x512 可读预览
  design_check.txt         通道语义与硬约束自检

坐标：每格 512x512 世界单位；输入图 256x256，1 px = 2 单位。
"""
from PIL import Image, ImageDraw, ImageFont
import math
import os
import random

ROOT = r'D:\GitHub\CSharpGameFramework\Resource\MapDesign'
TILES = os.path.join(ROOT, 'tiles')
os.makedirs(TILES, exist_ok=True)

FONT_CANDIDATES = [
    r'C:\Windows\Fonts\msyh.ttc',
    r'C:\Windows\Fonts\simhei.ttf',
    r'C:\Windows\Fonts\simsun.ttc',
]

def F(size):
    for p in FONT_CANDIDATES:
        if os.path.exists(p):
            try:
                return ImageFont.truetype(p, size)
            except Exception:
                pass
    return ImageFont.load_default()

# ---------------------------------------------------------------- 分区与命名
# 每区 2 行 x 4 列 = 8 格
REGIONS = [
    dict(code='ARR', name='亚拉腊·洪水层', r0=0, c0=0, color=(190, 207, 218), era='洪水层 / 现代',
         src='C 大洪水', diff=3),
    dict(code='ORB', name='天外锚点', r0=0, c0=4, color=(178, 173, 208), era='洪水前 / 现代',
         src='A 降临与设施 · G 回归', diff=5),
    dict(code='LEV', name='黎凡特·西奈', r0=2, c0=0, color=(216, 196, 165), era='苏美尔层 / 灾后',
         src='E 设施 · F 神族战争', diff=4),
    dict(code='MES', name='两河·苏美尔', r0=2, c0=4, color=(205, 212, 166), era='苏美尔层 / 现代',
         src='D 文明与权能', diff=2),
    dict(code='EGY', name='埃及·吉萨', r0=4, c0=0, color=(228, 206, 155), era='洪水前 / 苏美尔层',
         src='E 设施 · F 神族战争', diff=4),
    dict(code='ABZ', name='非洲·阿布祖', r0=4, c0=4, color=(202, 181, 150), era='洪水前层',
         src='A 降临 · B 造人', diff=3),
    dict(code='AMR', name='美洲·失落国度', r0=6, c0=0, color=(186, 207, 176), era='洪水后 / 灾后',
         src='E 设施 · F 失落国度', diff=4),
    dict(code='PAC', name='环太平洋·导航标记', r0=6, c0=4, color=(180, 201, 212), era='洪水前 / 现代',
         src='E 设施 · G 回归', diff=3),
]

TILE_NAMES = {
 'r0c0':'冰盖裂隙','r0c1':'方舟残骸','r0c2':'亚拉腊雪线','r0c3':'洪水冲刷谷',
 'r1c0':'幸存者营地','r1c1':'彩虹之约台','r1c2':'泥板沉层','r1c3':'泄密者之径',
 'r0c4':'火星信标','r0c5':'废弃中转站','r0c6':'月球背面','r0c7':'归来之门',
 'r1c4':'第十二天体','r1c5':'轨道解码台','r1c6':'尼比鲁观测台','r1c7':'天外锚点',
 'r2c0':'巴勒贝克巨石','r2c1':'西奈太空港','r2c2':'语言之塔','r2c3':'变乱碑',
 'r3c0':'罪恶之城','r3c1':'死海玻璃原','r3c2':'所多玛残迹','r3c3':'流亡之路',
 'r2c4':'埃利都','r2c5':'尼普尔中枢','r2c6':'乌鲁克城墙','r2c7':'巴别塔',
 'r3c4':'王表库','r3c5':'Me 泥板库','r3c6':'伊南娜神殿','r3c7':'两河渡口',
 'r4c0':'吉萨大金字塔','r4c1':'狮身人面','r4c2':'金字塔战争场','r4c3':'顶端晶石',
 'r5c0':'孟菲斯','r5c1':'拉的神殿','r5c2':'Zep Tepi 标记','r5c3':'尼罗河台地',
 'r4c4':'阿布祖金矿','r4c5':'采金竖井','r4c6':'伊吉吉矿坑','r4c7':'基因工房',
 'r5c4':'伊甸园','r5c5':'拿非利人遗迹','r5c6':'宁胡尔萨格营地','r5c7':'人类初生地',
 'r6c0':'太阳门','r6c1':'萨克塞华曼','r6c2':'冶金炉场','r6c3':'维拉科查台',
 'r7c0':'水晶头骨窟','r7c1':'羽蛇神殿','r7c2':'玛雅观星台','r7c3':'失落的国度',
 'r6c4':'复活节岛摩艾','r6c5':'巨石阵','r6c6':'远洋导航标','r6c7':'星象对齐台',
 'r7c4':'洪前航图','r7c5':'孤岛遗迹','r7c6':'深潜点','r7c7':'归航信标',
}

# 主线：按「真相因果链」串起的探索路线，不是地理顺序
MAIN_ROUTE = [
 'r2c4','r3c5','r3c4','r2c7',          # 文明权能 -> 时间轴 -> 语言分散
 'r0c1','r4c4','r5c4',                 # 洪水实证 -> 采金 -> 造人
 'r4c0','r2c1','r0c6',                 # 导航信标 -> 太空港 -> 月球背面
 'r7c0','r6c6','r1c6','r0c7',          # 真相文本 -> 导航网 -> 3417 -> 归来
]

def region_of(r, c):
    for g in REGIONS:
        if g['r0'] <= r < g['r0'] + 2 and g['c0'] <= c < g['c0'] + 4:
            return g
    return REGIONS[0]

def tile_id(r, c):
    return 'r%dc%d' % (r, c)

# ---------------------------------------------------------------- 4096 藏宝图
def draw_world_map():
    S = 4096
    CELL = S // 8
    img = Image.new('RGB', (S, S), (233, 219, 186))
    d = ImageDraw.Draw(img)

    # 纸纹（低频噪声，纯装饰，不携带语义）
    rnd = random.Random(2087)
    for _ in range(2600):
        x = rnd.randrange(S); y = rnd.randrange(S)
        v = rnd.randint(-9, 6)
        d.ellipse([x, y, x + rnd.randint(2, 9), y + rnd.randint(2, 9)],
                  fill=(max(0, 233 + v), max(0, 219 + v), max(0, 186 + v)))

    # 分区底色 + 大区标题
    for g in REGIONS:
        x0, y0 = g['c0'] * CELL, g['r0'] * CELL
        d.rectangle([x0, y0, x0 + CELL * 4 - 1, y0 + CELL * 2 - 1], fill=g['color'])
        d.rectangle([x0 + 8, y0 + 8, x0 + 4 * CELL - 9, y0 + 2 * CELL - 9],
                    outline=(120, 100, 70), width=4)
        d.text((x0 + 28, y0 + 22), '%s  %s' % (g['code'], g['name']),
               font=F(46), fill=(58, 44, 28))

    # 每格：地貌暗示 / 网格 / 名称 / 藏宝点
    for r in range(8):
        for c in range(8):
            x0, y0 = c * CELL, r * CELL
            g = region_of(r, c)
            tid = tile_id(r, c)
            dark = tuple(max(0, v - 34) for v in g['color'])
            # 地貌曲线
            for k in range(4):
                pts = []
                for t in range(0, 25):
                    xx = x0 + 20 + t * (CELL - 40) / 24
                    yy = y0 + 150 + k * 78 + 26 * math.sin(t * 0.55 + r * 1.3 + c * 0.8 + k)
                    pts.append((xx, yy))
                d.line(pts, fill=dark, width=3)
            d.rectangle([x0, y0, x0 + CELL - 1, y0 + CELL - 1], outline=(120, 100, 70), width=3)
            d.text((x0 + 18, y0 + 16), tid.upper(), font=F(26), fill=(96, 76, 50))
            name = TILE_NAMES[tid]
            bb = d.textbbox((0, 0), name, font=F(40))
            tw = bb[2] - bb[0]
            d.rectangle([x0 + 60, y0 + 300, x0 + CELL - 60, y0 + 380],
                        fill=(238, 228, 203), outline=(120, 100, 70), width=2)
            d.text((x0 + (CELL - tw) / 2, y0 + 316), name, font=F(40), fill=(54, 40, 26))

            # 藏宝点：主线金色菱形 + 序号；支线小圆
            if tid in MAIN_ROUTE:
                idx = MAIN_ROUTE.index(tid) + 1
                cx, cy = x0 + CELL // 2, y0 + CELL // 2 + 20
                d.polygon([(cx, cy - 34), (cx + 34, cy), (cx, cy + 34), (cx - 34, cy)],
                          fill=(206, 156, 54), outline=(92, 62, 24), width=4)
                d.text((cx - 14, cy - 24), str(idx), font=F(34), fill=(60, 36, 16))
            else:
                for n, (ox, oy) in enumerate([(150, 170), (370, 200)]):
                    d.ellipse([x0 + ox - 13, y0 + oy - 13, x0 + ox + 13, y0 + oy + 13],
                              fill=(170, 120, 60), outline=(92, 62, 24), width=3)

    # 主线虚线
    def center(tid):
        r = int(tid[1]); c = int(tid[3])
        return (c * CELL + CELL // 2, r * CELL + CELL // 2 + 20)

    for a, b in zip(MAIN_ROUTE, MAIN_ROUTE[1:]):
        x0, y0 = center(a); x1, y1 = center(b)
        dist = math.hypot(x1 - x0, y1 - y0)
        steps = int(dist / 26)
        for i in range(0, steps, 2):
            t0 = i / max(1, steps); t1 = min(1.0, (i + 1) / max(1, steps))
            d.line([(x0 + (x1 - x0) * t0, y0 + (y1 - y0) * t0),
                    (x0 + (x1 - x0) * t1, y0 + (y1 - y0) * t1)],
                   fill=(150, 62, 40), width=6)

    # 图例 / 罗盘：压在边角但不遮挡格名（格名位于每格 y+300..380）
    d.rectangle([70, S - 300, 830, S - 70], fill=(240, 230, 205), outline=(120, 100, 70), width=5)
    d.text((100, S - 285), '遗迹藏宝图 / 2087', font=F(44), fill=(54, 40, 26))
    d.text((100, S - 225), '◆ 主线藏宝点   ● 支线藏宝点   ┄ 调查路线',
           font=F(28), fill=(70, 54, 34))
    d.text((100, S - 180), '8x8 = 64 场景  |  1 格 512  |  信息层，非世界场景替代',
           font=F(26), fill=(86, 68, 44))
    d.text((100, S - 135), '尼比鲁近地点 3417  ·  遗迹复苏  ·  护人类',
           font=F(26), fill=(120, 56, 40))

    cx, cy = S - 160, 130
    d.ellipse([cx - 85, cy - 85, cx + 85, cy + 85], outline=(100, 80, 52), width=5)
    d.polygon([(cx, cy - 78), (cx - 18, cy + 20), (cx, cy + 5), (cx + 18, cy + 20)],
              fill=(150, 62, 40))
    d.text((cx - 14, cy - 125), 'N', font=F(40), fill=(54, 40, 26))

    img.save(os.path.join(ROOT, 'world_map.png'))
    # 缩略图下方加信息带，避免任何遮挡
    small = img.resize((1024, 1024), Image.LANCZOS)
    canvas = Image.new('RGB', (1024, 1120), (28, 26, 22))
    canvas.paste(small, (0, 0))
    cd = ImageDraw.Draw(canvas)
    cd.text((20, 1032), '遗迹藏宝图 / 2087   ·   8x8 = 64 场景   ·   ◆ 主线藏宝点   ● 支线   ┄ 调查路线',
            font=F(26), fill=(238, 228, 200))
    cd.text((20, 1072), '14 站主线：文明权能 → 时间轴 → 洪水 → 采金 → 造人 → 导航 → 天外 → 3417 → 归来',
            font=F(23), fill=(200, 180, 140))
    canvas.save(os.path.join(ROOT, 'world_map_preview.png'))
    return 'world_map.png 4096x4096 / world_map_preview.png 1024x1120'

# ---------------------------------------------------------------- 64 格索引
def write_regions():
    lines = [
        '# 失落的文明 — 8x8 藏宝图分区索引（阶段 0）',
        '# 格ID | 区码 | 名称 | 时代层 | 路线 | 藏宝点数 | 素材引用 | 难度 | 主线序号',
        '# 时代层：每格至少 2 套 .unity（现代调查层 + 该格主时代层）',
    ]
    rnd = random.Random(3417)
    for r in range(8):
        for c in range(8):
            tid = tile_id(r, c)
            g = region_of(r, c)
            main = tid in MAIN_ROUTE
            pts = 3 if main else rnd.choice([1, 1, 2])
            seq = MAIN_ROUTE.index(tid) + 1 if main else 0
            lines.append('%s | %s | %s | %s | %s | %d | %s | %d | %d' %
                         (tid, g['code'], TILE_NAMES[tid], g['era'],
                          '主线' if main else '支线', pts, g['src'], g['diff'], seq))
    p = os.path.join(ROOT, 'regions.txt')
    with open(p, 'w', encoding='utf-8') as f:
        f.write('\n'.join(lines) + '\n')
    return 'regions.txt rows=64'

# ---------------------------------------------------------------- 样板格工具
W = 256

def new_img():
    return Image.new('RGB', (W, W), (0, 0, 0))

def disk(px, cx, cy, rad, rgb):
    for y in range(max(0, int(cy - rad)), min(W, int(cy + rad + 1))):
        for x in range(max(0, int(cx - rad)), min(W, int(cx + rad + 1))):
            if (x - cx) ** 2 + (y - cy) ** 2 <= rad * rad:
                px[x, y] = rgb

def segment(px, x0, y0, x1, y1, width, rgb):
    steps = int(math.hypot(x1 - x0, y1 - y0) * 2) + 1
    for i in range(steps + 1):
        t = i / max(1, steps)
        disk(px, x0 + (x1 - x0) * t, y0 + (y1 - y0) * t, width, rgb)

def draw_terrain():
    """R 高度 / G 材质 / B 功能 mask。埃利都：冲积平原 + 两河 + 城市台地 + 边界山脊。"""
    img = new_img(); px = img.load()
    for y in range(W):
        for x in range(W):
            # 基底低平冲积平原
            h = 22 + 6 * math.sin(x * 0.035) * math.cos(y * 0.028)
            # 两条河道（低地）
            river1 = abs(y - (70 + 26 * math.sin(x * 0.028)))
            river2 = abs(y - (190 + 20 * math.sin(x * 0.022 + 1.7)))
            if river1 < 9 or river2 < 8:
                h = 6 + river1 * 0.5
            # 城市台地（右上）
            if 120 < x < 210 and 60 < y < 150:
                h = max(h, 62 + 8 * math.sin((x + y) * 0.05))
            # 远处沙丘 / 山脊
            d = math.hypot(x - 128, y - 128)
            if d > 96:
                h += min(90, (d - 96) * 1.1) * (0.6 + 0.4 * math.sin(x * 0.06 + y * 0.04))
            # 边界封边
            edge = min(x, y, W - 1 - x, W - 1 - y)
            if edge < 10:
                h = max(h, 190 + (10 - edge) * 5)
            R = int(max(0, min(255, h)))
            # G 材质：低地草/沙，河道沙，台地岩，高地雪
            if river1 < 9 or river2 < 8:
                G = 96
            elif R < 35:
                G = 34
            elif R < 80:
                G = 150
            elif R < 150:
                G = 170
            else:
                G = 226
            # B 功能 mask：战斗平台/道路 255，装饰带 128，边界/陡坡 0
            B = 255 if R < 80 else 128
            if edge < 10 or R > 170:
                B = 0
            px[x, y] = (R, G, B)
    # 两个战斗区：内部高度恒定（可走平台）
    for cx, cy in [(76, 108), (168, 120)]:
        disk(px, cx, cy, 24, (32, 34, 255))
    img.save(os.path.join(TILES, 'r2c4_terrain.png'))
    return img

def draw_layout(path, areas, events, rewards, portal, roads, strengths):
    """R 区域ID / G 强度 / B 触发类型。"""
    img = new_img(); px = img.load()
    for y in range(W):
        for x in range(W):
            px[x, y] = (0, 0, 0)
    # 道路（先画，区域后画会覆盖中心，但不影响连通）
    for (a, b) in roads:
        segment(px, a[0], a[1], b[0], b[1], 3, (200, 40, 0))
    # 战斗区
    for i, (cx, cy, rad) in enumerate(areas):
        disk(px, cx, cy, rad, (60 + 20 * i, strengths[i], 255))
    # 事件点 / 收益点 / 传送点
    for i, (cx, cy) in enumerate(events):
        disk(px, cx, cy, 6, (220, 120 + 20 * i, 160))
    for i, (cx, cy) in enumerate(rewards):
        disk(px, cx, cy, 5, (240, 60 + 45 * i, 80))
    disk(px, portal[0], portal[1], 7, (250, 0, 0))
    # 边界一圈标记禁止
    for y in range(W):
        for x in range(W):
            if min(x, y, W - 1 - x, W - 1 - y) < 4:
                px[x, y] = (0, 0, 0)
    img.save(os.path.join(TILES, path))
    return img

def draw_minimap(terrain, layout, path, title):
    """512x512 静态俯视底图：地形色 + 布局语义叠加（构建期产出，运行时叠动态标记）。"""
    t = terrain.resize((512, 512), Image.NEAREST).convert('RGB')
    l = layout.resize((512, 512), Image.NEAREST).convert('RGB')
    tp, lp = t.load(), l.load()
    for y in range(512):
        for x in range(512):
            R, G, B = tp[x, y]
            lr, lg, lb = lp[x, y]
            if lr == 200:      base = (196, 168, 108)   # 道路
            elif lr in (60, 80): base = (190, 84, 68)   # 战斗区
            elif lr == 220:    base = (232, 196, 66)    # 事件点
            elif lr == 240:    base = (70, 186, 200)    # 收益点
            elif lr == 250:    base = (222, 92, 176)    # 传送点
            elif B == 0:       base = (96, 92, 86)      # 边界
            elif R > 150:      base = (214, 210, 198)   # 高地
            elif R < 20:       base = (96, 148, 178)    # 河道
            elif B == 128:     base = (156, 156, 112)   # 装饰带
            else:              base = (126, 152, 96)    # 平原
            tp[x, y] = base
    d = ImageDraw.Draw(t)
    d.rectangle([8, 8, 300, 52], fill=(28, 26, 22))
    d.text((18, 12), title, font=F(24), fill=(238, 228, 200))
    d.rectangle([8, 452, 504, 504], fill=(28, 26, 22))
    d.text((16, 462), '道路 / 战斗区 / 事件 / 收益 / 传送点 / 边界',
           font=F(20), fill=(238, 228, 200))
    t.save(os.path.join(TILES, path))

# ---------------------------------------------------------------- 自检
def check_layout(path, label):
    img = Image.open(os.path.join(TILES, path)).convert('RGB')
    px = img.load()
    counts = {}
    walkable = set()
    for y in range(W):
        for x in range(W):
            r = px[x, y][0]
            if r in (60, 80, 200, 220, 240, 250):
                counts[r] = counts.get(r, 0) + 1
                walkable.add((x, y))
    # BFS 从传送点出发
    portals = [(x, y) for y in range(W) for x in range(W) if px[x, y][0] == 250]
    seen = set()
    if portals:
        q = [portals[0]]; seen.add(q[0])
        while q:
            x, y = q.pop()
            for nx, ny in ((x+1,y),(x-1,y),(x,y+1),(x,y-1)):
                if (nx, ny) in walkable and (nx, ny) not in seen:
                    seen.add((nx, ny)); q.append((nx, ny))
    def reached(rid):
        pts = [(x, y) for y in range(W) for x in range(W) if px[x, y][0] == rid]
        if not pts:
            return 'MISSING'
        return '%d/%d' % (sum(1 for p in pts if p in seen), len(pts))
    return ('%s  areas=%s events=%s rewards=%s portal=%s road=%s '
            'reachable: area1=%s area2=%s event=%s reward=%s' %
            (label, counts.get(60, 0) + counts.get(80, 0), counts.get(220, 0),
             counts.get(240, 0), counts.get(250, 0), counts.get(200, 0),
             reached(60), reached(80), reached(220), reached(240)))

def check_terrain():
    img = Image.open(os.path.join(TILES, 'r2c4_terrain.png')).convert('RGB')
    px = img.load()
    rmin, rmax, b0, b128, b255 = 255, 0, 0, 0, 0
    for y in range(W):
        for x in range(W):
            R, G, B = px[x, y]
            rmin = min(rmin, R); rmax = max(rmax, R)
            if B == 0: b0 += 1
            elif B == 128: b128 += 1
            elif B == 255: b255 += 1
    vals = [px[x, y][0] for y in range(84, 133) for x in range(52, 101)
            if (x - 76) ** 2 + (y - 108) ** 2 <= 24 ** 2]
    return ('terrain R=%d..%d  B0=%d B128=%d B255=%d  arena_R_minmax=%d..%d' %
            (rmin, rmax, b0, b128, b255, min(vals), max(vals)))

# ---------------------------------------------------------------- main
def main():
    log = []
    log.append(draw_world_map())
    log.append(write_regions())

    terrain = draw_terrain()
    portal = (30, 35)                       # 世界 (60,70) -> 像素 (30,35)
    # 现代调查层（2087）：考古营地 -> 探方 -> 泥板临时库
    modern_areas = [(76, 108, 24), (168, 120, 22)]
    modern_events = [(124, 92), (196, 168)]
    modern_rewards = [(52, 160), (150, 190), (214, 76)]
    modern_roads = [
        (portal, (76, 108)), ((76, 108), (124, 92)), ((124, 92), (168, 120)),
        ((168, 120), (196, 168)), ((76, 108), (52, 160)),
        ((124, 92), (150, 190)), ((168, 120), (214, 76)),
    ]
    l1 = draw_layout('r2c4_layout.png', modern_areas, modern_events,
                     modern_rewards, portal, modern_roads, [100, 160])
    draw_minimap(terrain, l1, 'r2c4_minimap.png', 'r2c4 埃利都 · 2087 调查层')
    shutil_copy_minimap('r2c4_minimap.png', 'r2c4_preview_modern.png')

    # 苏美尔层：塔庙 / 泥板库 / 运河码头，传送点位置不变（时空锚点固定）
    sumer_areas = [(96, 132, 24), (186, 96, 22)]
    sumer_events = [(140, 104), (206, 150)]
    sumer_rewards = [(60, 76), (128, 196), (220, 122)]
    sumer_roads = [
        (portal, (96, 132)), ((96, 132), (140, 104)), ((140, 104), (186, 96)),
        ((186, 96), (206, 150)), ((96, 132), (60, 76)),
        ((140, 104), (128, 196)), ((186, 96), (220, 122)),
    ]
    l2 = draw_layout('r2c4_layout_sumer.png', sumer_areas, sumer_events,
                     sumer_rewards, portal, sumer_roads, [130, 200])
    draw_minimap(terrain, l2, 'r2c4_minimap_sumer.png', 'r2c4 埃利都 · 苏美尔层')
    shutil_copy_minimap('r2c4_minimap_sumer.png', 'r2c4_preview_sumer.png')

    log.append(check_terrain())
    log.append(check_layout('r2c4_layout.png', 'layout_modern'))
    log.append(check_layout('r2c4_layout_sumer.png', 'layout_sumer'))
    log.append('main_route_stops=%d  regions=%d  tiles=%d' %
               (len(MAIN_ROUTE), len(REGIONS), len(TILE_NAMES)))
    with open(os.path.join(ROOT, 'design_check.txt'), 'w', encoding='utf-8') as f:
        f.write('\n'.join(log) + '\n')
    print('\n'.join(log))

def shutil_copy_minimap(src, dst):
    import shutil
    shutil.copy_file(os.path.join(TILES, src), os.path.join(TILES, dst))

if __name__ == '__main__':
    main()
