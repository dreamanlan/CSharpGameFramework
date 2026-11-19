# -*- coding: utf-8 -*-
"""
Stage 3: generate 640 battlefield stories (design doc 35).

One 32x32 neighbourhood per battlefield -> one event point per story. The old
per-tile "2~4 battle zones" chain no longer applies, so each battlefield gets
a compact 2-wave encounter: wave 1 in place, wave 2 after a short advance
whose direction is derived deterministically from the slot. All numbers
(ADVANCE_STEP=20, spawn ring=14, view=35, gohome=80) stay valid because the
magnified battlefield is still 512x512.

Data sources:
  battle_slots.txt                 640 rows: level_id / grid_key / era / kind / x,z
  tiles/{tid}_terrain.png          world-tile height (for narrative only)
  WorldEvent.txt                   event count per battlefield (GBK)

Every story keeps the shared Common DSL (autorun_battle / localmain /
skillstory), so click-to-move, explore and reporting still work unchanged.
"""
import os
import math
import random
from PIL import Image

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
STORY = os.path.join(ROOT, 'Resource', 'DslFile', 'Story')
EVENTS = os.path.join(ROOT, 'Resource', 'Tables', 'Server', 'WorldEvent.txt')
SLOTS = os.path.join(DESIGN, 'battle_slots.txt')

ADVANCE_STEP = 20.0
ADVANCE_TIMEOUT_MS = 4000
WAVES = 2

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


