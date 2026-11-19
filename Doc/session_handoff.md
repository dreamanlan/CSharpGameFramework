# 会话交接摘要（压缩状态，2026-11-18）

用途：本会话已很长，后续继续游戏开发时先读这份，避免重复翻历史。完整过程见 `Doc/lost_civilization_design.md`（第 1~37 节，含 29.6 关闭避让实验、第 30~34 节点击移动/相机/小地图、第 35 节 640 战场重构三阶段、第 36 节世界层接入运行时第一阶段、第 37 节名字板残留修复）；改名经验见 `Doc/api_rename_sop.md`。

**当前最重要的事**：仓库已从「64 格 1 战场」改为「64 格 × 10 = 640 战场」。唯一数据源是 `Resource/MapDesign/battle_slots.txt`（640 行），所有生成器都读它；改布局请改 `build_battle_slots.py` 后依次重跑 `build_battlefield_art.py` → `build_story.py` → `build_events.py` → `cutover.py` → `dslcopy.bat`，再在 Unity 跑 `map_build()` / `map_nav()` / `map_verify()`。`map_nav()` 全量约 2.3 小时，可传格 id 分批。详见 `lost_civilization_design.md` 第 35 节。

## 1. 项目与当前阶段

- 主仓 `d:\GitHub\CSharpGameFramework`（含服务端 + Unity 工程）；旁仓 `d:\GitHub\GameDemo`（同源框架快照，独立 Unity demo，非编译期源码依赖）。
- 玩法项目「失落的文明」：8×8=64 格藏宝图、时代层战场、服务端权威探索事件表、累积收益。
- 阶段 1（64 格场景/NavMesh/表/轮换）与阶段 2 主体（探索闭环、技能、世界格）已完成；当前在**打磨战斗观感与工程规范**。

## 2. 当前环境状态（可直接接着干）

- 全套服务在跑：`ServerCenter×2 / node / Lobby / DataCache / UserServer / RoomServer`，9001 监听；`EXPLORE_SELFTEST RESULT=PASS`、`WORLD_SYNC_SELFTEST RESULT=PASS`（含 AOI 断言）。
- Unity：编辑器 Play 中（pid 50400，`ai_3137` 登录），编译无 `error CS`；MCP 的 editor + runtime 双通道均正常连出（39528），`unity_exec` / `unity_editor_exec` / `unity_logs` 可用。**2026-11-19 电脑重启后全套恢复**：服务按 `RunServer.cmd` 重启、Unity 冷启动（旧 pid 34000 → 9352 → 50400）；域重载后 editor 通道不自动连出时，按纪律重启编辑器即可恢复（弹管理员提示需点 `&I wish to continue at my own risk`）。
- **相机 `up=0` 已实机验证**：`camera_probe()` 读到 `setting=[distance=52 up=0 pitch=46]`、`camDist=52.0`，视线中心精确落在主角坐标上。详见 `lost_civilization_design.md` 第 32.6 节。
- **登录账号要每轮换新的**：同一账号重复登录会跳过 `show_nickname`、卡在后续房间分配（UI 消失但 story 不推进）；换随机账号即恢复。本轮用过 `ai_2234`、`ai_2240`。
- **改 Editor 侧 C# 后必须 `asset_refresh()`**：直接改文件 Unity 不会自动重编译（`Assembly-CSharp-Editor.dll` 时间戳不动），探针仍是旧代码。`asset_refresh()` 会触发域重载、GM 通道断约 30~60 秒后自动重连，期间查 `unity_info` 即可，不要用长阻塞等待。
- **改客户端 `PluginFramework` 的部署路径**：源码在 `App/ClientModule/PluginFramework/`，但 `buildall` 从 `App/ClientModule/ClientPlugins/bin/Debug/netstandard2.1/PluginFramework.dll` 取 DLL（`ClientPlugins.csproj` 以 ProjectReference 聚合）。改完要 `dotnet build ClientPlugins.csproj`，再把 `PluginFramework.dll`/`.pdb` 复制到 `Unity3d/Assets/Plugins/`，最后 `asset_refresh()`。
- 域重载后 editor 通道偶发不自动重连时，`SetForegroundWindow` 激活 Unity 主窗口即可触发 `delayCall` 恢复（本轮实证 12 秒内连上）。
- GM 登录自动化：`dotnet_set(find_input("Account"/"Passwd"),"text",...)` → `click_ui("Button")` → 昵称框自动随机再 `click_ui("Button")` → 剧情 5 秒/条自动推进 → 进战斗。本轮 `do_login → show_nickname → start_game → open_battle` 全链实机通过。
- 本轮实机证据：客户端收到 `parallel_state`、`wave_advance_2 → npcarrived:1000 → wave_spawn_2` 闭环、`report_pos` 300ms 持续；服务端 `explore_traffic.txt` 19:12 刷新为 `enter_parallel=2 / battle_pos=140 / session 119 grid=r0c1`，RoomServer 日志无异常。
- 服务端事件表走项目读表器：`Resource/Tables/Server/WorldEvent.txt`（GBK 标准表）→ `WorldEvent.dat` → `TableConfig.WorldEventProvider.LoadForServer()`；客户端零泄漏。
- **所有 story API / skill trigger 注册名已是 snake_case**，无兼容层。GM 命令必须用新名，例如 `dotnet_set(find_input(...))`、`click_ui(...)`、`combat_probe()`、`camera_probe()`、`explore_probe("auto")`、`map_verify()`、`asset_refresh()`。

