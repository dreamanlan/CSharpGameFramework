# -*- coding: utf-8 -*-
"""
阶段 2：把布局图里的事件点烘焙成「服务端事件表 + 客户端模糊提示」。

安全边界（设计文档 12.2 / 12.6 / 12.7）：
  事件 ID、精确坐标、半径、收益、状态 —— 只存在于服务端。
  客户端包体里没有任何事件表、event_id 或精确坐标，只有 coarse 视觉提示。

输入：tiles/{tid}_layout_{era}.png 的 R=220 连通分量（设计图里的事件点）
输出：
  Resource/Tables/Server/WorldEvent.txt      服务端专用表（Tables/Server 只同步到服务端）
  Assets/Resources/MapData/{tid}_areas.txt   净化版：移除所有 220 行
  Assets/Resources/MapData/{tid}_hints.txt   客户端模糊视觉提示
  events_check.txt                           隔离自检报告

坐标：布局图 256x256，1px = 2 世界单位。Unity GetPixels32 自下而上，
      C# ReadPng 反转回自上而下的 design 缓冲，故 design_y == pil_y，
      worldX = pil_x * 2，worldZ = pil_y * 2（与 TerrainData / areas.txt 一致）。
"""
import os
import random
from collections import deque
from PIL import Image

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
CLIENT = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'MapData')
SERVER_TABLE = os.path.join(ROOT, 'Resource', 'Tables', 'Server', 'WorldEvent.txt')

ERA = {
    'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
    'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
}
SCALE = 2.0          # 1 px = 2 world units
HINT_GRID = 64.0     # coarse visual hint quantization
HINT_RADIUS = 48.0


def tiles():
    out = []
    with open(os.path.join(DESIGN, 'regions.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append(dict(tid=p[0], code=p[1], name=p[2], era_text=p[3],
                            route=p[4], src=p[6], diff=int(p[7]), seq=int(p[8])))
    return out


def event_centers(tid, era):
    """Return exact world-space centres of R=220 components in the era layout."""
    p = os.path.join(TILES, '%s_layout_%s.png' % (tid, era))
    if not os.path.exists(p):
        return []
    img = Image.open(p).convert('RGB')
    w, h = img.size
    px = img.load()
    seen = set()
    out = []
    for y in range(h):
        for x in range(w):
            if px[x, y][0] != 220 or (x, y) in seen:
                continue
            q = deque([(x, y)])
            seen.add((x, y))
            sx = sz = n = 0
            while q:
                cx, cy = q.popleft()
                sx += cx
                sz += cy
                n += 1
                for nx, ny in ((cx + 1, cy), (cx - 1, cy), (cx, cy + 1), (cx, cy - 1)):
                    if 0 <= nx < w and 0 <= ny < h and (nx, ny) not in seen and px[nx, ny][0] == 220:
                        seen.add((nx, ny))
                        q.append((nx, ny))
            # Unity Texture2D.GetPixels32() is bottom-up; C# ReadPng reverses it
            # back into a top-down design buffer, so design_y == PIL y. The
            # terrain and areas.txt therefore use worldZ = pil_y * SCALE.
            # (An earlier version flipped Y a second time and mirrored all event
            # positions relative to the terrain.)
            out.append((sx / n * SCALE, sz / n * SCALE, n))
    return out


def purify_areas(tid):
    """Remove exact event coordinates from the client-side baked data."""
    p = os.path.join(CLIENT, tid + '_areas.txt')
    if not os.path.exists(p):
        return 0
    with open(p, encoding='utf-8-sig') as f:
        lines = f.read().splitlines()
    kept = [l for l in lines if not (l.strip().startswith('220 ') or l.strip().startswith('220|'))]
    removed = len(lines) - len(kept)
    # Also scrub the header comment so it cannot hint at the event id mapping.
    kept = [l for l in kept if '220' not in l or l.lstrip().startswith('# type: 60')]
    with open(p, 'w', encoding='utf-8') as f:
        f.write('\n'.join(kept) + '\n')
    return removed


def write_hints(tid, centers, rnd):
    p = os.path.join(CLIENT, tid + '_hints.txt')
    lines = [
        '# Coarse visual hints only. No event ids, exact positions, radii or rewards.',
        '# hint | coarseX | coarseZ | radius | textKey',
    ]
    for i, (x, z, _n) in enumerate(centers):
        cx = round(x / HINT_GRID) * HINT_GRID + rnd.uniform(-10, 10)
        cz = round(z / HINT_GRID) * HINT_GRID + rnd.uniform(-10, 10)
        cx = max(20, min(492, cx))
        cz = max(20, min(492, cz))
        lines.append('hint | %.1f | %.1f | %.1f | clue_%s_%d' % (cx, cz, HINT_RADIUS, tid, i))
    with open(p, 'w', encoding='utf-8') as f:
        f.write('\n'.join(lines) + '\n')


def main():
    rows = tiles()
    rnd = random.Random(3417)
    out = [
        '# SERVER ONLY - never copied to the client build.',
        '# id | grid_id | era | x | z | radius | reward | token | title',
        '# exact positions/radii/rewards are server-side only; client gets coarse hints.',
    ]
    total = 0
    for rank, t in enumerate(rows, 1):
        era = ERA[t['code']]
        centers = event_centers(t['tid'], era)
        write_hints(t['tid'], centers, rnd)
        purify_areas(t['tid'])
        for i, (x, z, n) in enumerate(centers):
            reward = (100 + t['diff'] * 20) if t['route'] == '主线' else (40 + t['diff'] * 12)
            radius = 8.0 + min(4.0, n / 120.0)
            out.append('\t'.join([
                'ev_%s_%d' % (t['tid'], i), t['tid'], era,
                '%.1f' % x, '%.1f' % z, '%.1f' % radius, str(reward),
                'frag_%s_%d' % (t['tid'], i),
                '%s：%s' % (t['name'], t['src']),
            ]))
            total += 1
    os.makedirs(os.path.dirname(SERVER_TABLE), exist_ok=True)
    with open(SERVER_TABLE, 'w', encoding='utf-8') as f:
        f.write('\n'.join(out) + '\n')

    # Isolation self-check
    client_tables = os.path.join(ROOT, 'Unity3d', 'Assets', 'StreamingAssets', 'Tables')
    client_server_dir = os.path.join(client_tables, 'Server')
    leaks = []
    if os.path.isdir(client_server_dir):
        leaks.append('client has Tables/Server dir')
    for root, _dirs, files in os.walk(os.path.join(ROOT, 'Unity3d', 'Assets')):
        if '\\Library' in root or '\\Temp' in root:
            continue
        for fn in files:
            if fn == 'WorldEvent.txt' or fn == 'WorldEvent.dat':
                leaks.append(os.path.join(root, fn))
    leaked_rows = 0
    for t in rows:
        p = os.path.join(CLIENT, t['tid'] + '_areas.txt')
        if os.path.exists(p):
            with open(p, encoding='utf-8-sig') as f:
                leaked_rows += sum(1 for l in f if l.strip().startswith('220'))
    report = [
        'EVENTS_CHECK',
        'tiles=%d events=%d' % (len(rows), total),
        'server_table=%s' % SERVER_TABLE,
        'client_Tables_Server_exists=%s' % os.path.isdir(client_server_dir),
        'client_area_event_rows=%d' % leaked_rows,
        'leaks=%d' % len(leaks),
    ] + leaks
    with open(os.path.join(DESIGN, 'events_check.txt'), 'w', encoding='utf-8') as f:
        f.write('\n'.join(report) + '\n')
    print('\n'.join(report))


if __name__ == '__main__':
    main()
