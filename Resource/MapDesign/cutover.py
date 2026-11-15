# -*- coding: utf-8 -*-
"""
阶段 1 收尾：把 64 格时代层战场接入运行时表与故事脚本。

- 64 个 story.dsl 骨架 -> Resource/DslFile/Story/Battle_{tid}_{era}/story.dsl
- Level.txt 的 12 行战斗场景 -> 64 行（10001..10064，指向时代层 .unity）
- Story1/Story2 的轮换上限 10012 -> 10064
- 原 Level.txt 备份为 Level_pre64.txt.bak

同步（TableReaderGenerator / DslCopy）由外部命令完成，不在此脚本内。
"""
import os
import shutil

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
DSL = os.path.join(ROOT, 'Resource', 'DslFile', 'Story')
LEVEL = os.path.join(ROOT, 'Resource', 'Tables', 'Public', 'Level.txt')

ERA = {
    'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
    'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
}


def tiles():
    out = []
    with open(os.path.join(DESIGN, 'regions.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append((p[0], p[1], p[2]))
    return out


def write_stories(rows):
    n = 0
    for tid, code, name in rows:
        era = ERA[code]
        src = os.path.join(DESIGN, 'stories', tid + '_story.dsl')
        if not os.path.exists(src):
            print('MISS_STORY', tid)
            continue
        d = os.path.join(DSL, 'Battle_' + tid + '_' + era)
        os.makedirs(d, exist_ok=True)
        shutil.copyfile(src, os.path.join(d, 'story.dsl'))
        n += 1
    return n


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


def rewrite_level(rows):
    text, enc = read_text(LEVEL)
    lines = text.splitlines()
    head = lines[:8]
    shutil.copyfile(LEVEL, LEVEL.replace('Level.txt', 'Level_pre64.txt.bak'))
    out = list(head)
    for i, (tid, code, name) in enumerate(rows):
        era = ERA[code]
        scene = 'Battle_%s_%s' % (tid, era)
        story = 'Story/%s/story.dsl' % scene
        out.append('\t'.join([
            str(10001 + i), scene, '4', story,
            'Story/Common/autorun_battle.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl',
            '3;100', '60', '70', '10', 'RoomSvr1', '4', '4', '24', '1',
        ]))
    write_text(LEVEL, '\n'.join(out) + '\n', enc)
    return len(out) - len(head)


def bump_rotation():
    n = 0
    for name in ('Story1', 'Story2'):
        p = os.path.join(DSL, name, 'story.dsl')
        if not os.path.exists(p):
            continue
        s, enc = read_text(p)
        c = s.count('10012')
        if c:
            write_text(p, s.replace('10012', '10064'), enc)
            n += c
    return n


def main():
    rows = tiles()
    print('tiles=%d' % len(rows))
    print('stories=%d' % write_stories(rows))
    print('level_battle_rows=%d' % rewrite_level(rows))
    print('rotation_bumps=%d' % bump_rotation())


if __name__ == '__main__':
    main()