## 3. 已完成的关键修复（按文档章节）

| 节 | 内容 |
|---|---|
| 14 | 64 格时代层场景、BuildSettings、NavMesh、切表；`fix_story_spawn.py` 修 y 埋地/AI 够不着 |
| 15 | 事件表隔离（客户端 hints / areas 净化）、`ExploreSystem` 校验闭环、自检 |
| 16 | 64 个世界格注册进 `Level.txt`（type=2、CanPK=0） |
| 17 | 选中圈 prefab 物理组件、小地图引用（原来写死 `Map1`）、`ResourceSystem` 域重载自愈、事件坐标 Y 镜像 |
| 18 | 相机跟随与视距（300m → ~35m，`map_camera_fix` 就地修 128 场景，保住 NavMesh） |
| 19 | 战斗观感四根因：AI 震荡、队友跑向原点、技能骨骼挂点、血条 `bloodbar` 挂点缺失 |
| 20 | 主动目标锁定、`enter_parallel` 空 grid 修复、端到端探索命中 `119\|160\|ev_r0c1_0`、技能特效三处 fallback |
| 21 | 事件表接入项目读表器、四步部署流程实证 |
| 22 | 遭遇环形刷怪、动画锁（`LockAnimation`）、主角 `ai_cast_skill` 不再被 `ai_chase` 挂起、朝向 `SetWantedFaceDir` |
| 23 | 全量 snake_case 改名、双 TargetFramework 构建顺序、关键字误伤修复、跨仓 DLL 同步 |
| 24 | 带区域推进的分段移动（20 单位/波、到达刷怪、超时兜底） |
| 25 | 主场景多玩家信息同步：位移（停止/到达/心跳/进房快照）与可见状态（快照回放/离开/超时广播） |
| 26 | 主场景 AOI 可见范围过滤 + 进入视野回放；四步部署与服务端实机自检 |
| 27 | 战斗观感：朝向定量探针、小地图显示参数核对、world 轮换卡住归因 |
| 28 | 动作/行为不一致诊断：ANIMDIAG 定位为技能动画被 Run 覆盖 |
| 29 | 方案 A 落地：动画锁按 clip 长度校准、锁到期补播；避让/碰撞与点地面脱离 AI 现状调查 |
| 30 | 关闭实体间 NavMeshAgent 避让（保留基线）+ 鼠标点击地面移动单点修 |
| 31 | 相机拉远为足球场观众视角（distance 52 / up 16 / pitch 46，运行时统一覆盖） |
| 32 | 玩家屏幕居中（up→0）、小地图黄/绿/红圆点、点击移动回归诊断 |
| 33 | 点击移动真正根因（LeaderId 缓存失效 → 实时 unitId 解析）与实机验证 |
| 34 | 相机再拉远一倍（distance 52→104，camDist=104）+ 点击 raycast 100→500 联动 |
| 35 | 640 战场重构（32×32 邻域 ×16 放大）：三阶段落地、704 场景、708 行 Level.txt |
| 36 | 世界地图层接入运行时·第一阶段：修视觉隐藏判定、Config 巡点、r0c0 样板、实机验证 |
| 37 | 名字板残留根因（Release 传错类型 / Additive 重复 Init / 卸载不清理）与修复 |

## 4. 未完成 / 下一步（按建议优先级）

