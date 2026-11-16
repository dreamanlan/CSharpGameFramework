# 会话交接摘要（压缩状态，2026-11-16）

用途：本会话已很长，后续继续游戏开发时先读这份，避免重复翻历史。完整过程见 `Doc/lost_civilization_design.md`（第 1~23 节）；改名经验见 `Doc/api_rename_sop.md`。

## 1. 项目与当前阶段

- 主仓 `d:\GitHub\CSharpGameFramework`（含服务端 + Unity 工程）；旁仓 `d:\GitHub\GameDemo`（同源框架快照，独立 Unity demo，非编译期源码依赖）。
- 玩法项目「失落的文明」：8×8=64 格藏宝图、时代层战场、服务端权威探索事件表、累积收益。
- 阶段 1（64 格场景/NavMesh/表/轮换）与阶段 2 主体（探索闭环、技能、世界格）已完成；当前在**打磨战斗观感与工程规范**。

## 2. 当前环境状态（可直接接着干）

- 全套服务在跑：`ServerCenter×2 / node / Lobby / DataCache / UserServer / RoomServer`，`EXPLORE_SELFTEST RESULT=PASS`。
- Unity：非 Play，`asset_refresh` 后编译干净（`error CS=0`）。
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

## 4. 未完成 / 下一步（按建议优先级）

1. **改名后进战斗的 skill trigger 实机覆盖**：上一轮只跑到登录/对话链路（`unknown func=0`），未抓到战斗场景加载，触发器名在配置/二进制侧的引用还没实机验证。
2. **带区域推进的分段移动**：当前仍是队伍周围近身遭遇（环形半径 14）。方案已定：每波短距离（≤25，小于怪 `view=35`）+ 怪区域 leash 半径，绝不把双方拉开几百单位。设计见 20.6 / 22.4。
3. **朝向定量验证**：`SetWantedFaceDir → UpdateSpatial.RotateTowards` 已接入，但探针未输出角度。
4. **队友 Gohome 偏活跃**、追击阶段仍显示 `Run`（正常但观感可再调 `GohomeHandler` 容差）。
5. 小地图 UI 像素级/截图核对（消息与资源已确认存在）。
6. 3D 时空锚点光柱/残影、AOI、双客户端多玩家同屏、探索收益接角色数值与真知解锁。
7. `selfeffect/targeteffect ... bone can't find` 的 Warn 仍在（fallback 已生效，日志作资源提示保留）。

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
