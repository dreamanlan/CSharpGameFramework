# -*- coding: utf-8 -*-
"""
Stage-1 cutover for the 640-battlefield layout (design doc 35).

Rebuilds the battle rows of Level.txt from battle_slots.txt:
  - 4 legacy rows (1 / 2 / 101 / 102) are preserved verbatim
  - 640 battle rows, id 10001..10640, prefab Battle_{tid}_{era}_s{slot:02d}
  - 64 world-tile rows, id 20001..20064, are preserved (appended by
    cutover_world.py, regenerated here if missing)
  - Story1/Story2 rotation bounds are raised from 10064 to 10640

The 640 story skeletons are written by build_story.py (stage 3), not here.
TableReaderGenerator / DslCopy sync is an external command (dslcopy.bat).
"""
import os
import shutil

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
DSL = os.path.join(ROOT, 'Resource', 'DslFile', 'Story')
LEVEL = os.path.join(ROOT, 'Resource', 'Tables', 'Public', 'Level.txt')
SLOTS = os.path.join(DESIGN, 'battle_slots.txt')
REGIONS = os.path.join(DESIGN, 'regions.txt')

FIRST_BATTLE_ID = 10001
LAST_BATTLE_ID = 10640
FIRST_WORLD_ID = 20001
LAST_WORLD_ID = 20064

ERA = {
    'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
    'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
}


def tiles():
    out = []
    with open(REGIONS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append((p[0], p[1], p[2]))
    return out


def battle_slots():
    out = []
    with open(SLOTS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            out.append(dict(level_id=int(p[0]), tid=p[1], slot=int(p[2]),
                            grid_key=p[3], era=p[4], kind=p[5]))
    return out


def read_text(path):
    """Project tables are GBK, story DSL files are UTF-8; preserve each encoding."""
    for enc in ('utf-8', 'gbk'):
        try:
            with open(path, encoding=enc) as f:
                return f.read(), enc
        except UnicodeDecodeError:
            continue
    raise RuntimeError('unknown encoding: ' + path)


def write_text(path, text, enc):
    with open(path, 'w', encoding=enc) as f:
        f.write(text)


def world_rows(rows, existing):
    """Keep existing 20001..20064 rows; otherwise regenerate from regions.txt."""
    if existing:
        return existing
    out = []
    for i, (tid, _code, _name) in enumerate(rows):
        out.append('\t'.join([
            str(FIRST_WORLD_ID + i), 'Battle_' + tid, '2',
            'Story/Common/world_main.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl;Story/Common/autorun.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl',
            '3;100', '60', '70', '10', 'RoomSvr1', '1', '1', '24', '0',
        ]))
    return out


def rewrite_level(rows, slots):
    text, enc = read_text(LEVEL)
    lines = text.splitlines()
    head = lines[:4]                       # types / names / 2 comment rows
    legacy = [l for l in lines[4:] if l.strip() and
              int(l.split('\t')[0]) < FIRST_BATTLE_ID]
    existing_world = [l for l in lines[4:] if l.strip() and
                      FIRST_WORLD_ID <= int(l.split('\t')[0]) <= LAST_WORLD_ID]
    if not os.path.exists(LEVEL.replace('Level.txt', 'Level_pre640.txt.bak')):
        shutil.copyfile(LEVEL, LEVEL.replace('Level.txt', 'Level_pre640.txt.bak'))
    out = list(head) + legacy
    for s in slots:
        scene = 'Battle_%s_%s_s%02d' % (s['tid'], s['era'], s['slot'])
        story = 'Story/%s/story.dsl' % scene
        out.append('\t'.join([
            str(s['level_id']), scene, '4', story,
            'Story/Common/autorun_battle.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl',
            '3;100', '60', '70', '10', 'RoomSvr1', '4', '4', '24', '1',
        ]))
    out.extend(world_rows(rows, existing_world))
    write_text(LEVEL, '\n'.join(out) + '\n', enc)
    return len(slots), len(out) - len(head) - len(legacy) - len(slots)


def bump_rotation():
    """Raise the single-integer traversal bound in Story1/Story2 to 10640."""
    n = 0
    for name in ('Story1', 'Story2'):
        p = os.path.join(DSL, name, 'story.dsl')
        if not os.path.exists(p):
            continue
        s, enc = read_text(p)
        s2 = s.replace('10064', str(LAST_BATTLE_ID))
        if s2 != s:
            write_text(p, s2, enc)
            n += 1
    return n


def main():
    rows = tiles()
    slots = battle_slots()
    assert len(rows) == 64 and len(slots) == 640
    assert slots[0]['level_id'] == FIRST_BATTLE_ID
    assert slots[-1]['level_id'] == LAST_BATTLE_ID
    nb, nw = rewrite_level(rows, slots)
    print('tiles=%d battle_rows=%d world_rows=%d' % (len(rows), nb, nw))
    print('rotation_files_updated=%d' % bump_rotation())


if __name__ == '__main__':
    main()