-2. **名字板残留实机复测（2026-11-20 修复已完成，验证受阻）**：第 37 节三处根因已修（`OverHeadBarManager.Release()` 传错类型、Additive 重复 `Init()`、战场进出不清理），代码编译 0 错误、`PluginFramework.dll` 已部署，但 Unity 重启后 editor GM 通道始终未连出，**没有跑到 `combat_probe()`**。下次通道恢复后进 Play 连跑 3~4 场战斗，每场间隙看 `orphans=0` / `noParent=0` / `overheadBars≈entities`。`CombatProbe` 已新增这两个计数。

-1. **世界地图层接入运行时 · 第 2~5 阶段（2026-11-20 第一阶段已完成）**：第一阶段修了 `Game.cs` 视觉隐藏判定（按 levelId 精确识别战场，消除双层 Terrain）、世界层补 `Config/1..10` 巡点、新增 `map_world_build` / `map_world_nav`、完成 `r0c0` 单格样板并实机验证（以 20001 为基底自然进战斗、两波结算、Terrain/Camera 各只剩一套、无异常）。**未做**：正式入口 `change_scene(20001)` 及三端轮换链、服务端 `GridFromLevel` 扩到 20001+ 与跨格换房、相机/小地图多场景修正、4096 藏宝图 UI、铺满 64 格。详见 `lost_civilization_design.md` §36.5。当前 `MainUi/story.dsl:30` 仍是 `change_scene(101)`，运行时默认基底仍为 `Story1`。

0. ~~**NavMesh 全量烘焙 + 结算链复测**~~：**2026-11-19 21:30 已完成**。`map_nav()` 全量 640/640（0 缺失、大小正常、无异常），静态校验 story `close_battle` id 与 Level.txt prefab 各 0 处不匹配；实机两场战斗连续跑通「两波全清 → `dialog_over:998` → `UnloadBattle` → `on_battle_closed` → 换到下一个 id」，无异常。`BakeNav` 支持 `which` 分批与断点续跑。残留（非阻塞）：怪 `pos.y≈2.4` 与 `home.y=30.2` 不一致，但不影响交战结算。详见 `lost_civilization_design.md` §35.7。

