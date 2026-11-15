# -*- coding: utf-8 -*-
"""
把 64 个战斗 story 从「同一个刷怪模板」补成有世界观叙事的章节关卡。

数据源（设计意图都在数据里，脚本只做翻译）：
  regions.txt                       格名 / 大区 / 时代层 / 主线支线 / 素材引用 / 难度
  MapData/{tid}_areas.txt           战斗区中心（来自布局图）
  tiles/{tid}_terrain.png           真实地表高度（R 通道）
  Tables/Server/WorldEvent.txt      仅用于统计本格线索数（不下发坐标）

生成结构：
  开场叙事 -> 逐战斗区推进（objmove 带玩家过去 + 分段文案 + 分波刷怪）
  -> all_killed 推进下一波 -> 最后一波清完结算
  怪刷在玩家当前位置附近，保证 objmove 若不生效也仍能打完、循环不卡。

坐标：terrain PNG 256x256，1px=2 世界单位；C# 读图时做了上下翻转，
      故 pil_y = 255 - worldZ/2，height = R/255*70。
"""
import os
from PIL import Image

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
AREAS = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'MapData')
STORY = os.path.join(ROOT, 'Resource', 'DslFile', 'Story')
EVENTS = os.path.join(ROOT, 'Resource', 'Tables', 'Server', 'WorldEvent.txt')

OPENING = {
    'ARR': '洪水退去后，冰盖裂隙里还留着方舟的木痕。',
    'ORB': '信标仍在发送坐标，可接收者早已离开这颗星球。',
    'LEV': '巨石平台不该由人力垒成，落地痕迹却指向天空。',
    'MES': '泥板写着「王权自天而降」，城邦的砖缝里却全是灰。',
    'EGY': '金字塔不是陵墓，它的甬道对准某颗尚未归来的星。',
    'ABZ': '竖井太深了，深到不像为了开采，更像为了离开。',
    'AMR': '太阳门的浮雕记着一次渡海，也记着一次诀别。',
    'PAC': '这些石像不看向海，它们一致看向天上。',
}
ZONES = {
    'ARR': ['冰蚀裂口', '方舟残骸带', '洪水冲刷阶地', '彩虹之约台'],
    'ORB': ['信标残座', '废弃中转舱', '环形陨坑', '归来之门'],
    'LEV': ['巨石平台', '西奈着陆场', '变乱之碑', '流亡古道'],
    'MES': ['城邦外廓', '泥板库前庭', '塔庙阶道', '两河渡口'],
    'EGY': ['甬道入口', '狮身台地', '顶端晶石室', '旧战场'],
    'ABZ': ['采掘竖井', '矿工营墟', '基因工房', '伊甸残圃'],
    'AMR': ['太阳门广场', '冶金炉场', '观星台', '失落国度边墙'],
    'PAC': ['导航石阵', '洪前航图滩', '深潜礁盘', '归航信标'],
}
ENDING = {
    'ARR': '碎片拼出一段被抹去的洪水记录。',
    'ORB': '坐标串起来了，但发送者仍未露面。',
    'LEV': '平台的作用终于清楚：它不是祭坛，是着陆场。',
    'MES': '「王权自天而降」不再是一句颂词。',
    'EGY': '星象对齐的结果指向回归的时刻表。',
    'ABZ': '原来最初要找的不是奴隶，是金。',
    'AMR': '渡海者带走的不是黄金，是另一份记载。',
    'PAC': '所有标记指向同一个方向：天上。',
}
ERA_LABEL = {
    'ARR': '洪水层', 'ORB': '洪水前层', 'LEV': '苏美尔层', 'MES': '苏美尔层',
    'EGY': '洪水前层', 'ABZ': '洪水前层', 'AMR': '洪水后层', 'PAC': '远古层',
}


def tiles():
    out = []
    with open(os.path.join(DESIGN, 'regions.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append(dict(tid=p[0], code=p[1], name=p[2], era=p[3],
                            route=p[4], src=p[6], diff=int(p[7]), seq=int(p[8])))
    return out


def battle_zones(tid):
    p = os.path.join(AREAS, tid + '_areas.txt')
    out = []
    if not os.path.exists(p):
        return out
    with open(p, encoding='utf-8-sig') as f:
        for line in f:
            q = [x.strip() for x in line.split('|')]
            if len(q) < 4 or not q[0].isdigit():
                continue
            t = int(q[0])
            if 60 <= t <= 140:
                out.append((float(q[1]), float(q[2]), float(q[3])))
    return out[:4]


def event_count(tid):
    if not os.path.exists(EVENTS):
        return 0
    n = 0
    with open(EVENTS, encoding='utf-8') as f:
        for line in f:
            p = line.split('\t')
            if len(p) > 1 and p[1] == tid:
                n += 1
    return n


def ground_y(tid, wx, wz):
    img = Image.open(os.path.join(TILES, tid + '_terrain.png')).convert('RGB')
    px = img.load()
    x = max(0, min(255, int(wx / 2)))
    y = max(0, min(255, int(wz / 2)))
    return px[x, y][0] / 255.0 * 70.0


def esc(s):
    return s.replace('\\', '\\\\').replace('"', '\\"')


def wave_branch(code, name, w, x, y, z, diff, src):
    zone = ZONES[code][(w - 1) % len(ZONES[code])]
    count = 4 + diff // 2
    base = 1006 + w * 20
    lines = [
        '      if(@wave<%d){' % (w + 1),
        '        firemessage("push_tip_info", "%s：%s·%s");' % (esc(name), esc(zone), esc(src)),
        # Move first, then spawn on arrival. A timed fallback guarantees the
        # wave still spawns (at the player's current position) if objmove never
        # completes, so the battle loop can never stall.
        '        objmove(getleaderid(), vector3(%.0f,%.1f,%.0f), "wave_spawn_%d");' % (x, y, z, w),
        '        wait(3000);',
        '        firemessage("wave_spawn_%d");' % w,
        '      } else {',
    ]
    return lines


