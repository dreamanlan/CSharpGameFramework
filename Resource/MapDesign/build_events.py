# -*- coding: utf-8 -*-
"""
阶段 2：把布局图里的事件点烘焙成「服务端事件表 + 客户端模糊提示」。

安全边界（设计文档 12.2 / 12.6 / 12.7）：
  事件 ID、精确坐标、半径、收益、状态 —— 只存在于服务端。
  客户端包体里没有任何事件表、event_id 或精确坐标，只有 coarse 视觉提示。

输入：tiles/{tid}_layout_{era}.png 的 R=220 连通分量（设计图里的事件点）
输出：
  Resource/Tables/Server/WorldEvent.txt      标准项目表（行1=类型、行2=字段名、GBK）
                                             → TableReaderGenerator 生成 .dat 与
                                               TableConfig.WorldEventProvider
  Assets/Resources/MapData/{tid}_areas.txt   净化版：移除所有 220 行
  Assets/Resources/MapData/{tid}_hints.txt   客户端模糊视觉提示
  events_check.txt                           隔离自检报告

事件表走项目读表器，不再由脚本直接写运行时副本；`Server/` 目录在客户端同步时
被 TableReaderGenerator 跳过，因此隔离由读表器本身保证。

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
# A normal project table: TableReaderGenerator converts it to WorldEvent.dat
# and generates TableConfig.WorldEventProvider from table.dsl. The Server/
# folder is skipped by the client sync, so isolation is enforced by the reader.
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


def battle_slots():
    """640 rows from the single source of truth produced by build_battle_slots.py."""
    out = []
    with open(os.path.join(DESIGN, 'battle_slots.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            out.append(dict(level_id=int(p[0]), tid=p[1], slot=int(p[2]),
                            grid_key=p[3], era=p[4], kind=p[5],
                            x=float(p[6]), z=float(p[7]), radius=float(p[8]),
                            reward=int(p[9]), token=p[10], title=p[11]))
    return out


def main():
    rows = tiles()
    slots = battle_slots()
    rnd = random.Random(3417)
    # Keep the 9-column schema so TableReaderGenerator does not regenerate
    # DataReader.cs field definitions. Battlefield identity is encoded in
    # grid_id ("r0c0_s00"); ExploreSystem builds s_ByGrid from it, which makes
    # the ten battlefields of one tile hold disjoint event sets. level_id is
    # recoverable as 10001 + tile_index*10 + slot (see build_battle_slots.py).
    out = [
        '\t'.join(['string', 'string', 'string', 'float', 'float',
                   'float', 'int32', 'string', 'string']),
        '\t'.join(['id', 'grid_id', 'era', 'x', 'z',
                   'radius', 'reward', 'token', 'title']),
    ]
    # Coarse client hints are still per-tile (no exact event positions leak).
    by_tile = {}
    for s in slots:
        by_tile.setdefault(s['tid'], []).append((s['x'], s['z'], 1))
    for t in rows:
        write_hints(t['tid'], by_tile.get(t['tid'], []), rnd)
        purify_areas(t['tid'])
    for n, s in enumerate(slots):
        out.append('\t'.join([
            'ev_%s_%d' % (s['grid_key'], n), s['grid_key'], s['era'],
            '%.1f' % s['x'], '%.1f' % s['z'], '%.1f' % s['radius'],
            str(s['reward']), s['token'], s['title'],
        ]))
    total = len(slots)
    text = '\n'.join(out) + '\n'
    os.makedirs(os.path.dirname(SERVER_TABLE), exist_ok=True)
    # TableReaderGenerator / Txt2Binary read project tables as GBK (codepage 936).
    with open(SERVER_TABLE, 'w', encoding='gbk', newline='') as f:
        f.write(text)

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
        'tiles=%d battlefields=%d events=%d' % (len(rows), 640, total),
        'axis=%d branch=%d' % (sum(1 for s in slots if s['kind'] == 'axis'),
                               sum(1 for s in slots if s['kind'] == 'branch')),
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
