# -*- coding: utf-8 -*-
"""
让遭遇能真正打起来：调整 Actor 表的视野/回家范围。

原值：主角1 view=1000/gohome=1000；队友(2~5) view=30/gohome=30；
      怪(6+) view=15/gohome=20。
问题：怪视野 15 小于刷怪散布半径（原 ±18 双轴），且一离出生点 20 就切
      Gohome，追击与回家来回震荡；主角却在 1000 视野里朝远处放技能。
      表现就是「怪像僵尸瞎跑、技能乱飞、看不出在打谁」。

这里把怪的视野放大到能覆盖遭遇圈、把回家范围放宽到不会在战斗中触发；
配合 build_story.py 把刷怪收紧到 ±8，形成稳定的近距离交战。
"""
import os

ROOT = r'D:\GitHub\CSharpGameFramework'
ACTOR = os.path.join(ROOT, 'Resource', 'Tables', 'Public', 'Actor.txt')

RULES = {
    'ally': (45, 80),
    'mob': (35, 80),
}


def decode(path):
    raw = open(path, 'rb').read()
    for enc in ('utf-8', 'gbk'):
        try:
            return raw.decode(enc), enc
        except UnicodeDecodeError:
            continue
    raise RuntimeError('unknown encoding: ' + path)


def main():
    text, enc = decode(ACTOR)
    lines = text.splitlines()
    out = []
    changed = 0
    for line in lines:
        p = line.split('\t')
        if len(p) > 21 and p[0].isdigit():
            try:
                actor_id = int(p[0])
            except ValueError:
                actor_id = -1
            if actor_id > 1:
                name = p[1]
                if '怪' in name:
                    view, home = RULES['mob']
                elif '主角' in name:
                    view, home = RULES['ally']
                else:
                    view, home = None, None
                if view is not None and (p[20] != str(view) or p[21] != str(home)):
                    p[20] = str(view)
                    p[21] = str(home)
                    line = '\t'.join(p)
                    changed += 1
        out.append(line)
    with open(ACTOR, 'w', encoding=enc, newline='') as f:
        f.write('\n'.join(out) + '\n')
    print('Actor.txt enc=%s changed=%d' % (enc, changed))
    for line in out:
        p = line.split('\t')
        if len(p) > 21 and p[0].isdigit() and 1 <= int(p[0]) <= 12:
            print('  id=%-3s %-12s view=%-5s gohome=%s' % (p[0], p[1], p[20], p[21]))


if __name__ == '__main__':
    main()
