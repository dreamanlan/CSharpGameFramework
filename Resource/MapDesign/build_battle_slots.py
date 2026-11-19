# -*- coding: utf-8 -*-
"""
Battlefield slot planner (design doc 35).

Produces the single source of truth for every downstream generator:
  Resource/MapDesign/axis_events.txt   148 authoritative main-axis events (UTF-8 snapshot)
  Resource/MapDesign/battle_slots.txt  640 rows = 64 tiles x 10 slots

Layout rule: one 32x32 world-space neighbourhood per battlefield, magnified
x16 into an independent 512x512 scene. Slots are aligned to the 32-unit grid
so two battlefields never share a neighbourhood. Main-axis events keep their
original coordinates and claim their own slot; splinter clusters that fall in
the same slot are merged into one event (largest component wins). Remaining
slots are filled with deterministic farthest-point sampling so branch events
are spread as evenly as possible.

level_id = 10001 + tile_index * 10 + slot_index   (tile_index = r*8+c)
grid_key = {tid}_s{slot:02d}                      (battlefield-level event key)

This script is idempotent: it always reads the immutable axis snapshot, never
the generated WorldEvent.txt (which this pipeline expands to 640 rows).
"""
import os
import math
import random

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
SERVER_TABLE = os.path.join(ROOT, 'Resource', 'Tables', 'Server', 'WorldEvent.txt')
AXIS = os.path.join(DESIGN, 'axis_events.txt')
SLOTS = os.path.join(DESIGN, 'battle_slots.txt')
REGIONS = os.path.join(DESIGN, 'regions.txt')

L = 32.0                 # neighbourhood side in world units
CELLS = int(512 / L)     # 16 x 16 slots per tile
PER_TILE = 10
MAGNIFY = 16             # 32 x 16 = 512


def tiles():
    out = []
    with open(REGIONS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append(dict(tid=p[0], code=p[1], name=p[2], era_text=p[3],
                            route=p[4], src=p[6], diff=int(p[7]), seq=int(p[8])))
    return out


def read_axis():
    """Load the immutable 148-row snapshot, creating it once from the GBK table."""
    if os.path.exists(AXIS):
        rows = []
        with open(AXIS, encoding='utf-8') as f:
            for line in f:
                if line.startswith('#') or not line.strip():
                    continue
                rows.append(line.rstrip('\n').split('\t'))
        return rows
    # First run: snapshot the current authoritative server table (GBK).
    with open(SERVER_TABLE, encoding='gbk') as f:
        lines = f.read().splitlines()
    data = [l.split('\t') for l in lines[2:] if l.strip()]
    with open(AXIS, 'w', encoding='utf-8', newline='') as f:
        f.write('# axis event snapshot | id grid_id era x z radius reward token title\n')
        for r in data:
            f.write('\t'.join(r) + '\n')
    return data


def slot_of(x, z):
    i = min(CELLS - 1, max(0, int(float(x) // L)))
    j = min(CELLS - 1, max(0, int(float(z) // L)))
    return (i, j)


def farthest_pick(seed_slots, k, seed):
    """Deterministically grow a spread-out slot set from the axis slots."""
    rnd = random.Random(seed)
    chosen = list(seed_slots)
    cand = [(i, j) for i in range(CELLS) for j in range(CELLS)]
    rnd.shuffle(cand)
    while len(chosen) < k:
        best, bestd = None, -1.0
        for c in cand:
            if c in chosen:
                continue
            d = min((c[0] - q[0]) ** 2 + (c[1] - q[1]) ** 2 for q in chosen)
            if d > bestd:
                bestd, best = d, c
        if best is None:
            raise RuntimeError('not enough slots')
        chosen.append(best)
        cand.remove(best)
    return chosen


def main():
    rows = tiles()
    axis = read_axis()
    by_tile = {}
    for r in axis:
        by_tile.setdefault(r[1], []).append(r)

    out = [
        '# battlefield slots | 64 tiles x 10 = 640',
        '# level_id | tile_id | slot | grid_key | era | kind | x | z | radius | reward | token | title | code | name | src | diff | route',
    ]
    stats = {'axis': 0, 'branch': 0, 'merged': 0}
    worst_sep = 999.0

    for ti, t in enumerate(rows):
        tid = t['tid']
        era = {'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian',
               'MES': 'sumer', 'EGY': 'prediluvian', 'ABZ': 'prediluvian',
               'AMR': 'postdiluvian', 'PAC': 'ancient'}[t['code']]

        # Claim one slot per distinct main-axis neighbourhood; merge splinters.
        claimed = {}
        for r in by_tile.get(tid, []):
            k = slot_of(r[3], r[4])
            if k in claimed:
                stats['merged'] += 1
                if float(r[5]) <= float(claimed[k][5]):
                    continue
            claimed[k] = r
        axis_slots = sorted(claimed.keys())
        picked = farthest_pick(axis_slots, PER_TILE, 1000 + ti)
        chosen = sorted(picked, key=lambda q: (q[1], q[0]))
        d = min(math.hypot(a[0] - b[0], a[1] - b[1])
                for a in chosen for b in chosen if a != b)
        worst_sep = min(worst_sep, d)

        for si, (i, j) in enumerate(chosen):
            level_id = 10001 + ti * PER_TILE + si
            grid_key = '%s_s%02d' % (tid, si)
            if (i, j) in claimed:
                r = claimed[(i, j)]
                kind = 'axis'
                x, z, radius = r[3], r[4], r[5]
                reward, token, title = r[6], r[7], r[8]
                stats['axis'] += 1
            else:
                kind = 'branch'
                x, z = '%0.1f' % (i * L + L / 2), '%0.1f' % (j * L + L / 2)
                radius = '8.9'
                reward = str(40 + t['diff'] * 8)
                token = 'branch_%s_s%02d' % (tid, si)
                title = '%s：%s勘察点 %02d' % (t['name'], era, si + 1)
                stats['branch'] += 1
            out.append('\t'.join([
                str(level_id), tid, str(si), grid_key, era, kind,
                x, z, radius, reward, token, title,
                t['code'], t['name'], t['src'], str(t['diff']), t['route'],
            ]))

    with open(SLOTS, 'w', encoding='utf-8', newline='') as f:
        f.write('\n'.join(out) + '\n')

    print('SLOTS rows=%d tiles=%d per_tile=%d' % (len(out) - 2, len(rows), PER_TILE))
    print('axis=%d branch=%d merged_splinters=%d' %
          (stats['axis'], stats['branch'], stats['merged']))
    print('min_slot_separation=%.1f (>=1 means disjoint 32x32 neighbourhoods)' % worst_sep)
    print('level_id range=10001..%d' % (10001 + len(rows) * PER_TILE - 1))
    print('magnification=x%d -> %dx%d battlefield' % (MAGNIFY, int(L * MAGNIFY), int(L * MAGNIFY)))


if __name__ == '__main__':
    main()
