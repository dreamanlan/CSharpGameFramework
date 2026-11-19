# -*- coding: utf-8 -*-
"""
LEGACY / DO NOT RUN after design doc 35 (640-battlefield layout).

阶段 1 实机修正：让 64 个战场的入口遭遇能真正打完并轮换。

警告：本脚本按旧布局（每格 1 个战场、id 10001..10064）重写 story，
在 640 战场布局上运行会覆盖 build_story.py 生成的 640 份故事并使用错误的
close_battle id。阶段 3 之后请只用 build_story.py。保留此文件仅供历史回溯。

原因：新地形有起伏，story 骨架仍把实体放在 y=0（埋地下），且战斗区在世界
120~440、玩家在 (60,70)，AI 感知不到 -> 全场 Idle、all_killed 永不触发。

做法（不改 .unity、不改已烘焙的 NavMesh）：
  - 从 {tid}_areas.txt 读取传送点（type=250）世界坐标
  - 从 {tid}_terrain.png 读取该点 R -> 世界高度 R/255*70
  - 玩家/队友/怪都围绕传送点生成，y 用真实地形高度
  - 多区域布局数据保留在 MapData，阶段 2 用它做「按区域推进 + 玩家移动」

写完需外部执行 DslCopy 同步两端。
"""
import os
from PIL import Image

ROOT = r'D:\GitHub\CSharpGameFramework'
DESIGN = os.path.join(ROOT, 'Resource', 'MapDesign')
TILES = os.path.join(DESIGN, 'tiles')
AREAS = os.path.join(ROOT, 'Unity3d', 'Assets', 'Resources', 'MapData')
DSL = os.path.join(ROOT, 'Resource', 'DslFile', 'Story')

ERA = {
    'ARR': 'flood', 'ORB': 'prediluvian', 'LEV': 'sumerian', 'MES': 'sumer',
    'EGY': 'prediluvian', 'ABZ': 'prediluvian', 'AMR': 'postdiluvian', 'PAC': 'ancient',
}
TERRAIN_HEIGHT = 70.0


def tiles():
    out = []
    with open(os.path.join(DESIGN, 'regions.txt'), encoding='utf-8') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            p = [x.strip() for x in line.split('|')]
            out.append((p[0], p[1]))
    return out


def portal_of(tid):
    p = os.path.join(AREAS, tid + '_areas.txt')
    with open(p, encoding='utf-8-sig') as f:
        for line in f:
            if line.startswith('#') or not line.strip():
                continue
            q = [x.strip() for x in line.split('|')]
            if len(q) >= 3 and q[0] == '250':
                return float(q[1]), float(q[2])
    return 60.0, 70.0


def ground_y(tid, wx, wz):
    img = Image.open(os.path.join(TILES, tid + '_terrain.png')).convert('RGB')
    px = img.load()
    x = max(0, min(255, int(wx / 2)))
    y = max(0, min(255, int(wz / 2)))
    return px[x, y][0] / 255.0 * TERRAIN_HEIGHT


TEMPLATE = '''story(battle_main)
{{
  // Stage 1: entry encounter at the portal. Multi-area progression driven by
  // MapData/{{tid}}_areas*.txt is a stage-2 feature (player movement + events).
  onmessage("start")
  {{
    create_npc(1000,vector3({px},{py},{pz}),0,3,1,"ai_player",string_list("Ai/ailogic_player.dsl"));
    set_leader_id(unit_id_2_obj_id(1000));
    npc_set_formation(1000,0);
    loop(4){{
      create_npc(1001+$$,vector3({px}+rnd_int(0,10),{py},{pz}+rnd_int(0,10)),0,3,$$+2,"ai_member",string_list("Ai/ailogic_member.dsl"),get_leader_id());
      npc_set_formation(1001+$$,$$+1);
    }};
    camera_follow(1000);
    publish_gfx_event("loading_complete", "ui");
    start_story("auto_battle");
    loop(12){{
      create_npc(1006+$$,vector3({mx}+rnd_int(0,30),{py},{mz}+rnd_int(0,30)),0,4,$$+6,"ai_normal",string_list("Ai/ailogic_normal.dsl"));
    }};
  }};
  onmessage("all_killed")
  {{
    clear_message("show_dlg");
    fire_message("show_dlg",998);
  }};
  onmessage("npc_killed:1000")
  {{
    clear_message("show_dlg");
    fire_message("show_dlg",999);
  }};
  onmessage("all_died")
  {{
    clear_message("show_dlg");
    fire_message("show_dlg",999);
  }};
  onmessage("dialog_over",998)
  {{
    terminate();
    close_battle({level_id});
  }};
  onmessage("dialog_over",999)
  {{
    terminate();
    close_battle({level_id});
  }};
}};
'''


def main():
    rows = tiles()
    for i, (tid, code) in enumerate(rows):
        era = ERA[code]
        wx, wz = portal_of(tid)
        y = ground_y(tid, wx, wz)
        txt = TEMPLATE.format(
            tid=tid, px='%.0f' % wx, pz='%.0f' % wz, py='%.1f' % y,
            mx='%.0f' % (wx - 15), mz='%.0f' % (wz - 15),
            level_id=10001 + i)
        d = os.path.join(DSL, 'Battle_%s_%s' % (tid, era))
        os.makedirs(d, exist_ok=True)
        with open(os.path.join(d, 'story.dsl'), 'w', encoding='utf-8') as f:
            f.write(txt)
        with open(os.path.join(DESIGN, 'stories', tid + '_story.dsl'), 'w', encoding='utf-8') as f:
            f.write(txt)
    print('stories_rewritten=%d' % len(rows))
    wx, wz = portal_of('r0c0')
    print('sample r0c0 portal=(%.0f,%.0f) y=%.1f' % (wx, wz, ground_y('r0c0', wx, wz)))


if __name__ == '__main__':
    main()