1. ~~**改名后进战斗的 skill trigger 实机覆盖**~~：**2026-11-18 核查结案**。`Unity3d/Game.log`（2026-11-17 03:29 会话）03:36 起多轮 `LoadBattle` 期间 `CreateTriger` 以全 snake_case 名成功创建 `hit_effect` / `self_effect` / `adjust_section_duration` / `impact` / `aoe_impact` / `replace_shader_and_fade_color` 等触发器，全日志 `CreateTriger failed=0`、`unknown func=0`、`NullReference=0`；注册侧 `GfxSkillSystem.cs`、配置侧 `Skill/*.dsl` 与 `StreamingAssets/Dsl/Skill/` 副本均无旧名残留。该条目是未及时更新的过期记录。边界：无独立 verify 落盘文件、trigger 名未做 DLL 字符串抽查，只有运行时创建成功的间接确证。
2. ~~**带区域推进的分段移动**~~：**2026-11-17 已完成并实机验证通过**。每波 20 单位分段推进、到达后刷怪、超时兜底、leash 沿用 `HomePos+gohome=80`；64 个 story 三端同步，静态验证通过；全套服务 + Unity Play 实机跑通（`npcarrived:1000` 在推进后 2.31s 触发，多轮闭环，`unknown func=0`）。详见 `lost_civilization_design.md` 第 24 节。
3. ~~**朝向定量验证**~~：**2026-11-18 已完成**。`combat_probe()` 补 `face / wanted / faceErr / expect / expectErr` 与 `facing / expectErrAvg / expectErrMax` 汇总；实机两次采样主角（攻击态、`moving=False`）`faceErr=0.0`、`expectErr=1.7° / 2.6°`，朝向收敛到当前目标，`SetWantedFaceDir → RotateTowards` 生效。详见 `lost_civilization_design.md` 第 27 节。
4. **队友 Gohome 偏活跃**、追击阶段仍显示 `Run`（正常但观感可再调 `GohomeHandler` 容差）。本轮 `combat_probe()` 实机再次确认 4 个队友全为 `state=102` 且 `anim=Run`；同时发现**移动中实体 `faceErr` 可达 25°~100°**（转向插值尚未追上 `wanted`），静止/攻击态才收敛到 0。这是新观察到的观感项。
4b. ~~**动作/行为不一致**~~：**2026-11-19 方案 A 已修复并实机验证**。诊断结论是「播了但被覆盖」（`by Run after 0.603s`，对应固定 `LockAnimation(0.6f)`）。A 把锁改为按 `AnimatorStateInfo.length` 运行时校准、锁到期按实时 `IsMoving` 补播 `Run`/`Idle`、同状态重复 `Play` 直接跳过；诊断代码已全部删除。实机验证：`by Run` 覆盖 **0 次**，`Attack→0.837s`（clip 0.83）、`Death→2.071s`（clip 2.07）、`Stand→1.505s`，只剩按设计补播 `Idle`。详见 `lost_civilization_design.md` 第 28/29 节。
4c. **避让/碰撞（关闭状态已保留为基线）**：全仓无 `CharacterController`/`Rigidbody`/实体 `Collider`，"碰撞"实为 `NavMeshAgent` 的 `MedQualityObstacleAvoidance` 分离力；`SyncPosition()` 每帧把 Agent transform 无条件写回 `msi`，与服务端直线推进构成回写闭环。2026-11-19 加编译期开关 `c_EnableObstacleAvoidance = false` 关闭，用户实机反馈「关了碰撞后好多了」，据此保留 `false`。Y 偏差与短时聚集仍在，属已知代价。回滚：改回 `true` 并重新部署 DLL。详见 `lost_civilization_design.md` 第 29.6 节。
4d. **鼠标点击地面移动（两层根因均已修，超时脱离 AI 语义仍缺）**：① 64 个 `Battle_r*c*` 战场 story 与共用 `Common/*.dsl` 全都没有 `onmessage("move_to")`，只有已弃用的 `Battle1..12` 有，消息被 `StoryInstance` 静默回收 → 已在所有战场共用的 `Story/Common/autorun_battle.dsl` 加 handler（第 30 节）。② 加完仍不动，用 `CLICKDIAG` 临时日志打穿链路后确证：`obj_move` 执行了、参数绑定正确，但 `get_leader_id()` 返回的缓存 `LeaderId` 在战斗轮换实体重建后指向已销毁的旧 objId，`GetEntityById` 返回 null，整个 `if` 静默跳过 → 已把 handler 改为 `obj_move(unit_id_2_obj_id(1000),vector3($x,$y,$z))` 实时按 unitId 解析当前主角，实机 `entityNull` 从 `True` 变 `False`（第 33 节）。诊断代码已删除、最终 DLL 已部署冒烟。**验证边界**：`entityNull=False` 确证 `TargetPosition` 已写入并切 `MoveCommand`，但波次切换快、且点击后约 136ms 有 per-scene 波次推进覆盖目标，未能做同波次位移定量对比；GM 无法注入真实 `Input.GetMouseButtonDown`。附带发现（未修）：64 个战场波次推进的 `obj_move(get_leader_id(),...)` 同样会命中过期 id，靠 `wait(4000)` 兜底。原"短暂脱离 AI + 超时恢复"语义仍未恢复。详见 `lost_civilization_design.md` 第 30 / 33 节。
5. ~~小地图 UI 显示与圆点核对~~：**2026-11-18 核对显示参数**（`texture=r1c5_minimap_prediluvian`、512×512、`enabled=True`、白不透明、`uvRect` 全幅）。**2026-11-19 补动态圆点并实机验证通过**：`MiniMap.cs` 运行时生成圆形 sprite；实机读到玩家点 `RGBA(1,0.9,0.1,1)` 黄、友方 `RGBA(0.2,0.9,0.25,1)` 绿、敌方 `RGBA(0.95,...)` 红，`RawImage` 下 30 个 `Dot_<id>` 随波次增删。像素级截图仍未做。详见 `lost_civilization_design.md` 第 32.2 / 32.6 节。
5b. **点击不移动 / 偶发战斗卡住（待修，归因已修正）**：`01-40~01-56` 会话确有 `wave_spawn_3` 后 5 分钟不结算、主角 `state=2 MoveCommand` 挂起的历史证据；但 **2026-11-19 实机复测未复现**——GM 注入 `move_to` 两次后战斗 4 波全清、`UnloadBattle`、切回 world、再 `open_battle` 完整闭环，无异常。确证的是「点击后主角没动」（`move_to` 消息入队，但主角 `state=1` / `tgtPos=(0,0,0)` / 位置不变）。优先查 `move_to` handler 里 `get_leader_id()` 在 `auto_battle` 上下文是否指向正确实体（`LeaderId` 每轮 open_battle 会重建，实测 1245→1275→1310），以及 AI `AiPursue` 是否覆盖 `TargetPosition`；`$y` 换主角高度、给 `MoveCommand` 加超时（补 29.5 语义）作为附带修。按授权范围本轮只诊断未改。详见 `lost_civilization_design.md` 第 32.3 / 32.6 节。
6. **主场景多玩家同步的剩余项**：跨格无缝换房、断线重连恢复、双客户端多玩家同屏实机验证、3D 时空锚点光柱/半透明残影/头顶文案、探索收益接角色数值与真知解锁。服务端位移/状态同步（第 25 节）与 AOI 过滤（第 26 节）已于 2026-11-18 补上并部署。
7. `selfeffect/targeteffect ... bone can't find` 的 Warn 仍在（fallback 已生效，日志作资源提示保留）。
8. **双客户端同屏实机验证仍未完成**：2026-11-18 单客户端实机已全链通过（登录 → 剧情 → 战斗 → `parallel_state` 回包 → 分段推进），服务端流量与自检均正常，MCP 双通道可用。缺的只是第二个客户端实例：仓内无 standalone 构建，同工程多开 Unity 会争抢 `Temp/UnityLockfile`。需构建一个 player 或使用不同工程副本，再用两个账号登录同一世界格验证 AOI 过滤与状态回放。
9. **排查教训**：Unity 冷启动后 GM client 每 10 秒重连一次，`unity_wait_ready()` 会独占 MCP 服务最长 180 秒。冷启动期间连续调用 `unity_wait_ready` + 立即查 `unity_info` 容易误判为「通道未连出」；应等编译/域重载完成后直接查连接表，不要用长阻塞等待占位。