def spawn_handler(w, diff):
    count = 4 + diff // 2
    base = 1006 + w * 20
    return [
        '  onmessage("wave_spawn_%d")' % w,
        '  {',
        '    if(@spawned<@wave){',
        '      @spawned=@wave;',
        '      loop(%d){' % count,
        '        createnpc(%d+$$,vector3(getpositionx(getleaderid())+rndint(0,18),getpositiony(getleaderid()),getpositionz(getleaderid())+rndint(0,18)),0,4,$$+6,"ai_normal",stringlist("Ai/ailogic_normal.dsl"));' % base,
        '      };',
        '    };',
        '  };',
    ]


def build(t, rank):
    tid, code, name = t['tid'], t['code'], t['name']
    era_suffix = {
        'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
        'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
    }[code]
    zones = battle_zones(tid)
    if not zones:
        zones = [(60.0, 70.0, 30.0), (200.0, 200.0, 30.0)]
    total = len(zones)
    py = ground_y(tid, 60.0, 70.0)
    ev = event_count(tid)
    level_id = 10000 + rank
    opening = OPENING[code]
    era = ERA_LABEL[code]
    main = '这条线索连着主线，别急着离开。' if t['route'] == '主线' else '支线记载，或许能补上别处缺的那一页。'

    L = []
    L.append('story(battle_main)')
    L.append('{')
    L.append('  // %s | %s | %s | %s' % (tid, name, era, t['src']))
    L.append('  local')
    L.append('  {')
    L.append('    @wave(0);')
    L.append('    @spawned(0);')
    L.append('    @total(%d);' % total)
    L.append('  };')
    L.append('  onmessage("start")')
    L.append('  {')
    L.append('    createnpc(1000,vector3(60,%.1f,70),0,3,1,"ai_player",stringlist("Ai/ailogic_player.dsl"));' % py)
    L.append('    setleaderid(unitid2objid(1000));')
    L.append('    npcsetformation(1000,0);')
    L.append('    loop(4){')
    L.append('      createnpc(1001+$$,vector3(60+rndint(0,10),%.1f,70+rndint(0,10)),0,3,$$+2,"ai_member",stringlist("Ai/ailogic_member.dsl"),getleaderid());' % py)
    L.append('      npcsetformation(1001+$$,$$+1);')
    L.append('    };')
    # No-arg form uses the already-set LeaderId directly; the unit-id form
    # silently does nothing when GetEntityByUnitId misses.
    L.append('    camerafollow();')
    L.append('    publishgfxevent("loading_complete", "ui");')
    L.append('    firemessage("set_map_image", "Minimap/%s_minimap_%s");' % (tid, era_suffix))
    L.append('    startstory("auto_battle");')
    L.append('    firemessage("push_tip_info", "【%s】%s");' % (esc(name), esc(opening)))
    L.append('    firemessage("push_tip_info", "时代层：%s。%s");' % (esc(era), esc(main)))
    if ev > 0:
        L.append('    firemessage("push_tip_info", "这一带留着线索，靠近后按 E 调查");')
    L.append('    firemessage("wave_go");')
    L.append('  };')
    L.append('  onmessage("wave_go")')
    L.append('  {')
    L.append('    inc(@wave);')
    L.append('    if(@wave>@total){')
    L.append('      firemessage("push_tip_info", "%s");' % esc(ENDING[code]))
    L.append('      clearmessage("show_dlg");')
    L.append('      firemessage("show_dlg",998);')
    L.append('    } else {')
    # Nested < comparisons only (no == dependency): wave 1 / 2 / 3 / 4.
    for i, (x, z, _r) in enumerate(zones):
        w = i + 1
        y = ground_y(tid, x, z)
        L.extend(wave_branch(code, name, w, x, y, z, t['diff'], t['src']))
    for _ in range(total):
        L.append('      };')   # close the nested else chain, one per line
    L.append('    };')
    L.append('  };')
    for w in range(1, total + 1):
        L.extend(spawn_handler(w, t['diff']))
    L.append('  onmessage("all_killed")')
    L.append('  {')
    L.append('    firemessage("wave_go");')
    L.append('  };')
    L.append('  onmessage("npc_killed:1000")')
    L.append('  {')
    L.append('    clearmessage("show_dlg");')
    L.append('    firemessage("show_dlg",999);')
    L.append('  };')
    L.append('  onmessage("all_died")')
    L.append('  {')
    L.append('    clearmessage("show_dlg");')
    L.append('    firemessage("show_dlg",999);')
    L.append('  };')
    L.append('  onmessage("dialog_over",998)')
    L.append('  {')
    L.append('    terminate();')
    L.append('    closebattle(%d);' % level_id)
    L.append('  };')
    L.append('  onmessage("dialog_over",999)')
    L.append('  {')
    L.append('    terminate();')
    L.append('    closebattle(%d);' % level_id)
    L.append('  };')
    L.append('};')
    return '\n'.join(L) + '\n'


def main():
    rows = tiles()
    for rank, t in enumerate(rows, 1):
        era_suffix = {
            'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
            'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
        }[t['code']]
        d = os.path.join(STORY, 'Battle_%s_%s' % (t['tid'], era_suffix))
        os.makedirs(d, exist_ok=True)
        with open(os.path.join(d, 'story.dsl'), 'w', encoding='utf-8') as f:
            f.write(build(t, rank))
    print('stories=%d' % len(rows))
    print('sample r2c4 zones=%d' % len(battle_zones('r2c4')))


if __name__ == '__main__':
    main()