def slots():
    out = []
    with open(SLOTS, encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = line.rstrip('\n').split('\t')
            out.append(dict(level_id=int(p[0]), tid=p[1], slot=int(p[2]),
                            grid_key=p[3], era=p[4], kind=p[5],
                            x=float(p[6]), z=float(p[7]), title=p[11],
                            code=p[12], name=p[13], src=p[14],
                            diff=int(p[15]), route=p[16]))
    return out


def ground_y(s, wx, wz):
    """Spawn height from the battlefield's own magnified terrain PNG."""
    png = os.path.join(TILES, s['grid_key'] + '_terrain.png')
    if not os.path.exists(png):
        png = os.path.join(TILES, s['tid'] + '_terrain.png')
    img = Image.open(png).convert('RGB')
    px = img.load()
    x = max(0, min(255, int(wx / 2)))
    y = max(0, min(255, int(wz / 2)))
    return px[x, y][0] / 255.0 * 70.0


def event_count(grid_key):
    if not os.path.exists(EVENTS):
        return 0
    n = 0
    with open(EVENTS, encoding='gbk') as f:
        for line in f:
            p = line.split('\t')
            if len(p) > 1 and p[1] == grid_key:
                n += 1
    return n


def esc(s):
    return s.replace('\\', '\\\\').replace('"', '\\"')


def signed(v):
    return ('+%.1f' % v) if v >= 0 else ('-%.1f' % -v)


def advance_vector(s):
    """Deterministic short advance direction, seeded by battlefield key."""
    rnd = random.Random(s['level_id'] * 7919 + 13)
    ang = rnd.uniform(-math.pi, math.pi)
    return ADVANCE_STEP * math.cos(ang), ADVANCE_STEP * math.sin(ang)


def wave_branch(s, w):
    zone = ZONES[s['code']][(s['slot'] + w - 1) % len(ZONES[s['code']])]
    action = 'wave_spawn_1' if w == 1 else 'wave_advance_%d' % w
    return [
        '      if(@wave<%d){' % (w + 1),
        '        fire_message("push_tip_info", "%s：%s·%s");' %
        (esc(s['name']), esc(zone), esc(s['src'])),
        '        fire_message("%s");' % action,
        '      } else {',
    ]


def advance_handler(w, dx, dz):
    return [
        '  onmessage("wave_advance_%d")' % w,
        '  {',
        '    @advancing=1;',
        '    fire_message("push_tip_info", "向下一处遗迹推进 %d 步（第 %d 波）");' %
        (int(ADVANCE_STEP), w),
        '    obj_move(unit_id_2_obj_id(1000),vector3(get_position_x(unit_id_2_obj_id(1000))%s,'
        'get_position_y(unit_id_2_obj_id(1000)),get_position_z(unit_id_2_obj_id(1000))%s),'
        '"npcarrived:1000");' % (signed(dx), signed(dz)),
        '    wait(%d);' % ADVANCE_TIMEOUT_MS,
        '    if(@advancing>0){',
        '      @advancing=0;',
        '      fire_message("wave_spawn_%d");' % w,
        '    };',
        '  };',
    ]


def arrived_handler(total):
    lines = ['  onmessage("npcarrived:1000")', '  {', '    if(@advancing>0){', '      @advancing=0;']
    for w in range(1, total + 1):
        lines.append('      if(@wave<%d){' % (w + 1))
        lines.append('        fire_message("wave_spawn_%d");' % w)
        lines.append('      } else {')
    for _ in range(total):
        lines.append('      };')
    lines += ['    };', '  };']
    return lines


def spawn_handler(w, diff):
    count = 4 + diff // 2
    base = 1006 + w * 20
    radius = 14.0
    lines = [
        '  onmessage("wave_spawn_%d")' % w,
        '  {',
        '    if(@spawned<@wave){',
        '      @spawned=@wave;',
    ]
    for i in range(count):
        ang = 2.0 * math.pi * i / count
        dx, dz = round(radius * math.cos(ang), 1), round(radius * math.sin(ang), 1)
        lines.append(
            '      create_npc(%d,vector3(get_position_x(unit_id_2_obj_id(1000))%s,'
            'get_position_y(unit_id_2_obj_id(1000)),'
            'get_position_z(unit_id_2_obj_id(1000))%s),'
            '0,4,%d,"ai_normal",string_list("Ai/ailogic_normal.dsl"));'
            % (base + i, signed(dx), signed(dz), 6 + i))
    lines += ['    };', '  };']
    return lines


def build(s):
    code, name, src = s['code'], s['name'], s['src']
    level_id = s['level_id']
    scene = 'Battle_%s_%s_s%02d' % (s['tid'], s['era'], s['slot'])
    py = ground_y(s, 60.0, 70.0)
    ev = event_count(s['grid_key'])
    era = ERA_LABEL[code]
    main = '这条线索连着主线，别急着离开。' if s['route'] == '主线' else '支线记载，或许能补上别处缺的那一页。'
    zone_hint = '这里留着主轴线索。' if s['kind'] == 'axis' else '这是一处次要勘察点。'

    L = ['story(battle_main)', '{',
         '  // %s | %s | %s | %s | level=%d' % (s['grid_key'], name, era, s['kind'], level_id),
         '  local', '  {', '    @wave(0);', '    @spawned(0);', '    @advancing(0);',
         '    @total(%d);' % WAVES, '  };',
         '  onmessage("start")', '  {',
         '    create_npc(1000,vector3(60,%.1f,70),0,3,1,"ai_player",string_list("Ai/ailogic_player.dsl"));' % py,
         '    set_leader_id(unit_id_2_obj_id(1000));',
         '    npc_set_formation(1000,0);',
         '    loop(4){',
         '      create_npc(1001+$$,vector3(60+rnd_int(0,10),%.1f,70+rnd_int(0,10)),0,3,$$+2,"ai_member",string_list("Ai/ailogic_member.dsl"),unit_id_2_obj_id(1000));' % py,
         '      npc_set_formation(1001+$$,$$+1);',
         '    };',
         '    camera_follow();',
         '    publish_gfx_event("loading_complete", "ui");',
         '    fire_message("set_map_image", "Minimap/%s_minimap");' % s['grid_key'],
         '    send_room_story_message("enter_parallel", "%s");' % s['grid_key'],
         '    start_story("auto_battle");',
         '    fire_message("push_tip_info", "【%s】%s");' % (esc(name), esc(OPENING[code])),
         '    fire_message("push_tip_info", "时代层：%s。%s");' % (esc(era), esc(main)),
         '    fire_message("push_tip_info", "%s");' % esc(zone_hint)]
    if ev > 0:
        L.append('    fire_message("push_tip_info", "这一带留着线索，靠近后按 E 调查");')
    L += ['    fire_message("wave_go");', '  };',
          '  onmessage("wave_go")', '  {', '    inc(@wave);',
          '    if(@wave>@total){',
          '      fire_message("push_tip_info", "%s");' % esc(ENDING[code]),
          '      clear_message("show_dlg");', '      fire_message("show_dlg",998);',
          '    } else {']
    for w in range(1, WAVES + 1):
        L.extend(wave_branch(s, w))
    for _ in range(WAVES):
        L.append('      };')
    L += ['    };', '  };']
    for w in range(1, WAVES + 1):
        L.extend(spawn_handler(w, s['diff']))
    for w in range(2, WAVES + 1):
        dx, dz = advance_vector(s)
        L.extend(advance_handler(w, dx, dz))
    L.extend(arrived_handler(WAVES))
    L += ['  onmessage("all_killed")', '  {', '    fire_message("wave_go");', '  };',
          '  onmessage("npc_killed:1000")', '  {',
          '    clear_message("show_dlg");', '    fire_message("show_dlg",999);', '  };',
          '  onmessage("all_died")', '  {',
          '    clear_message("show_dlg");', '    fire_message("show_dlg",999);', '  };',
          '  onmessage("dialog_over",998){ terminate(); close_battle(%d); };' % level_id,
          '  onmessage("dialog_over",999){ terminate(); close_battle(%d); };' % level_id,
          '};']

    d = os.path.join(STORY, scene)
    os.makedirs(d, exist_ok=True)
    with open(os.path.join(d, 'story.dsl'), 'w', encoding='utf-8', newline='') as f:
        f.write('\n'.join(L) + '\n')
    return scene


def main():
    rows = slots()
    for s in rows:
        build(s)
    print('STORIES battlefields=%d waves_each=%d' % (len(rows), WAVES))
    print('level_id range=%d..%d' % (min(r['level_id'] for r in rows),
                                     max(r['level_id'] for r in rows)))


if __name__ == '__main__':
    main()