## 5. 操作纪律（踩过坑，务必遵守）

- **不要在 Play 中改 C# 或 `asset_refresh()`**：会触发域重载、打断 runtime GM 通道并清空运行时引用。改代码一律先 `editor_stop_now()`。
- **服务端改动按四步**：`StopServer → buildall Debug False → copyserverdlls Debug → RunServer`。只复制 `RoomServer.dll` 会漏依赖（曾因 `GameObjects.dll` 未更新导致 `TypeLoadException`）；服务在跑时 DLL 被占用无法覆盖。
- **story 工程有两套**：`*_dotnet_framework.csproj`（net472，独立 sln，`buildall` 不构建）与 `*.csproj`（netstandard2.1）。共用 `obj/`，必须**先 framework 后 core**，各自 `/t:Restore`；framework 产物要复制回仓库根目录，GameDemo 的 `StoryScript.dll` 要同步到 CSharpGameFramework 的 `Unity3d/Assets/Plugins/`。
- **RunServer 后第一次 Play 登录可能卡在 `do_login`**（无错误日志），重启一次 Play 重连即恢复。
- 表改动要跑 `TableReaderGenerator` 两端同步；DSL 改动要跑 `DslCopy` 两端同步；`WorldEvent.txt` 是 GBK 标准表，不要用 UTF-8 读写。

## 6. 常用工具与文件

| 用途 | 位置 / 命令 |
|---|---|
| 64 个叙事 story 生成 | `Resource/MapDesign/build_story.py` |
| 事件表 / 客户端提示生成 | `Resource/MapDesign/build_events.py` |
| AI 参数调整 | `Resource/MapDesign/tune_combat.py` |
| 改名工具 | `Tools/RenameStoryApi/rename_api.py`（scan/apply/undo/verify） |
| 改名映射/清单 | `BuildLog/rename_map.tsv`（幂等重跑后会清空）、`rename_registry.tsv`、`rename_review.tsv` |
| 运行时诊断 | `combat_probe()`、`camera_probe()`、`anim_runtime_probe()`、`explore_probe("auto")`、`map_verify()` |
| 主场景同步 | `App/ServerModule/RoomServer/World/WorldSyncSystem.cs`、`ServerEnv/bin/Data/world_sync_selftest.txt` |
| 服务端自检/流量/收益 | `App/ServerModule/ServerEnv/bin/Data/explore_selftest.txt`、`explore_traffic.txt`、`rewards_state.txt` |
| 设计总文档 | `Doc/lost_civilization_design.md` |

## 7. 最近一次实机基线

```
改名后：LoginDlg → NicknameDlg → StoryDlg ×15
       unknown func=0  CreateTriger failed=0  NullReference=0  error CS=0
改名后 DLL 抽查：camera_follow / get_position_x / send_gfx_message 存在，旧名不存在
改名后构建：framework×2 exit=0、buildall exit=0（Client/Server.sln 0 错误）、
           GameDemo core exit=0、copyserverdlls exit=0
战斗基线（改名前一节）：主角 anim=Attack moving=False，lockedTarget 与 target 一致，
                       怪/队友坐标分散，overheadBars=entities
```
