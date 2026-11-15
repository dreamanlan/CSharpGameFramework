# -*- coding: utf-8 -*-
"""
把 64 个现代层场景注册为世界地图格（type=2 剧情/野外场景）。

- id 20001..20064, 行优先，与 10001..10064 的战场格一一对应
- prefab = Battle_{tid}（现代层 .unity，阶段 1 已生成）
- CanPK = 0：世界层只有移动交互，没有战斗
- 1 thread x 1 room：避免 64 格 x 16 房间把服务端启动撑爆
- SceneDslFile 复用通用的 Story/Common/world_main.dsl

只追加，不改动现有 10001..10064 战斗行，现有自动轮换不受影响。
"""
import os

ROOT = r'D:\GitHub\CSharpGameFramework'
LEVEL = os.path.join(ROOT, 'Resource', 'Tables', 'Public', 'Level.txt')
REGIONS = os.path.join(ROOT, 'Resource', 'MapDesign', 'regions.txt')


def tiles():
    out = []
    with open(REGIONS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            out.append(line.split('|')[0].strip())
    return out


def main():
    raw = open(LEVEL, 'rb').read()
    enc = None
    for e in ('utf-8', 'gbk'):
        try:
            text = raw.decode(e)
            enc = e
            break
        except UnicodeDecodeError:
            continue
    assert enc, 'unknown Level.txt encoding'

    lines = text.splitlines()
    if any(l.startswith('20001\t') for l in lines):
        lines = [l for l in lines if not (l[:5].isdigit() and 20001 <= int(l.split('\t')[0]) <= 20064)]
    ids = tiles()
    for i, tid in enumerate(ids):
        cols = [
            str(20001 + i), 'Battle_' + tid, '2',
            'Story/Common/world_main.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl;Story/Common/autorun.dsl',
            'Story/Common/localmain.dsl;Story/Common/skillstory.dsl',
            '3;100', '60', '70', '10', 'RoomSvr1', '1', '1', '24', '0',
        ]
        lines.append('\t'.join(cols))
    with open(LEVEL, 'w', encoding=enc, newline='') as f:
        f.write('\n'.join(lines) + '\n')
    print('Level.txt enc=%s world_rows=%d total_lines=%d' % (enc, len(ids), len(lines)))


if __name__ == '__main__':
    main()
