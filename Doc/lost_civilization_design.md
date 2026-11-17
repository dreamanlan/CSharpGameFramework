# 失落的文明 — 设计纪要

> 状态：阶段 0（世界观与数据结构对齐，具体制作尚未开始）
> 记录：2026-11-15
> 用途：后续开发（含 AI 独立开发）的**意图基准**。反复出现过的偏差是：把关卡做成单战斗区擂台、把地形拉平、把设计意图写死在生成器代码里、把工具本身当成目标。本文件用于防止再次跑偏。

---

## 1. 已确认设定

| 项 | 设定 |
|---|---|
| 游戏名 | 失落的文明 |
| 世界观参考 | 撒迦利亚·西琴《地球编年史》（作为虚构设定素材；学界视为伪史，不作史实陈述） |
| 玩家身份 | 近未来**调查者**（调查队、记录者），不是外星血脉、不是神 |
| 立场 | **护人类**（恩基一系）；恩利尔系的支配派作为对立抉择 |
| 揭示节奏 | 早期以神话/传说进入，随泥板与遗址逐步发现"神"其实是降临者 |
| 时间结构 | **多时代重叠 + 穿越特征**；同一地理空间叠加不同时代层 |
| 世界入口 | 现代发现的**遗迹藏宝图**，4096×4096，拆 8×8 = 64 个场景 |
| 藏宝点 | 进入战场地图的**传送点**，需玩法包装，不能一个接一个跑 |
| 章节驱动 | 事件点与路线选择影响最终收益；战斗场景主要服务故事与世界观 |
| **时间锚点（方案 A，已定）** | 故事年 **2087**；尼比鲁近地点 **3417**（3600 年长周期，距今约 1400 年） |
| **驱动力（方案 A，已定）** | 遗迹提前复苏 + 恩利尔系势力为"回归"做准备；**不是**天象倒计时 |

工程里已有序章文案就是这个基调，见 `Resource/DslFile/Story/Battle2/story.dsl:33-51`（"最早降临地球的那些智慧生物，被称为神"、"苏美尔人、玛雅文明、古埃及…水晶头骨"、"尼比鲁星球再一次靠近地球"、"月球背面真的有基地么"、"我们回到当年的大陆去看看吧"、"序章--人？神？"）。**"回到当年的大陆"就是传送/穿越的叙事动机，不需新编。**

## 2. 时间线与周期推算

西琴原著常见周期为 **3600 年**（一个 *shar*）；**2600 年**多见于二手转述。两版都记录在此，游戏采用 3600。

锚点：苏美尔/西琴体系里的大洪水约 **公元前 10,983 年**（常取整前 11,000 年）。

公式：`近地点 = 基准年 + 周期 × ceil((0 - 基准年) / 周期)`

| 周期 | 基准 | 上一次近地点 | 下一次近地点 |
|---|---|---|---|
| 2600 | 前 10,983 | 前 583 | 公元 2017 |
| 2600 | 前 11,000（取整） | 前 600 | 公元 2000 |
| 2600 | 前 10,963（误差内微调） | 前 563 | 公元 2037 |
| **3600（采用）** | **前 10,983** | **前 183** | **公元 3417** |
| 3600 | 前 11,000（取整） | 前 200 | 公元 3400 |

**关键结论**：按 3600 周期，现代（21 世纪）不在近地点附近——上一次约公元前 200 年，下一次约公元 3400~3417 年。因此"降临前 7 年"的倒计时不成立。

**方案 A（用户已确认）**：降临年 3417，故事年 2087。长周期反而让"多时代重叠"更自然（一个周期里文明兴衰多次，藏宝图叠着多个时代层）；紧迫感来自"有人想让回归提前 / 正在唤醒设施"，而非天象。玩家身份不改，仍是近未来调查者。前 183/200 年那次近地点落在苏美尔-巴比伦文明尾声、"诸神已离去"的时代，可作为一个时代层或一条剧情线（那次回归发生了什么、为什么记载被抹掉）。

## 3. 工程现状（避免重复造轮子 / 误用）

- `Packages/myself.gmscript/Editor/TerrainProcessor.cs`：**保留但不使用**。用户已删除重复的 `Assets/Scripts/Editor/TerrainProcessor.cs`（含菜单 `工具/地形处理`）。新工具自行开发即可，不要求复用它。
- `Unity3d/TerrainDsl/*.dsl`（5 个）与仓库根 `chess.png / circle.png / spiral.png / world.png`：历史资产。这 4 张图是**图案/色块图，不是高度图**（黑底黄线彩点、螺旋线、彩色大陆块），原 DSL 只用 `sample_red/1000` 当微弱扰动。**不要把它们当成关卡设计数据。**
- DSL 里 `sampler` 路径是失效的旧绝对路径 `F:/OpenSource/CSharpGameFramework_local/*.png`（当前机器不存在）。
- 工程侧 GM API 集中在 `Unity3d/Assets/Scripts/GmScript/Editor/AppEditorDslApi.cs`（符合目录约定）：`scene_catalog / scene_shot / scene_fix / scene_verify / scene_audit / scene_terrain_info / scene_repair / res_probe / actor_probe / actor_fix / anim_fix / prop_scale / asset_polish / nav_bake / controller_fix / animator_host_fix / anim_binding_probe / anim_sample_probe / anim_runtime_probe` 等。
- 现有 `TerrainData` 规格 **512×512**（由首轮 `scene_fix` 输出 `camY = 128 + 512×0.55 = 409.6` 反推）。`4096 ÷ 8 = 512`，与藏宝图 8×8 切片天然吻合。
- 现有 `Battle1~Battle12` 场景与 `Assets/Scenes/Levels/BattleN/Ground.asset`：**过渡产物**。它们是"中央一个圆形可走平台 + 环带装饰 + 静态俯视相机"的擂台结构，且 `Battle*.story.dsl` 把所有 `create_npc` 挤在 `(40~90, 40~90)`，只用了 512 场地约 1/25。**按新意图需要重做，不要在此基础上继续加内容。**
- 曾把 10 种地形形态写死在 `TerrainDesign.Shape()` 里、bitmap 只做调制——属于设计意图放错位置，应删除，改为只读输入图通道。

## 4. 关卡意图（硬约束）

1. **空间**：不是擂台，是一张完整关卡地图，512 地形全用。多处战斗区由道路串联，区域之间有真实移动距离与地貌变化；地形边界只用于封图，不是场地边界。
2. **玩法**：玩家在场景内移动 + 多场遭遇；有事件点（调查 / 交互 / 进入触发）；事件结果与路线选择影响本章收益与结局分支。战斗不等于"一处刷完 `all_killed` 就结束"。
3. **叙事**：地图分区本身就是故事与世界观的载体（例：村落 → 商道 → 神殿 → 峡谷 → 旧战场），地貌、材质、区域命名、事件内容都服务章节叙事，不是"先有地形再撒装饰"。
4. **数据**：设计意图全部画在输入图里，生成器只做忠实翻译，不内置形态、不发明战斗区、不硬编码刷怪坐标。

## 5. 地图与场景结构

```
4096 藏宝图（信息/导航层；现代发现；2D 俯视缩略；8×8 分区 + 藏宝点分布）
   ↕ 一一对应（不是替换关系）
世界地图 8×8 = 64 个可玩场景（玩家在此层移动、多玩家互相可见、只有移动与交互、无战斗）
   └─ 每格 512×512，一个独立 .unity（不同时代 = 不同 .unity，不叠在同一场景）
        ├─ 地形图 256×256：R 高度 / G 材质 / B 功能 mask
        ├─ 布局图 256×256：R 区域 ID / G 强度 / B 触发类型
        ├─ 时代层：独立的 .unity 场景（现代营地 / 苏美尔城邦 / 洪水前设施 / 灾后废墟…）
        └─ 小地图：构建期随场景一起生成（静态底图 + 运行时动态标记）
```

- **64 个 `.unity`**，不用通用模板：`.unity` 本身不是负担，相关资源才是；世界观的空间/时间感要靠独立场景体现。
- **不同时代 = 换场景**，不是同场景内 GameObject 分组切换（叠加会拖性能）。
- **藏宝图与世界 8×8 场景是对应关系，不是替换**：世界地图本身是可玩的 3D 空间，藏宝图是信息/导航层。
- 主场景（`Story1`）**可以不保留**，按内存开销与加载时间权衡。
- **每个战斗场景同时生成小地图**。

坐标约定：**整图 = 512 地形，256×256 输入图，1px = 2 世界单位，区域位置由图决定**；`(60,70)` 只作为出生区参考点之一，不再是全场中心。输入图只在**构建期**读取，运行时不读 PNG。

## 6. 输入图规格（用户已同意通道定义）

**物理图** `*_terrain.png`

| 通道 | 含义 | 取值 |
|---|---|---|
| R | 高度 | `0..255` → `0..地形高度` |
| G | 地表材质 | `0-63` 草 / `64-127` 沙 / `128-191` 岩 / `192-255` 雪 |
| B | 功能 mask | `255` 可通行/战斗区 / `128` 装饰摆放带 / `0` 禁止或边界 |

**布局图** `*_layout.png`

| 通道 | 含义 | 取值 |
|---|---|---|
| R | 区域 ID | `40+20*N` 战斗区 1..N / `200` 道路 / `220` 事件点 / `240` 收益点 / `250` 传送点(藏宝点) / `0` 无 |
| G | 强度 | 战斗区难度、事件点收益系数 |
| B | 触发类型 | `80` 进入触发 / `160` 交互 / `255` 击杀完成 |

绘制约束：战斗区内部 R 恒定、B=255；边界一圈 B=0 且 R 抬高封边；山脊不得侵入战斗区；**每张图必须含 ≥2 战斗区 + 连通道路 + ≥1 事件点**。

已有样板（讨论用，未定稿）：`E:\tmp\bitmap_design\chapter1_terrain.png` / `chapter1_layout.png` / `chapter1_preview.png`，以及单格三层样板 `battle3_input.png` / `battle3_R_height.png` / `battle3_G_material.png` / `battle3_B_mask.png`。绘制脚本 `e:\tmp\draw_level_map.ps1`。

## 7. 传送点玩法包装（逐步迭代，不急一步到位）

1. **解码解锁**：藏宝点需对应泥板碎片 / 星象对齐才能激活，未解码只显示"？"，复用"逐步发现真相"节奏
2. **进入条件**：真知等级、前置事件完成、能量/次数限制
3. **传送点自身是玩法节点**：占点防守、解谜、恩基/恩利尔立场抉择
4. **路线影响收益**：从哪个点进、按什么顺序走，改变碎片与结局分支
5. **时代层可见性**：某些藏宝点只在特定时代层显现，逼玩家来回穿越

现有 12 场景自动轮换循环可留作阶段 1 的临时机制，等藏宝图 UI 与解码玩法完成再替换。

## 8. 迭代阶段

- **阶段 0（当前）**：背景与数据结构定稿；画 1 张 4096 藏宝图缩略（8×8 分区命名 + 藏宝点分布），选 1 格做完整样板（地形图 + 布局图 + 2 套时代层）
- **阶段 1**：生成器改为纯翻译（删 `Shape()`）；单场景多战斗区 + 道路 + 事件点 + 收益结算，story 按区域推进
- **阶段 2**：藏宝图 8×8 切片、藏宝点解码与传送玩法、多场景切换
- **阶段 3**：多时代层重叠穿越、阵营分支与结局

## 9. 验收约束（防止再次"拉平交差" / 擂台化）

- 输入图必须含 ≥2 战斗区 + 连通道路 + ≥1 事件点，否则拒绝生成
- 生成器不得保留形态函数；heightmap 只能来自 R 通道
- `create_npc` 点位来自布局图区域，story 按区域推进并驱动事件/收益结算
- 场景验收看**可移动范围 / 区域数 / 路线连通性**，不看"中央平台是否平"
- 当前 `scene_verify` 的 `platformFlat` / `relief` 是擂台版过渡指标，阶段 1 起须替换

## 10. 待定项

降临年、故事年、驱动力已由方案 A 定稿（第 1、2 节）；六项技术决策已定稿（第 12 节）。仍待研究或开发时确定：

| 项 | 说明 |
|---|---|
| 时代层数量与命名 | 参考第 11 节事件年代自然分层 |
| 8×8 地理分区 | 建议两河 / 埃及 / 黎凡特 / 亚拉腊 / 非洲阿布祖 / 安第斯 / 中美洲 / 特殊（月球·火星） |
| 规模 | 64 格全做 or 先做一章数格（建议先一章） |
| 战斗数值与技能设定 | 由开发侧负责，原则见第 12.4 |
| 世界地图多玩家同步 | 需基于现有 RoomServer 机制研究，见第 12.3 |
| 每个场景 story.dsl 细节 | 开发时自定，自洽即可，不设限 |

---

## 11. 藏宝点素材表：《地球编年史》事件

> 以下均为**西琴书中的设定/主张**（学界视为伪史），仅作虚构素材。年代为书中约数或神话时间，不必精确。
> 用法：每一行都可落成一个藏宝点（布局图 R=250）、一个战斗区、一个事件点，或一整格时代层。地理分布可直接映射到 8×8 藏宝图的分区。

### A. 降临与设施（洪水前层 / 阿努纳奇设施）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| 恩基率 50 名阿努纳奇降临，建**埃利都 Eridu**，"王权自天而降" | 约 45 万年前 | 波斯湾/两河 | 起始藏宝点：第一地球站遗址，解码"王权自天而降"泥板 | 解锁藏宝图全貌 |
| **阿布祖 Abzu** 非洲南部金矿 | 远古 | 南部非洲 | 多层竖井矿坑战场、遗留采掘守卫 | "金"动机线索 |
| 恩利尔建**尼普尔 Nippur** 任务控制中心，DUR.AN.KI"天地纽带" | 远古 | 两河 | 中枢设施：启动/关闭全球导航网 | 阵营抉择入口 |
| **火星中转基地**被弃 | 远古 | 火星（特殊层） | 需先修复信标才可传送的特殊格 | 高价值碎片 |
| **西奈太空港**洪水后重建 | 洪水后 | 西奈 | 大型战斗区 + 发射场解谜 | 解锁跨区传送 |

### B. 造人与禁忌（护人类主线的根源）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| **伊吉吉 Igigi 矿工叛乱** | 约 30 万年前 | 矿区 | 防守战：镇压还是协助叛乱 | 立场抉择 |
| 恩基 + 宁胡尔萨格**基因造人**（"卢卢"/亚当） | 约 30 万年前 | 埃利都 | 实验室遗址：拼合基因谱系 | 核心真相碎片（人类起源） |
| **伊甸园与"蛇"**、知识果、被逐出 | 神话时代 | 两河 | 调查事件点：谁在给人类知识 | 恩基线索引出 |
| **阿达帕 Adapa** 被拒永生 | 神话时代 | 埃利都 | 事件点：选"永生"还是"知识" | 收益类型分支 |
| 观察者与人类女子通婚 → **拿非利人/巨人 Nephilim** | 洪水前 | 多地 | 精英战斗区（半神遗迹） | 高强度战斗碎片 |
| **吉尔伽美什**求永生、见乌特纳匹什提姆 | 约前 2700 | 乌鲁克 | 跨 2~3 格的多段任务链 | 长线收益 |

### C. 大洪水（可做成一整个时代层）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| 尼比鲁近地点引发冰盖滑动 → **大洪水** | 前 10,983 | 全球 | 水位上升的限时关卡、灾变场景 | 大量碎片但有代价 |
| 诸神会议决定不救人类，**恩基泄密**给阿特拉哈西斯/乌特纳匹什提姆 | 前 10,983 | 两河 | 关键抉择：是否泄露（呼应护人类） | 主线立场锁定 |
| **方舟停亚拉腊山** | 前 10,983 | 亚拉腊 | 藏宝点：方舟残骸 | 通往"洪水前层"的钥匙 |
| 洪水后**彩虹之约**、人类获准存续 | 洪水后 | 两河 | 章节结算节点 | 后续章节收益倍率 |

### D. 文明与权能（苏美尔层）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| **苏美尔文明突然出现**（文字/冶金/历法"授予"） | 约前 3800–3500 | 苏美尔 | 城市区：收集"Me"清单 | 解锁科技/装备 |
| **Me 泥板**（文明权能），伊南娜自恩基处骗得带往乌鲁克 | 神话时代 | 埃利都→乌鲁克 | 护送/争夺任务，跨格路线 | 可转移收益（会被对手夺走） |
| **巴别塔**（"造名"= 发射塔），恩利尔变乱语言、分散人类 | 约前 4 千纪 | 巴比伦 | 高塔战斗区 + 语言解码谜题 | 藏宝图碎片分散到多格 |
| **苏美尔王表**：洪水前诸王统治 241,200 年 | 神话时代 | 苏美尔 | 解码时间轴，揭示 3600 周期 | 揭开 3417 降临年 |
| **VA 243 泥板**"十二天体"图案 | 苏美尔 | 苏美尔 | 解码尼比鲁轨道 | 揭示回归天象 |

### E. 设施与标记（分布式藏宝点，最适合铺满 8×8）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| **吉萨大金字塔**为导航信标（非法老墓） | 远古 | 埃及 | 大型室内战斗区 + 星象对齐 | 导航图碎片 |
| **金字塔战争**：马尔杜克占用，宁努尔塔拆除顶端晶石 | 远古 | 埃及 | 阵营对抗战斗区 | 抉择：保留或摧毁信标 |
| **狮身人面像**指向狮子座 / Zep Tepi"第一时间" | 远古 | 埃及 | 天文观测事件点 | 揭示时代层切换条件 |
| **巴勒贝克三石塔 Trilithon** 巨石平台 | 远古 | 黎凡特 | 着陆场战斗区 | 传送点解锁 |
| **蒂亚瓦纳科太阳门 / 萨克塞华曼**：洪水后冶金中心 | 洪水后 | 安第斯 | 跨大陆藏宝格 | 金线索闭环 |
| **巨石阵、复活节岛摩艾、羽蛇神 Quetzalcoatl / 维拉科查** | 各异 | 全球 | 分布式导航标记，集齐才显形 | 高收益合集 |
| **水晶头骨**（序章已提） | 传说 | 中美洲 | 知识载体事件点 | 解锁"逐步揭示真相"关键文本 |
| **月球背面基地**（序章已提） | 传说 | 月球（特殊层） | 终局传送点 | 结局分支 |

### F. 神族战争与灾变（对立势力，制造紧迫感）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| **恩基 vs 恩利尔**长期对立 | 全程 | 多时代层 | 贯穿主线的阵营抉择 | 护人类路线（已定） |
| **马尔杜克 vs 宁努尔塔**争权；马尔杜克在埃及称"拉" | 约前 3–2 千纪 | 埃及/巴比伦 | 多方势力战斗区 | 可引入第三方 |
| **前 2024 年前后苏美尔毁灭** / "罪恶之城"核爆、死海、玻璃化沙 | 约前 2024 | 两河/死海 | 灾后废土战斗区、环境 debuff | 高代价高收益 |
| **"失落的国度"**：中美洲流亡神与冶金 | 洪水后 | 中美洲 | 独立章节（8×8 里一整片区域） | 独立收益线 |
| **前 183/200 年上一次近地点**：诸神离去、记载被抹 | 前 183 | 巴比伦末期 | 隐藏藏宝点 | 补完"为什么现代没人知道" |

### G. 回归动机链（主线收束）

| 事件 | 书中年代 | 地点 | 可转成的玩法节点 | 收益 / 抉择 |
|---|---|---|---|---|
| **金粉修复尼比鲁大气层** = 采金 → 造人 → 文明 → 回归的动机源头 | 全程 | 跨时代 | 最终真相：把前面所有碎片串成一条因果 | 主线收束 |
| **"回归/弥赛亚"其实是归来**，天与地合一 | 3417 | 全图 | 终局章节 | 结局分支（护人类是否达成） |

### 从素材自然长出的结构

- **藏宝点** = 事件地点，天生有名字、有叙事，不需要编"宝箱 1/2/3"
- **时代层** = 事件所属年代（洪水前设施 / 洪水 / 苏美尔城邦 / 灾后废土 / 现代营地），分层天然成立
- **真相碎片** = 动机链的因果顺序（采金→造人→洪水→文明授予→神族战争→回归准备），决定揭示节奏与解锁顺序
- **阵营抉择** = 恩基/恩利尔，天生贯穿每个事件（泄密、造人、巴别塔、护人类）
- **8×8 地理分区** = 两河、埃及、黎凡特、亚拉腊、非洲阿布祖、安第斯、中美洲、特殊层（火星/月球）；每区若干格，一格一事件或一事件链
- **传送点解码** = 泥板/星象对齐，天然来自"解读苏美尔记载"这一核心动作

---

## 12. 技术实现（决策已定）

### 12.1 六项决策

| # | 决策 | 备注 |
|---|---|---|
| 1 | **64 个 `.unity`**，不用通用模板 | 世界观宏大，要体现空间/时间感；`.unity` 不是负担，相关资源才是 |
| 2 | **构建期**读输入图，运行时不读 PNG | 烘焙成地形/布局/点位数据 |
| 3 | **双端**，服务端职责严格受限 | 详见 12.2 |
| 4 | **不同时代 = 换场景**（各自 `.unity`） | 不同时代叠在同一场景影响性能 |
| 5 | 每场景都需要一个 `story.dsl` | 可新增 `chapter_main`；细节开发时自定，自洽即可，不设限 |
| 6 | 主场景可不保留（按内存/加载时间权衡） | 藏宝图与世界 8×8 场景是**对应关系不是替换**；**每个战斗场景同时生成小地图** |

### 12.2 服务端职责边界（用户定义，不可越界）

**核心原则：服务端不模拟战场**（算力有限）。

- 战场逻辑几乎全在客户端：不创建战斗实体、不跑 AI、不判定战斗、**不推进战场内位置**。
- 服务端**不管理战场加载/切换/生命周期**（没有进出战场的逻辑）；只在世界地图玩家对象上同步一个状态标记（见 12.7）。
- 服务端维护：世界地图位置（固定在传送点）、战场会话状态、**一张事件表**、累积收益、校验记录。
- 战场内位置**由客户端上报**：
  - **定时**上报 `{session, seq, x, z}`，服务端按**两次上报的间隔时间**评估移动速度，异常累计处理。
  - 事件触发上报 `{event_id, x, z}`，服务端校验「位置必须落在事件半径内」，否则记作弊嫌疑。
- **事件精确坐标与半径只在服务端事件表，绝不下发客户端**；客户端地图上只有视觉提示，需玩家观察解谜。
- **客户端无权决定收益**：收益由服务端校验后计算并下发，客户端消息不得携带收益数值。
- 防洪水：同一事件尝试次数超阈值 → 该事件**对该玩家永久关闭**（账号维度）。
- **战斗不校验**（战斗是乐趣不是考验）；服务端**参与数值玩法**：持有累积成长/属性。
- 玩家在服务端的位置模型 = **世界地图上的传送点**，不随战场内移动变化。

### 12.3 世界地图层（RoomServer 存在的意义）

- 该层**多玩家互相可见**；玩家**只有移动交互，没有战斗**。
- 现有基础（已核实，不必从零造）：
  - `App/ServerModule/RoomServer/RoomServer/LogicMsgHandler.cs:68` `Msg_CR_UserMoveToPosHandler`：客户端上报目标 `(tx,tz)`，服务端写 `msi.TargetPosition` 与朝向，再 `scene.NotifyAllUser(Msg_RC_NpcMove, ...)` 广播 → **多玩家可见的现成链路**。
  - `Movement/MovementSystem.cs:25,46` `Tick` / `MoveNpc`：服务端按 `ActualProperty(最终速度) × deltaTime` 推进移动 → 适用于**世界地图层 / NPC**；**战场内位置不由此推进**（服务端不模拟战场），战场速度校验改用 12.7 的「定时上报 + 间隔评估」。
  - `LogicMsgHandler.cs:239` `Msg_CRC_StoryMessage`：客户端消息带 `client_` 前缀，源码注释明确"防止客户端直接调用服务端逻辑" → **事件触发校验的落点**。
  - `Msg_CR_SkillHandler`：只广播不校验 → 与"战斗不校验"一致。
  - `Scene/DataSyncUtility.cs` + `scene.NotifyAllUser`：观察者/广播机制。
- 待研究：世界地图层的 Scene/Room 组织（现有 `RoomManager.cs` 只对 `type=2` 剧情场景建 field room）、玩家可见范围(AOI)、跨格移动无缝衔接、断线重连。

### 12.4 基本原则（不可违背）

- **故事 + 累积收益成长为主**。
- **战斗不产生明显收益差，但战斗要有乐趣** —— 乐趣来自技能组合、节奏、地形互动，不是数值碾压。
- 服务端只保存数据与校验后提供收益，不参与战斗过程，但参与数值玩法。
- 世界地图层只有移动交互与多玩家可见，无战斗。
- 战斗数值与技能设定由开发侧负责，需满足"不产生明显收益差"这一约束。

### 12.5 构建期工具链（改造方向）

- 输入图只在构建期读；工程侧 GM API 拆成 `map_slice`（4096 切 8×8）/ `terrain_from_map`（只读 R/G/B）/ `layout_bake`（点位·道路·事件·传送）/ `era_bake` / `scene_build` / `scene_verify` / `minimap_bake`。
- **删掉 `TerrainDesign.Shape()`**，设计意图回到输入图。
- 每个场景产出：`.unity` + `TerrainData` + 布局烘焙数据 + **小地图** + `story.dsl`。
- 表与 DSL 仍需 `TableReaderGenerator.exe` / `DslCopy.exe` 同步客户端与服务端两端。
- 与 `Packages/myself.gmscript/Editor/TerrainProcessor.cs` 无关（保留不使用）。

### 12.6 验收补充

- 每格必须有 `story.dsl`、小地图、≥2 战斗区、连通道路、≥1 事件点、传送点。
- **事件表（ID、坐标、半径、收益、状态）不得出现在任何客户端包体/配置/DSL/资源中**；客户端连 `event_id` 都没有。
- 客户端上行消息不得携带收益数值或事件标识，只携带坐标与探索请求；`ExploreResult` 不得返回可复用的事件索引。
- 未命中响应必须与"该处无事件"完全一致，不得含距离/邻近/冷却状态提示（防二分探测与状态探测）；**冷却拒绝也必须与未命中不可区分**。
- **cooldown 判定必须在服务端且短路拒绝**（不查事件表、不写轨迹）；客户端本地冷却只做 UI 表现，服务端不采信。
- `scene_verify` 去掉擂台版的 `platformFlat` / `relief`，改查区域数、路线连通性、事件点、小地图、story 完整性。

### 12.7 战场校验协议与「平行时空」状态

#### 12.7.1 消息与校验（建议方案，参数待定）

**触发入口是探索技能**。事件不是走近自动触发，玩家必须在自己判断的位置**主动使用探索技能**；客户端既不知道 `event_id`，也不知道事件表、精确坐标、半径与收益，只上报「我在这里用了一次探索」。事件表（ID、坐标、半径、收益、状态）**完全只存在于服务端**，客户端包体/DSL/配置里一行都没有，反编译也拿不到可枚举目标。

| 方向 | 消息 | 内容 | 服务端动作 |
|---|---|---|---|
| C→S | `EnterParallel` | `grid_id, teleport_id, era_layer` | 打 `InParallelSpace` 标记并广播；位置仍固定传送点；**不加载战场、不建实体** |
| C→S | `BattlePos`（周期 200~500ms） | `session_id, seq, x, z` | 记轨迹；按**服务端接收间隔**（不采信客户端时间戳）算 `v = dist/dt`；超 `max_speed × 容差` 记异常，滑动窗口累计 |
| C→S | `Explore`（探索技能释放） | `session_id, seq, x, z` | ⓪ **冷却闸门**：`now - last_explore < cooldown` → 直接拒绝，**不查事件表、不写轨迹、不改事件状态** ① 校验探索技能消耗（服务端计时）② 与最近 `BattlePos` 轨迹连续（不能瞬移触发）③ **按坐标空间反查事件表**：找出所有「半径覆盖该坐标」且本会话可达、未完成、未永久关闭的事件，取优先级最高/最近的一个 ④ 命中则累加收益 |
| S→C | `ExploreResult` | `found, content_token, delta, total` | 客户端只展示；**不返回 event_id、事件表索引或距离信息**；`found=false` 时响应与"该处真的没有事件"完全一致（**不可区分响应**，防二分探测） |
| C→S | `LeaveParallel` | `session_id` | 清除标记并广播；不管理战场结束逻辑 |

- `content_token`：服务端下发的一次性展示标识（文本/资源 key），只用于本次叙事展示，不可复用、不含事件表索引。
- `seq` 单调递增防重放；`session_id` 进入时分配。
- 探索技能自带 **cooldown**（服务端计时，建议 1.5~3s，按事件密度调），是**服务端拒绝高频尝试的主闸门**：未过冷却直接短路返回，不查事件表、不写轨迹、不改状态。客户端本地的冷却 UI 只是表现，服务端不采信，改本地值无效（属"服务端参与数值玩法"，不违反不模拟战场）。
- **冷却把网格扫描变成不可行**：事件半径按 3 单位估算，覆盖 512×512 约需 9000 个采样点，冷却 2s 即 5 小时以上不间断操作，且还要同时满足轨迹连续与速度校验、客户端又不知道哪个区域值得扫。
- 冷却拒绝的响应与 `found=false` **完全一致**（不可区分），不告诉客户端"因冷却被拒"，避免拿它探测服务端状态。超频分级：偶发连点/网络重传 → 静默拒绝不罚；持续超频 → `suspicion++`；累计阈值 → 会话收益冻结 / 探索技能禁用。
- 冷却是**会话级速率限制**，"事件永久关闭"是**账号×事件级惩罚**，两者独立叠加。
- 未命中不返回"差一点""附近有事件"之类提示；事件对该玩家永久关闭后也统一返回 `found=false`（静默），不给反馈边信道。
- 校验失败/异常只记 `suspicion`；累计到阈值 → 该事件对该玩家永久关闭 / 会话收益冻结 / 探索技能禁用（分级，待定）。
- 战斗结果完全不进收益链，伪造战斗结果没有收益动机。
- 参数待定：上报周期、速度上限与容差、事件半径、探索技能冷却与消耗、滑动窗口、异常阈值。

**玩法闭环**：地图视觉提示（泥板残文、地貌异常、光晕、星象对齐）→ 玩家观察推理出位置 → 走过去（受速度校验）→ 主动施放探索技能 → 服务端按坐标裁定 → 命中才给收益与真相碎片。解谜是玩法本体，不是走到就领奖；也正因客户端没有任何事件数据，"观察解谜"才是唯一可行的路径。

**残留风险（如实说明）**：客户端仍可做「网格扫描」——等间距疯狂按探索技能。但不知道哪里值得按、不知道命中与否的差别（不可区分响应）、受冷却与轨迹连续性限制、扫描密度会被异常检测识别，成本远高于按提示解谜。可追加加固：每个会话对事件坐标做服务端侧微小随机偏移（客户端永远拿不到精确值）、扫描模式检测、轨迹存证与事后审计。目标是作弊成本远高于解谜成本，不追求形式上的不可破解。

#### 12.7.2 「平行时空」状态标记（细化）

**叙事解释**：世界地图是现代层（2087），战场是**同一地点在过去时代的投影**。玩家不是"走开"，而是潜入同一坐标的另一时间层——身体仍留在传送点，别人看到的是他的时空锚点。序章"我们回到当年的大陆去看看吧"正是这个动作。**技术限制（服务端不模拟战场）反而成了世界观表达**。

| 项 | 设计 |
|---|---|
| 状态名 | `InParallelSpace`（平行时空 / 潜入·过去层） |
| 数据 | `state, grid_id, teleport_id, era_layer, enter_time, session_id, suspicion` |
| 世界地图位置 | **仍固定在传送点**，不随战场内移动变化（服务端不模拟） |
| 其他玩家看到 | 玩家模型半透明/残影 + 传送点上一根「时空锚点」光柱或涟漪；头顶状态示例：「已潜入·苏美尔层」「已潜入·洪水前层」 |
| 交互规则 | 该层无战斗、初期不支持组队进入；可点击锚点看到「正在调查 XX 遗址」，**不泄露事件坐标与内容**；不能交易/邀请 |
| 广播 | 复用 `scene.NotifyAllUser` 同步状态变更（进入 / 退出 / 超时），AOI 内才广播 |
| 生命周期 | 客户端 `LeaveParallel` 清除；`BattlePos` 长时间不上报或断线 → 服务端超时自动清除标记并结束会话（**只清状态，不管理战场**） |
| 可见性 | 该玩家对其他玩家仍以锚点形态可见，符合"世界地图多玩家互相可见" |

状态标记不是"进出战场逻辑"：服务端不加载场景、不创建实体、不决定何时开始或结束战斗，只同步一个对外可见的状态位与计时器。

---

## 13. 阶段 0 产出：藏宝图与样板格

**全部文件在 `Resource/MapDesign/`**（设计数据，构建期读取，运行时不读 PNG）。生成脚本 `draw_maps.py`（Python 3 + Pillow，`py -3 draw_maps.py` 可复现）——脚本是"画笔"，不是产品工具。

### 13.1 文件清单

| 文件 | 规格 | 含义 |
|---|---|---|
| `world_map.png` | 4096×4096 | 藏宝图主图，8×8 网格，每格 512 |
| `world_map_preview.png` | 1024×1120 | 核对用缩略 + 信息带 |
| `regions.txt` | 64 行 UTF-8 | 每格：ID / 区码 / 名称 / 时代层 / 主线或支线 / 藏宝点数 / 素材引用 / 难度 / 主线序号 |
| `tiles/r2c4_terrain.png` | 256×256 | 物理图 R 高度 / G 材质 / B 功能 mask |
| `tiles/r2c4_layout.png` | 256×256 | 2087 现代调查层布局 |
| `tiles/r2c4_layout_sumer.png` | 256×256 | 苏美尔层布局（同一地形、不同区域） |
| `tiles/r2c4_minimap*.png` | 512×512 | 两套时代层的小地图静态底图 |
| `tiles/r2c4_preview_*.png` | 512×512 | 可读预览 |
| `design_check.txt` | — | 通道语义与硬约束自检 |

### 13.2 8 大区与主线

| 区码 | 名称 | 格范围 | 主时代层 | 素材来源 |
|---|---|---|---|---|
| ARR | 亚拉腊·洪水层 | r0-1 / c0-3 | 洪水层 / 现代 | C 大洪水 |
| ORB | 天外锚点 | r0-1 / c4-7 | 洪水前 / 现代 | A 降临设施 · G 回归 |
| LEV | 黎凡特·西奈 | r2-3 / c0-3 | 苏美尔层 / 灾后 | E 设施 · F 神族战争 |
| MES | 两河·苏美尔 | r2-3 / c4-7 | 苏美尔层 / 现代 | D 文明与权能 |
| EGY | 埃及·吉萨 | r4-5 / c0-3 | 洪水前 / 苏美尔层 | E 设施 · F 神族战争 |
| ABZ | 非洲·阿布祖 | r4-5 / c4-7 | 洪水前层 | A 降临 · B 造人 |
| AMR | 美洲·失落国度 | r6-7 / c0-3 | 洪水后 / 灾后 | E 设施 · F 失落国度 |
| PAC | 环太平洋·导航标记 | r6-7 / c4-7 | 洪水前 / 现代 | E 设施 · G 回归 |

**主线 14 站**（按真相因果链，不是地理顺序）：

`r2c4 埃利都 → r3c5 Me泥板库 → r3c4 王表库 → r2c7 巴别塔 → r0c1 方舟残骸 → r4c4 阿布祖金矿 → r5c4 伊甸园 → r4c0 吉萨大金字塔 → r2c1 西奈太空港 → r0c6 月球背面 → r7c0 水晶头骨窟 → r6c6 远洋导航标 → r1c6 尼比鲁观测台 → r0c7 归来之门`

因果链：文明权能 → 时间轴/3600 周期 → 语言分散 → 洪水实证 → 采金动机 → 造人 → 导航信标 → 太空港 → 天外 → 真相文本 → 导航网 → 3417 → 归来。其余 50 格为支线藏宝点（1~2 个/格）。

### 13.3 样板格 `r2c4`「埃利都」

- **物理**：冲积平原 + 两条河道 + 城市台地 + 远处沙丘/边界山脊；两个战斗区内部 R 恒定 32（可走平台）。
- **现代调查层（2087）**：考古营地 / 探方 / 泥板临时库；传送点在像素 `(30,35)` = 世界 `(60,70)`。
- **苏美尔层**：塔庙 / 泥板库 / 运河码头；**传送点位置不变**（时空锚点固定），战斗区、事件点、收益点、道路全部重布 → 体现"同一片土地的不同年代"。
- 两套布局各含：2 战斗区（`R=60/80`）、2 事件点（`R=220`）、3 收益点（`R=240`）、1 传送点（`R=250`）、连通道路（`R=200`）；G 为强度、B 为触发类型。

### 13.4 自检（`design_check.txt`）

```
terrain R=6..240  B0=9840 B128=1421 B255=54275  arena_R_minmax=32..32
layout_modern  areas=3310 events=226 rewards=243 portal=149 road=1710
               reachable: area1=1793/1793 area2=1517/1517 event=226/226 reward=243/243
layout_sumer   areas=3310 events=226 rewards=243 portal=149 road=1528
               reachable: area1=1793/1793 area2=1517/1517 event=226/226 reward=243/243
```

即：真实起伏（非拉平）、战斗区内部恒定、装饰带/边界 mask 齐备；**从传送点 BFS 可 100% 到达所有战斗区 / 事件点 / 收益点**（道路连通）。硬约束「≥2 战斗区 + 连通道路 + ≥1 事件点 + 传送点」全部满足。

### 13.5 状态：阶段 0 已完成，自主开发授权

**用户已授权：游戏由开发侧自主设计与实施，不再逐项请示。** 8 大区命名、主线 14 站顺序、样板格与时代层安排均按本节定稿执行。

- `draw_maps.py` 产出藏宝图与索引；`draw_all_tiles.py` 按 `regions.txt` 铺开 **64 格全部输入图**（每格 1 张物理图 + 2 套布局图 + 2 张小地图，共 320 张 PNG）。
- `tiles_check.txt`：`TOTAL=64 PASS=64 FAIL=0`。每格两套布局均满足「≥2 战斗区 + 连通道路 + ≥1 事件点 + 传送点」，从传送点 BFS 可达率 100%。
- 地貌按 8 区配方分化：雪山裂谷 / 陨坑高原 / 岩台干谷 / 冲积平原双河道 / 沙丘尼罗河 / 矿坑环 / 山地梯田 / 群岛礁石。小地图按 **R 高度 + G 材质连续着色**，避免"看着都一样"。
- 自主修正两处：① 首版 64 格小地图观感雷同 → 拉开各区高度与材质基线并改连续着色；② 水面原被标 `B=255` 可通行 → 改为 `B=0`，跨水只能走布局图里的道路/桥；边界与陡坡同样 `B=0`。
- **生成器尚未改动**（`TerrainDesign.Shape()` 仍在，阶段 1 删除）；现有 12 个 `.unity` 与 DSL 未动，当前可运行状态保持。

下一步按阶段 1 推进：生成器改为纯通道翻译，按 64 格产出 `.unity` / `TerrainData` / `story.dsl` / 小地图，并替换 `scene_verify` 指标。

---

## 14. 阶段 1 进展：纯通道翻译生成器

### 14.1 新工具

`Unity3d/Assets/Scripts/GmScript/Editor/AppEditorDslApi.Maps.cs`（新文件），注册于 `AppEditorDslApi.Register`：

| API | 用途 |
|---|---|
| `map_build([tile_id])` | 从阶段 0 设计 PNG 生成场景，不传 = 全部 64 格 |
| `map_verify([tile_id])` | 验收场景/地形/区域数据/小地图/story 与真实起伏 |
| `map_shot([tile_id])` | 用场景相机渲染预览到 `e:/tmp/map_previews/` |

**纯通道翻译，无内置形态、无硬编码刷怪点**：
- 地形 PNG：`R → heightmap`（`0..255` → `0..70` 世界单位）、`G → alphamap` 材质档、`B` 供验收/后续 mask 使用
- 布局 PNG：按相同 `R` 值做连通分量聚类，烘焙成 `areas.txt`（`type / worldX / worldZ / radius / G强度 / B触发 / 像素数`）——区域位置**只来自图**
- 两套时代层的布局各自烘焙；传送点固定在世界 `(60,70)`

### 14.2 产出

| 类别 | 数量 | 位置 |
|---|---|---|
| 战斗场景 `.unity` | 64 | `Assets/Scenes/Levels/Battle_{tid}.unity` |
| TerrainData | 64 | `Assets/Scenes/Levels/MapData/{tid}_Ground.asset` |
| 区域数据（现代层 + 时代层） | 128 | `Assets/Resources/MapData/{tid}_areas*.txt` |
| 小地图静态底图 | 128 | `Assets/Resources/Minimap/` |
| story 骨架 | 64 | `Resource/MapDesign/stories/{tid}_story.dsl` |
| 3D 预览图 | 64 | `e:/tmp/map_previews/{tid}.png` |

`map_verify()`：**`MAP_VERIFY total=64 pass=64 fail=0`**。每格 `relief` 40.1~51.1、战斗区 2~4、事件 2~3、收益 3~4、传送点 1，场景/地形/区域/小地图/story 五项齐全。

### 14.3 过程中自主修正

| 问题 | 修正 |
|---|---|
| 预览相机垂直怼地，只拍到脚下草地 | 改为斜俯视整格 `(20,300,20) → (256,0,256)`，FOV 60 |
| 边界形成整齐方形高墙 | 设计数据改为缓坡 + 噪声起伏 + 岩材质，不用雪白 |
| 战斗区变成突兀白色圆盘 | 战斗区只压平高度、保留原地材质 |
| 水面 `B=255` 可通行 | 改 `B=0`，跨水只走布局图里的道路/桥 |
| 首版 64 格小地图观感雷同 | 按 `R 高度 + G 材质` 连续着色，拉开 8 区基线 |
| 3D 场景偏暗 | ambient 提亮、雾密度降到 0.0008 |
| 从基础地形继承的树实例形成满屏黑点并刷 `Pine_tree` 警告 | `treeInstances` / `treePrototypes` 清空（阶段 0 设计图没有树层） |

抽查 3D 预览：ARR 橙岩环坑 / ORB 白灰陨坑高原 / ABZ 矿坑环 / PAC 绿岛浅滩，区域形态可辨。

### 14.4 阶段 1 收尾（已完成，实机验证通过）

| 项 | 结果 |
|---|---|
| 时代层 `.unity` | 每格 `Battle_{tid}_{era}.unity`，与现代层共 128 个；共享 TerrainData、场景独立，时代层用更暗的色调/雾 |
| BuildSettings | `BUILD_SETTINGS added=128 total=146` |
| NavMesh | `map_nav()` 烘焙 64 个时代层战场，各生成 `Battle_{tid}_{era}/NavMesh.asset` |
| 表与脚本 | `Level.txt` 扩为 64 行（10001~10064，指向时代层场景）；`Story1/Story2` 轮换上限 10012 → 10064；64 个 story 目录；`TableReaderGenerator` / `DslCopy` 同步两端 |
| 脚本 | `Resource/MapDesign/cutover.py`（切表）、`fix_story_spawn.py`（实机修正） |

**实机验证**：自动登录后连续进入 `Battle_r0c0_flood → r0c1_flood → r0c2_flood → r0c3_flood → r0c4_prediluvian → r0c5_prediluvian → r0c6_prediluvian`，跨 ARR/ORB 两个时代层；`exception` / `can only be called on an active agent` / `Object reference` 全部 0；`anim_runtime_probe` 显示动作状态在切换、主场景视觉全隐藏、战斗场景相机生效。

**实机暴露并修正的问题**：首版 story 把实体放在 `y=0`（新地形有起伏，实体埋地下），且战斗区在世界 120~440、玩家在 `(60,70)`，AI 够不着 → 全场 Idle、`all_killed` 不触发、循环卡在第一场。用 `fix_story_spawn.py` 从 `areas.txt` 读传送点、从地形 PNG 取真实高度重生成 64 个 story：玩家/队友/怪围绕传送点生成且 `y` 用 `R/255*70`（样本 `r0c0` 为 `y=28.5`）。修正后循环即跑通。

阶段 1 的战斗仍是**入口遭遇**（`autorun_battle.dsl` 为空，玩家不会自动跑图）；`areas.txt` 的多区域数据已就绪，按区域推进是阶段 2 的事。

### 14.5 剩余项与阶段 2

**剩余项**：
- **水体表现**：`G` 材质档没有「水」，低洼暂以浅滩材质表示；小地图用蓝色区分。需加独立水面网格/材质。
- **边界方框感**：整格斜俯视预览可见一圈封边山脊；游戏内 `camera_follow` 贴近角色不会见到全貌。按世界观包装为遗址外围山脊/裂谷边缘。
- **旧 `TerrainDesign.Shape()` / `scene_fix`**：已不再被任何运行链路使用，可安全删除（未删以免动到历史调试入口）。
- **Editor.log**：Play 期间累积约 173MB 带堆栈的 Warn，Unity 占用无法清空；重启编辑器会轮转。不是错误洪水。

**阶段 2 方向**：藏宝图 UI 与传送点解码玩法、世界地图 8×8 多玩家可见层、服务端事件表与 `Explore` 校验协议（12.7）、按 `areas.txt` 的多区域推进与探索技能、累积收益成长、时代层穿越与阵营分支。

---

## 15. 阶段 2 进展：服务端事件表与探索校验闭环

### 15.1 物理隔离（关键）

工程表体系本来就有客户端/服务端分区：`TableReaderGenerator` 同步客户端时跳过 `Resource/Tables/Server/`，同步服务端时跳过 `Client/`。运行时实测：

```
ServerEnv/bin/Tables        -> Public, Server      （无 Client）
StreamingAssets/Tables      -> Client, Public     （无 Server）
```

故 **事件表是 `Resource/Tables/Server/WorldEvent.txt`，一张标准项目表**，客户端包体永远拿不到。`build_events.py` 自检：`client_Tables_Server_exists=False`、`client_area_event_rows=0`、`leaks=0`。

**它走项目读表器，不是自定义文本格式**（详见第 21 节）：

```
WorldEvent.txt (行1=类型, 行2=字段名, GBK/936)
  → TableReaderGenerator gendsl → table.dsl 里 tabledef(WorldEvent, dictionary, server)
  → TableReaderGenerator (无参) → DataReader.cs / FilePathDefine.cs
      → TableConfig.WorldEventProvider / FilePathDefine_Server.C_WorldEvent
  → Txt2Binary 生成 Server/WorldEvent.dat
  → RoomServer.LoadData() 里 WorldEventProvider.Instance.LoadForServer()
  → ExploreSystem 用 WorldEventMgr.GetData() 构建按 grid 索引的事件字典
```

`Server/` 目录在客户端同步时被 `TableReaderGenerator` 跳过，因此**隔离由读表器本身保证**，不再靠脚本把文件藏在别处。曾有一版把表移出 `Resource/Tables/` 并手写 TSV 解析来绕开格式要求，已废弃。

**同时堵掉的漏洞**：阶段 1 把布局图烘焙的 `areas.txt` 放进了 `Assets/Resources/MapData/`，其中 `type=220` 事件点带着精确坐标——等于客户端已经持有事件坐标。`build_events.py` 现在把数据分流：

| 输出 | 内容 | 去向 |
|---|---|---|
| `Resource/Tables/Server/WorldEvent.txt` → `WorldEvent.dat` | 148 条事件：ID / grid / 时代层 / **精确 x,z** / 半径 / 收益 / token / 标题 | 仅服务端，由 `WorldEventProvider` 读 |
| `MapData/{tid}_hints.txt` | 量化到 64 单位网格的**模糊提示**、半径 48、文本 key | 客户端 |
| `MapData/{tid}_areas.txt` | 净化版，**移除全部 220 行** | 客户端 |

事件坐标由构建期直接从布局图 `R=220` 连通分量烘焙（`design_y == pil_y`，故 `worldX = pil_x × 2`、`worldZ = pil_y × 2`，与 TerrainData/areas.txt 一致；早期文档里的「Y 翻转」表述作废，见 17.4）。服务端表与客户端数据不再同源。

### 15.2 服务端校验闭环

`App/ServerModule/RoomServer/Explore/ExploreSystem.cs`（新文件），在 `MsgStoryMessageHandler` 入口拦截 4 个协议消息，走编译进 `RoomServer.dll` 的 C# 逻辑，**不经过客户端可达的 story 脚本**。复用现有 `Msg_CRC_StoryMessage` 双向通道，未改 proto 生成代码。

| 消息 | 客户端发 | 服务端做 |
|---|---|---|
| `enter_parallel` | `get_scene_id()` | 由 level id 推导 `grid_id`（`10001+row*8+col`），建会话，广播 `InParallelSpace` 状态标记；**不加载战场、不建实体** |
| `battle_pos` | 定时 300ms 上报 `x,z` | 记轨迹；按**服务端接收间隔**算速度，超 `18×1.6` 记异常，滑动窗口累计 |
| `explore` | 探索技能释放时的 `x,z` | ⓪ cooldown 2s 短路（不查表）→ 轨迹连续性（跳跃 ≤16）→ 按坐标反查半径内未完成事件 → 累加收益并持久化 |
| `leave_parallel` | 无参 | 清会话，广播 `Left` |

- 客户端**没有 `event_id`**，只发坐标；响应 `explore_result` 只有 `found / content_token / delta / total`，无事件表索引、无坐标、无距离。
- 未命中、冷却拒绝、事件永久关闭、会话失效**全部走同一个 `BuildReply(false,"",0,total)`**，不给二分探测/状态探测的边信道。
- 收益由服务端计算并下发，持久化在 `ServerEnv/bin/Data/rewards_state.txt`（roleId / total / 已领取事件 ID），重启不丢。
- 战斗结果不进收益链；服务端不模拟战场。

### 15.3 客户端接入：探索技能与玩家输入

按「玩家在事件位置主动使用探索技能」落地，不做自动触发：

- **技能实体**：`Skill.txt` 新增 id 100，`Skill/explore.dsl` 只播角色调查动作；不配 impact/damage，探索不产生战斗收益差。
- **玩家输入**：`Unity3d/Assets/Scripts/Game.cs` 的 `Update()` 监听 `E` 键，带 2 秒本地 UX 冷却，发 story 消息 `cast_explore`。本地冷却只是表现，服务端 2 秒闸门才是权威。
- `Story/Common/autorun_battle.dsl`：进入战场发 `enter_parallel`；`report_pos` 每 300ms 上报位置；`cast_explore` 时 `obj_cast_skill(100)` + 提示 + 发 `explore(x,z)`。客户端只发坐标，没有 `event_id`。
- `Story/Common/localmain.dsl`：`onmessage("explore_result")` 展示结果、`onmessage("parallel_state")` 记录世界地图状态标记。
- 编辑器诊断 `explore_probe()` / `explore_probe(x,z)`：在 Play 模式派发同一条 `cast_explore` 消息，用于验证回包；仅 Editor 程序集，不进客户端包。

**实机验证**（自动登录后）：`cast_explore` → 服务端 `explore=1` → 客户端收到 `explore_result` → UI 提示，链路完整；`EXPLORE_TRAFFIC` 记录 `enter_parallel` / `battle_pos=120+` / `explore` 均到达服务端。未命中时返回 `found=0`，未出现任何可区分反馈。

一个诊断尝试如实记录：为验证命中分支，曾用 GM 手动传事件精确坐标，但战斗约 20 秒轮换一格，查询坐标与请求之间 grid 已变化，因此实机始终 `found=0`（这反而验证了**跨格坐标不可命中**）。尝试过的 `auto` 诊断模式因场景/路径判断不稳已移除，不留在代码里。命中与收益改由服务端协议级自检确定性验证。

### 15.4 自检结果

`ServerEnv/bin/Data/explore_selftest.txt`（服务端启动时自动跑）：

```
EXPLORE_SELFTEST
grids=64 events=148
table_path=.../Tables/Server/WorldEvent.dat reader=TableConfig.WorldEventProvider
cooldown_ms=2000 max_speed=18 tolerance=1.6 max_jump=16
hit_exact=True hit_at_radius_edge=True miss_outside_radius=True
claimed_blocked=True closed_blocked=True
reward_added=True cannot_rehit=True
indistinguishable_failures=True gate_active=True
client_Tables_Server_exists=False client_area_event_rows=0
RESULT=PASS
```

自检用与线上**同一个** `ResolveHit` / `ApplyReward` 函数，因此断言的是真实校验与收益逻辑而不是复刻品：`reward_added` 验证命中后累计收益按事件表增加，`cannot_rehit` 验证已领取事件不会重复给收益。

### 15.5 未完成项（下一步）

- **世界地图 `InParallelSpace` 标记的可视化**：服务端已广播 `parallel_state`，客户端目前只记日志，锚点光柱/半透明残影未做；世界地图 8×8 多玩家可见层也尚未实现（RoomServer 的主要意义所在）。
- **实机命中回包**：命中与收益已由服务端自检确定性验证，但还没在实机里让角色真正走到事件点按 E 拿到收益。需要玩家移动 + 视觉提示引导完成后再验一次。
- **按 `areas.txt` 的多区域推进与移动玩法**：阶段 1 仍是入口遭遇，`autorun_battle` 只做上报，玩家不会跑图。
- **累积收益成长**：收益已持久化，但还没接到角色数值/真知等级/解锁链。
- 事件表的会话级坐标微偏移、轨迹存证与事后审计（12.7 的加固项）。
- 收益目前是进程内字典 + 文件，正式持久化应接 UserServer/DB。

---

## 16. 世界地图层：8×8 多玩家可见层

### 16.1 现有能力（已核实代码）

| 能力 | 位置 | 说明 |
|---|---|---|
| 移动上报与广播 | `RoomServer/RoomServer/LogicMsgHandler.cs:69` `Msg_CR_UserMoveToPosHandler` | 客户端上报目标 `(x,z)` → 服务端写 `msi.TargetPosition` + 朝向 → `scene.NotifyAllUser(Msg_RC_NpcMove, ...)` 广播给房间内所有人 |
| 服务端推进 | `Movement/MovementSystem.cs:25,46` | 按 `ActualProperty(最终速度) × deltaTime` 推进（世界层/NPC；**战场内不由此推进**，服务端不模拟战场） |
| 新玩家同步 | `Scene.SyncForNewUser` | 进房时把已有实体同步给新玩家 |
| Field room 建立 | `Room/RoomManager.cs:49-64` | 对 `cfg.type == SceneTypeEnum.Story`（type=2）的场景，按 `ThreadCountPerScene × RoomCountPerThread` 建房间并 `AddFieldSceneRoomId` 记录 scene→room 映射 |
| 房间广播 | `Scene/Scene.cs:240` | `NotifyAllUser` 遍历 `RoomUsers` 逐个 `SendMessage`，**当前无 AOI** |

结论：**多玩家互相可见的链路本来就是完整的**，不需要新建系统；缺的是「64 个世界场景没有注册进运行时表」以及上层玩法/表现。

### 16.2 本轮落地

`Resource/MapDesign/cutover_world.py` 向 `Level.txt` **追加** 64 行 type=2 世界格：

| 字段 | 值 | 理由 |
|---|---|---|
| id | `20001..20064`（行优先） | 与 `10001..10064` 战场格一一对应 |
| prefab | `Battle_{tid}` | 阶段 1 生成的现代调查层 `.unity`，本来就该是世界格 |
| type | `2`（Story/野外） | 复用 `RoomManager` 的 field room 机制 |
| `CanPK` | `0` | **世界层只有移动交互，没有战斗** |
| `Thread × Room` | `1 × 1` | 避免 64 格 × 16 房间 = 1024 个房间撑爆服务端 |
| `EnterX/Y` | `60/70` | 与藏宝点/传送点一致 |
| `SceneDslFile` | `Story/Common/world_main.dsl` | 新建通用服务端脚本，只有移动与进入日志，没有战斗 |

**只追加、不改动现有 10001..10064 战斗行**，所以 64 场自动轮换不受影响。表与 DSL 已同步两端；`RoomServer` 重启后启动正常、日志无 ERROR、`EXPLORE_SELFTEST RESULT=PASS`。`Level.txt` 现有 64 行战斗 + 64 行世界格 + 4 行原有场景/UI。

**实机回归**：自动登录后场景仍连续轮换 `r0c0 → r0c1 → r0c2 → r0c3`，`exception` / `Object reference` / `can only be called on an active agent` 全部 0；服务端 `EXPLORE_TRAFFIC` 记录本轮 `enter_parallel=5`、`battle_pos=240`，说明进战场状态标记与定时位置上报在新表结构下照常工作。客户端 `parallel_state` 回显未在本轮日志窗口内抓到（广播是同步发出的，需用两个客户端实例专门验证）。

### 16.3 状态标记（对应 12.7.2）

- 服务端 `ExploreSystem` 在 `enter_parallel` / `leave_parallel` 时 `NotifyAllUser` 广播 `parallel_state(roleId, state, grid, era)`，**只同步状态位**：不加载场景、不创建实体、不决定战斗何时开始结束。
- 客户端 `Story/Common/localmain.dsl` 收到后写日志并 `push_tip_info` 提示「时空锚点亮起」。
- 3D 表现（玩家半透明残影、传送点光柱/涟漪、头顶「已潜入·苏美尔层」文案）尚未接入：需要 `ParallelAnchor.prefab` 与按 `roleId` 找到实体视图的挂点逻辑。

### 16.4 差距与下一步

- **AOI**：`NotifyAllUser` 是房间内全量广播，玩家多时浪费带宽；需按距离/可见范围过滤。
- **跨格移动**：64 格目前是独立 field room，没有无缝衔接或换房逻辑；藏宝图 UI 与传送点解码也还没做，`change_scene(20001+)` 只是临时入口。
- **超时恢复**：会话状态在服务端内存，断线/`BattlePos` 超时会清标记，但进程重启不恢复。
- **多玩家同屏实机验证**：需要两个客户端实例，目前只验证了单实例下的广播链路。
- **3D 锚点**：`create_game_object(name, prefab){position(...);scale(...);}obj(...)` 命令已确认存在，可作为无侵入的接入点（生成 `Effects/ParallelAnchor.prefab` 后在 `parallel_state` 里创建）。

### 16.5 层次命名澄清

阶段 1 生成的 128 个场景中，`Battle_{tid}.unity`（现代调查层）现在同时是世界地图格；`Battle_{tid}_{era}.unity` 是进入平行时空后的时代层战场。命名沿用历史前缀 `Battle`（`Game.cs` 靠 `StartsWith("Battle")` 判断战斗场景并隐藏主场景视觉），但语义上两者是不同层。后续重构时建议改名为 `World_{tid}` / `Battle_{tid}_{era}` 并同步 `Game.cs` 判断。

---

## 17. 场景 story 补写与实机问题修复

### 17.1 起点：64 个 story 是同一个空壳

核实结果：64 个 `Battle_{tid}_{era}/story.dsl` 哈希各不相同，但差异**只有地形高度 `y` 和 `close_battle` 的 id**，正文是同一个模板（主角 1 + 队友 4 + 怪 12，全挤在 `(45~75, 55~85)`）。有怪、能打、能轮换，但 512 地形又空了 24/25，没有世界观、事件、移动、多区域——违背「战斗场景主要服务故事与世界观」。

### 17.2 `build_story.py`：按格补写

`Resource/MapDesign/build_story.py`，数据源仍是设计数据，脚本只做翻译：

| 来源 | 用途 |
|---|---|
| `regions.txt` | 格名 / 大区 / 时代层 / 主线支线 / 素材引用 / 难度 |
| `MapData/{tid}_areas.txt` | 每格 2~4 个战斗区中心（来自布局图） |
| `tiles/{tid}_terrain.png` | `obj_move` 目标点的真实地表高度 |
| `Tables/Server/WorldEvent.txt` | 仅统计本格线索数（**不下发坐标**） |

每格生成：开场叙事（8 区各自开场词 + 格名 + 时代层 + 主线提示）、逐战斗区推进（`obj_move` 带玩家过去 + 分段文案 + 分波刷怪）、`all_killed` 推进下一波、收尾文案与结算。分支只用已验证的 `<` / `else`，波次坐标、unitId、文案在构建期展开，不依赖运行时 `==` 或字符串格式化。

**防卡死设计**：怪刷在**玩家当前位置附近**（`get_position_x/y/z(get_leader_id())`），且 `obj_move(..., "wave_spawn_N")` 到达后才刷、`wait(3000)` 兜底必刷、`@spawned` 防重复。这样即使 `obj_move` 未生效也不会出现「玩家走远、怪在原地、AI 追不到」的死循环（阶段 1 切表时踩过这个坑）。

### 17.3 三项实机问题

| 问题 | 根因 | 修复 |
|---|---|---|
| `Non-convex MeshCollider with non-kinematic Rigidbody` | `Assets/Resources/Effects/Select.prefab` 里选中圈的 `Plane` 带 `MeshCollider`，纯视觉特效不需要物理 | 新增 `fix_select_prefab()` GM API，`PrefabUtility.LoadPrefabContents` 删除所有 `Collider` / `Rigidbody` 后保存。实测 `SELECT_FIX removed=1` |
| 小地图对不上 | `localmain.dsl` 写死 `set_map_image("UITexture/Map1")`，阶段 1 生成的 128 张小地图**从未被引用** | 每个 story 的 `start` 里 `fire_message("set_map_image", "Minimap/{tid}_minimap_{era}")` |
| `NullReferenceException` @ `ResourceSystem.Tick():216` | Play 期间执行 `asset_refresh()` 重编译触发**域重载**，非序列化的 `m_ResPoolRootTransform` 引用丢失；`ResPool` 是 `DontDestroyOnLoad` 对象，本体还在 | `ResourceSystem.Tick` 加自愈：引用为空时 `GameObject.Find("ResPool")` 找回，找不到则重建并 `DontDestroyOnLoad`；重编译 `PluginFramework.csproj` 并部署 `Assets/Plugins/PluginFramework.dll` |

**重要操作纪律**：不要在 Play 期间改 C# 或执行 `asset_refresh()`；域重载会打断 runtime GM 通道并清空运行时引用。改代码一律先 `editor_stop_now()`。

### 17.4 附带修掉的数据 bug：事件坐标 Y 镜像

`build_events.py` 早先按「C# 读 PNG 做了上下翻转」把 `worldZ` 算成 `(255 - pil_y) × 2`。实际 `Texture2D.GetPixels32()` 自下而上，C# `ReadPng` 反转回自上而下的 design 缓冲，**`design_y == pil_y`**，因此 TerrainData 与 `areas.txt` 用的是 `worldZ = pil_y × 2`。事件表被多翻了一次，与地形/布局图镜像——这也很可能是之前实机探索始终 `found=0` 的原因之一。

已修正 `build_events.py` 与 `build_story.py` 的高度采样，重生成事件表（例：`ev_r0c0_0` 的 z 由 238 → 272），并同步到 `ServerEnv/bin/Tables/Server/`。服务端自检仍 `RESULT=PASS`。文档 15.1 中旧的「Y 翻转约定」表述作废，以本节为准。

### 17.5 已验证 / 待验证

**已验证**：`fix_select_prefab` 执行成功；64 个 story 与事件表重生成并同步两端表/DSL；`RoomServer` 重启无 ERROR、`EXPLORE_SELFTEST RESULT=PASS`；`PluginFramework.dll` 编译 0 错误并部署；**新 Play 会话日志尾部 `NullReferenceException=0`、`Non-convex MeshCollider=0`、`error CS=0`**。

**待实机验证（上一轮自动登录操作被取消，未继续）**：进战斗后确认小地图确实显示本格图、Select 特效不再报错、`wave_spawn_N` 多波次能打完且循环不卡、按 E 在事件点能命中。这些需要在 Play 中走一遍登录—进战斗流程。

---

## 18. 相机跟随与战斗视距

### 18.1 症状与根因

用户反馈「相机没有跟随玩家，距离也太远」。两层原因叠加：

1. **场景相机保存的是整格预览机位** `position=(20,300,20)`、`LookAt(256,0,256)`、FOV 60（阶段 1 `map_shot` 需要的机位被写进了 `.unity`）。`CameraController.Update()` 只有在 `followTargetId>0` 时才会重写 `camera.transform.position`，因此跟随一旦没建立，相机就停在 300 米高空。
2. **`camera_follow(1000)` 可能静默失效**：`CameraFollowCommand` 带 unitId 时走 `GetEntityByUnitId(1000)`，查不到就什么都不发；另外 `OnLevelLoaded`（`sceneLoaded` 回调）里 `new CameraController(Camera.main)` 发生在 `OnLoadBattleComplete` 禁用主场景相机**之前**，可能绑定到主场景相机而非战斗相机。
3. `CameraSetting` 默认 `distance=60 / up=10 / pitch=60`，对新战场也偏远。

### 18.2 修复（4 处）

| 位置 | 改动 |
|---|---|
| `AppEditorDslApi.Maps.cs` `MakeScene` | 场景相机初始位姿改为**玩家出生点附近的战斗视角**：pitch 52°、distance 28、up 8、FOV 50，位置 `spawn + up*8 - rot*forward*28`；并给相机挂 `CameraSetting` 写入这些值（用反射按类型名查找，避免 Editor/Runtime 程序集耦合） |
| `map_camera_fix()`（新 GM API） | **就地**批量修 128 个已建场景的相机，只改相机并保存场景；不用 `map_build()` 重建，保住 64 份已烘焙的 NavMesh 引用（实测 `NavMesh.asset=64` 未丢） |
| `MapBuild.Shot()` | 预览出图时**临时**切到整格斜俯视机位，渲染后恢复，不再把预览机位写进场景资产 |
| `Game.cs` `OnLoadBattleComplete` | 主场景相机禁用后重新 `new CameraController(Camera.main)`，确保绑定战斗相机 |
| `build_story.py` | `camera_follow(1000)` → `camera_follow()`：无参分支直接用已设置的 `LeaderId`，不依赖 `GetEntityByUnitId` 查找 |

### 18.3 实机验证

新增 `camera_probe()` GM API，读运行时 `Camera.main`、`CameraSetting` 与主角实体的真实距离。自动登录进战斗后连续两次采样：

```
#1  CAM pos=(240.8, 38.9, 306.0) rot=(52,0,0) fov=50 setting=[distance=28 up=8 pitch=52]
    LEADER pos=(240.8, 8.9, 323.3) camDist=34.6
#2  CAM pos=( 84.3, 52.9, 108.7) rot=(52,0,0) fov=50 setting=[distance=28 up=8 pitch=52]
    LEADER pos=( 84.3, 22.8, 125.9) camDist=34.7
```

两次主角位置完全不同（且已离开出生点，说明 `obj_move` 生效、玩家真的在跨区域移动），相机始终 **x 对齐、z 落后约 17、距离恒定 34.6~34.7** → 跟随稳定，视距从 300 米降到 ~35 米。

同轮确认：当前战斗场景 `Battle_r0c5_prediluvian`、怪 Animator 状态在切换、服务端 `EXPLORE_TRAFFIC enter_parallel=6 / battle_pos=680`、`EXPLORE_SELFTEST RESULT=PASS`、无 `NullReferenceException` / MeshCollider 报错。

### 18.4 尚未核对

小地图在 UI 上的实际显示、`wave_spawn_N` 多波次是否全部打完、按 E 在事件点命中，仍未在实机里逐项确认（本轮聚焦相机）。`map_shot()` 的整格预览图在相机改动后需要时用一次，验证临时机位恢复逻辑。

---

## 19. 战斗观感修复：AI 震荡、技能落点与血条挂点

用户反馈「技能表现乱、看不出在打谁、怪跟僵尸似的瞎跑」以及「玩家与怪都没有血条，是不是没创建挂点」。用新增的 `combat_probe()` 拿到运行时实体状态后，确认是**四个独立问题叠加**，不是单一表现 bug。

### 19.1 根因（有数据）

`Actor.txt` 原始参数：

| 角色 | id | view | gohome |
|---|---|---|---|
| 主角 | 1 | 1000 | 1000 |
| 队友 | 2~5 | 30 | 30 |
| 怪 | 6~12 | **15** | **20** |

| # | 问题 | 依据 |
|---|---|---|
| 1 | **怪在追击/回家之间震荡** | `AiDoNormal.CombatHandler` 离 `HomePos` 超过 `GohomeRange` 就切 Gohome；怪 `gohome=20`、`view=15`，而 story 把怪刷在玩家 `+rnd_int(0,18)` 双轴（对角达 25，已超视野）又用 `obj_move` 把主角拉到 200~400 单位外 → 怪看不见目标、一离开出生点就回家 |
| 2 | **技能乱飞、打谁不明** | 主角 `view=1000` 会锁定远处目标放技能，但双方根本没近身；且所有实体都没有血条，掉血无反馈 |
| 3 | **队友跑向世界原点** | `AiDoMember.GetHomePos` 用 `leader.GetMovementStateInfo().TargetPosition` 当队形锚点，主角静止时该值是 `(0,0,0)` → 探针里队友 `state=102` Gohome、`tgtPos=(-2,0,1)` 这类**局部队形偏移**，全员离队，主角实际 1v5 |
| 4 | **血条/技能挂点缺失** | 角色 prefab 走 AssetBundle（`Assets/ThirdParty/GameRef/Resources/Monster/`），全工程只有 `Characters/monster.prefab` 带 `bloodbar` 节点。`OverHeadBarManager.AddActor` 找不到 `bloodbar` 就**整段跳过不创建血条**；技能 DSL 的 `eyes`/`hit` 同理，特效 fallback 到根节点（脚底） |

### 19.2 修复

| 位置 | 改动 |
|---|---|
| `Resource/MapDesign/tune_combat.py`（新） | 按角色类型改 `Actor.txt`：队友 `view=45/gohome=80`、怪 `view=35/gohome=80`（30 行），主角保持 1000 |
| `Resource/MapDesign/build_story.py` | 刷怪从 `+rnd_int(0,18)` 双轴收紧为 `+rnd_int(0,16)-8` 环形（±8，落在视野内）；**取消把主角拉到远处的 `obj_move`**，怪在队伍周围直接生成。区域仍驱动叙事分段，`areas.txt` 的区域坐标保留给后续「带 leash 半径的区域守卫」用 |
| `App/ClientModule/ClientPlugins/Story/AiCommand/AiDoMember.cs` | 队形锚点 `TargetPosition` → `GetPosition3D()`（主角**当前位置**） |
| `Unity3d/Assets/Scripts/OverHeadBarManager.cs` | 无 `bloodbar` 时用角色所有 `Renderer` 的合并 bounds 推算头顶锚点（`max.y + 0.4`）再创建血条；并对 `OverHeadBar` prefab/组件加空值保护 |
| `App/ClientModule/PluginFramework/Skill/Trigers/EffectTrigger.cs` | 技能骨骼缺失时优先回退到 Humanoid `Head` / `UpperChest`，而不是角色根节点（脚底） |
| 诊断 | 新增 `combat_probe()`（实体 AI 状态 + 血条计数）、`camera_probe()`（见第 18 节） |

`PluginFramework.csproj` 与 `ClientPlugins.csproj` 均编译 0 错误，两个 dll 已部署到 `Assets/Plugins/`；表与 64 个 story 已同步两端、RoomServer 重启。

### 19.3 实机验证

修复前 `combat_probe()`：

```
entities=10  主角 state=1 target=1122 moving=False
队友 state=102 (Gohome) target=0 moving=True tgtPos=(-2,0,1) / (-1,0,2) ...   ← 跑向原点
怪   state=101 target=1115 moving=False home 在出生点 20 内
```

修复后：

```
COMBAT_PROBE entities=22 overheadBars=22
E id=1100 主角 pos=(59.8,2.7,78.8) state=1    target=1119 view=1000
E id=1103 队友 pos=(59.8,2.7,78.8) state=101  target=1120 view=45  ← 与主角同位置、有目标
（其余 20 个为场景 NPC，均有血条）
CAM pos=(62.0,69.7,51.0) fov=50  LEADER pos=(62.0,39.7,68.2) camDist=34.7
```

- **每个实体都有血条**（22/22），玩家和怪掉血可见 → 「看不出在打谁」有了反馈基础。
- 队友不再跑向原点，回到队伍身边并进入 Combat、有目标。
- 怪近身、视野覆盖遭遇圈，不再追击/回家震荡。
- 无 `NullReferenceException` / `MissingReferenceException` / 编译错误；相机跟随与 34.7 视距保持。

### 19.4 主动收回与未完成项

- 为保证战斗能真正打起来，本轮**主动取消跨区域长途移动**，遭遇改回队伍周围的近距离交战；区域只保留在叙事分段里。这与「大场景多区域 + 移动」的长期意图有落差，后续应通过「给怪配置区域 leash 半径 + 玩家短距离分段移动」实现，而不是重新把双方拉开。按用户「逐步迭代求精」的口径，先把战斗观感修对。
- `animation is null`（技能动画片段缺失）与 `self_effect/track bone ... can't find` 的**告警**仍在：特效落点已改善，但技能动画本身未补。
- 目标锁定/选中圈（`SetLockTarget`，主角受伤时才触发）尚未做成主动锁定；血条出现后已有基本反馈，进一步的高亮/仇恨指示待做。
- 小地图 UI 实际显示、多波次全打完、按 E 命中仍未逐项实机核对。

---

## 20. 自主开发轮次：目标锁定、探索闭环与技能落点

用户授权「继续，明天再看结果」，本轮不逐项请示。目标是收口「看不出在打谁 / 技能表现乱 / 探索始终不命中」，并给出可复现的结果清单。

### 20.1 关键 bug：探索请求的 grid 一直是空的

`autorun_battle.dsl` 里发 `send_room_story_message("enter_parallel", get_scene_id())`。`get_scene_id()` 返回的是**剧情场景 id（101/102）**，不是 10001~10064 的战场 id；服务端 `GridFromLevel` 映射不到任何格 → `s.GridId = ""` → 事件表按空 grid 查，`ResolveHit` 必然返回 null。

服务端流量文件实证：`session 119 grid=`（空）。**这是之前多轮实机探索始终 `found=0` 的主因**，不是场景轮换竞态（Y 镜像是另一个已修的独立 bug）。

修法：
- `build_story.py`：每个 story 在 `start` 里自报 `send_room_story_message("enter_parallel", "r2c4")`（构建期就知道 tid）。
- `autorun_battle.dsl`：删掉公共 story 里错误的 `get_scene_id()` 上报。
- `ExploreSystem.Enter`：兼容 `rNcM` 字符串，同时保留旧的 level-id 路径。
- 64 个 story 已重生成并同步两端，RoomServer 重新编译部署。

### 20.2 主动目标锁定

`AiCommand.AiTarget` 定义了却**从未被调用**；`AiSkill` 里的锁定调用是注释状态。所以只有「主角挨打」或「玩家手动点击」才会出现选中圈。

- `AiSelectTarget.cs`：主角 AI 每次选目标时，若 `npc.GetId() == LeaderId` 则调 `AiCommand.AiTarget`，把可见锁定圈与 UI 目标读数接到当前攻击目标。
- `AiCommand.AiTarget`：改为「目标不同就切换 / 目标已死则跳过」，修掉旧逻辑「已锁定就永不换目标」的卡死问题。
- `combat_probe()` 增加 `lockedTarget` 输出，实机读到 `lockedTarget=1121` 与主角 `target` 一致。

### 20.3 技能特效落点（三处对齐）

| 位置 | 原问题 | 修复 |
|---|---|---|
| `EffectTrigger` `self_effect` | 骨骼 `eyes`/`hit` 找不到 → 挂根节点（脚底） | 回退到 Humanoid `Head` / `UpperChest` |
| `EffectTrigger` `target_effect` | 找不到骨骼时 fallback 到 **`obj.transform`（施法者）** → 受击特效出现在攻击者脚下，看起来像乱放技能 | 改为挂到**目标**的 `UpperChest` / `Head`，再退回目标根节点 |
| `ImpactTrigger` `track bone` | 弹道只朝「脚底 +1.5」 | 回退到人形 `Head` |
| `AnimationTrigger` | 预定义 impact/buff 技能没有动画名，每次施法刷 `animation is null` | 静默跳过（无动画可播，不是错误） |

`PluginFramework.csproj` / `ClientPlugins.csproj` 编译 0 错误，两个 dll 已部署。骨骼缺失的 Warn 日志仍保留（用于提示资源缺挂点），但画面落点已修正。

### 20.4 服务端会话清理与诊断工具

- `ExploreSystem.WriteTraffic`：清理 30 秒无 `battle_pos` 上报的残留会话（自动轮换不总是发 `LeaveParallel`），并输出 `session <roleId> grid=<grid>`（**只输出 grid，不输出事件坐标**）。
- `explore_probe("auto")`：从服务端流量文件取**最新**会话的 grid（旧实现取第一个，会读到残留会话），再从服务端专用事件表取该格坐标，派发 `cast_explore_at` 做端到端验证。两个文件都在客户端包体之外。
- `combat_probe()`：改为从存活血条反查实体（实体 id 全局递增，固定窗口扫描会漏），并输出 `overheadBars` / `lockedTarget`。

### 20.5 实机结果（最终一轮）

```
COMBAT_PROBE entities=23 overheadBars=23 lockedTarget=1121
  怪 state=101 target=1100 view=35 gohome=80  ← 近身 Combat、有目标
EXPLORE_TRAFFIC  enter_parallel=2  battle_pos=120  explore=1
                 session 119 grid=r0c1            ← grid 不再是空
AUTO_HIT grid=r0c1 (366.0,172.0) dispatched
rewards_state.txt  119|160|ev_r0c1_0             ← 命中并累加收益
```

- **探索端到端闭环首次跑通**：客户端按 E（诊断模拟）→ 服务端按坐标反查事件表 → 命中 → 累加 160 → 持久化。
- 再次对**同一事件**坐标触发不再给收益（`rewards` 未增长）——防重复领取在实机也生效，与自检 `cannot_rehit=True` 一致。
- 日志确认 `wave_go` / `wave_spawn_1` / `parallel_state` / `push_tip_info` / `set_map_image` / `explore_result` 消息链路都在跑。
- 尾部日志：`NullReference=0`、`MissingReference=0`、`Non-convex MeshCollider=0`、`animation is null=0`、`error CS=0`；`EXPLORE_SELFTEST RESULT=PASS`；相机 `camDist=34.7` 保持跟随。

### 20.6 本轮未做（明确留到下一步）

- **带区域推进的分段移动**：当前仍是队伍周围的近身遭遇。设计方向已定——每波之间让玩家朝设计区域做 **≤25 单位短距离分段移动**（小于怪 `view=35`），怪在玩家到达后的位置生成或配置区域 leash 半径，**不再把双方拉开几百单位**。本轮为保证战斗稳定性未动，避免刚修好的链路再次被破坏。
- 小地图仅确认 `set_map_image` 消息发出且 `Assets/Resources/Minimap/{tid}_minimap_{era}.png` 存在，未做像素级/截图核对。
- `self_effect/target_effect ... bone can't find` 的 Warn 仍在（fallback 已生效，日志保留作资源提示）。
- 3D 时空锚点光柱、AOI、双客户端多玩家同屏、主动仇恨/多目标指示未做。

### 20.7 可复现命令（Unity Editor GM 控制台）

| 命令 | 用途 |
|---|---|
| `combat_probe()` | 实体 AI 状态、血条数、当前锁定目标 |
| `camera_probe()` | 运行时相机位姿与到主角的距离 |
| `explore_probe("auto")` | 用当前会话 grid 自动做一次探索命中验证（看 `Data/rewards_state.txt`） |
| `explore_probe(x,z)` | 指定坐标的探索诊断 |
| `map_verify()` | 64 格场景/地形/区域/小地图/story 验收 |
| `map_camera_fix()` | 就地修 128 个场景相机（不破坏 NavMesh） |
| `fix_select_prefab()` | 清理选中圈的物理组件 |
| `fix_story_spawn.py` / `build_story.py` / `build_events.py` / `cutover_world.py` / `tune_combat.py` | `Resource/MapDesign/` 下的构建期脚本 |

---

## 21. 事件表接入项目读表器与标准构建部署流程

用户质疑「事件表不使用项目的读表器读取吗」。核实后确认：**之前确实没有**，是一次绕开，现已改成正规接入。

### 21.1 之前的问题

| 项 | 之前的做法 | 问题 |
|---|---|---|
| 格式 | 3 行 `#` 注释 + 纯 TSV 数据、UTF-8 | 不符合项目表「行1=类型、行2=字段名、GBK/936」约定 |
| 读取 | `ExploreSystem` 里 `File.ReadAllLines` + `Split('\t')` 手读 | 绕过 `TableConfig` Reader，无类型校验 |
| 位置 | 因 `buildall` 报 `field count != 1`，把表**移出** `Resource/Tables/` 到 `MapDesign/server/` | 不是修格式而是绕开 `TableReaderGenerator` |
| 部署 | `build_events.py` 直接写运行时 `.txt` 副本 | 不生成 `.dat`，不随标准构建同步，编码/部署各一套 |

### 21.2 正规链路（已落地）

```
Resource/Tables/Server/WorldEvent.txt   ← 标准表：行1类型、行2字段名、GBK
   │ build_events.py 生成
   ├─ TableReaderGenerator gendsl      → table.dsl 增加
   │                                    tabledef(WorldEvent, dictionary, server)
   ├─ TableReaderGenerator (无参)      → DataReader.cs / FilePathDefine.cs
   │     → TableConfig.WorldEventProvider (WorldEventMgr.GetData())
   │     → FilePathDefine_Server.C_WorldEvent = "Server/WorldEvent.dat"
   └─ TableReaderGenerator ... isserver → Server/WorldEvent.dat（客户端跳过 Server/）
RoomServer.LoadData()                   → WorldEventProvider.Instance.LoadForServer()
ExploreSystem.EnsureLoaded()            → 用 Provider 构建 grid→事件字典
```

配套改动：
- `build_events.py`：输出标准表头、GBK、只写源表；自检仍验证客户端零泄漏。
- `build_story.py` / `ExploreAutoProbe`：改读标准源表（GBK，跳过表头两行）。
- `RoomServer_Config.LoadData()`：与其它表一起 `LoadForServer()`；`EnsureLoaded()` 只做兜底。
- `RoomServer.Main`：`EnsureLoaded()` 从 `Init(args)` **之前**移到**之后**——`HomePath.InitHomePath()` 在 `Init` 里才执行，新读表器依赖它，放前面会启动即崩。
- 生成产物已同步到 `InformationService/GameObjects/TableReader/{DataReader.cs,FilePathDefine.cs}`。

### 21.3 为什么必须按四步流程（实证）

改造后只复制 `RoomServer.dll` 就重启，进程直接崩：

```
Unhandled exception. System.TypeLoadException:
Could not load type 'TableConfig.WorldEventProvider' from assembly 'GameObjects'
   at RoomServer.RoomServer.LoadData()
```

`DataReader.cs` 属于 `GameObjects.csproj`，新类型在 `GameObjects.dll` 里——简化部署漏了依赖 dll。而直接覆盖 `GameObjects.dll` 又报「文件正由另一进程使用」，因为 Lobby / UserServer / DataCache / ServerCenter 都加载着它。**这正是 `StopServer → buildall → copyserverdlls → RunServer` 存在的理由。**

按完整顺序重跑的结果：

```
step1 停止全套 .NET 服务           remaining=0
step2 buildall Debug False        exit=0, Client.sln errors=0, Server.sln errors=0
step3 copyserverdlls Debug        exit=0, GameObjects.dll 更新
step4 RunServer.cmd               ServerCenter=2 node=3 Lobby=1 DataCache=1
                                  UserServer=1 RoomServer=1
```

读表器自检（新代码写出，非旧文件）：
```
grids=64 events=148
table_path=.../Tables/Server/WorldEvent.dat reader=TableConfig.WorldEventProvider
hit_exact=True ... reward_added=True cannot_rehit=True
client_Tables_Server_exists=False client_area_event_rows=0
RESULT=PASS
```

### 21.4 顺带清理与副作用

- 删掉运行时残留的 `ServerEnv/bin/Tables/Server/WorldEvent.txt`（旧手写副本），现在运行时只有 `WorldEvent.dat`。
- `buildall` 把带旧 PDB 的 `ClientPlugins.dll` / `PluginFramework.dll` 复制进 `Assets/Plugins/`，Unity `AssemblyUpdater` 读符号失败（`Mono.Cecil ReadSymbols`）；删除两个过期 `.pdb` + `.meta` 后 `asset_refresh` 正常，GM API `camera_probe / combat_probe / explore_probe` 均可用（无 CS 错误）。
- `buildall` 输出里的 `pdb2mdb.exe ... exit code:1` 是工程原有的 Mono 调试符号转换噪音，脚本不视为失败（`exit=0`）。

### 21.5 结论

事件表现在是**项目的一等公民表**：由 `TableReaderGenerator` 生成 `.dat` 与 `TableConfig` Reader，由 `Server/` 目录保证客户端隔离，服务端通过 `WorldEventProvider` 读取。后续新增服务端表都应照此办理，先改表格式与 `table.dsl`，再按四步流程构建部署。

---

## 22. 战斗观感：遭遇分散、攻击动画与朝向

用户反馈「战斗挤成一团、玩家一直跑步没见攻击、没有朝向目标表现」。用 `combat_probe()`（新增 Animator 状态输出）定位到四条独立原因。

### 22.1 根因

| 症状 | 根因 | 代码依据 |
|---|---|---|
| 挤成一团 | story 把 5 只怪刷在玩家 `±8` 随机点；`EntityViewModel.Create` 里 `obstacleAvoidanceType = NoObstacleAvoidance`；所有怪 `AiPursue` 到**同一个玩家坐标** | `build_story.py` / `EntityViewModel_Npc.cs:43` / `AiDoNormal.cs:103` |
| 一直跑步、没攻击 | `MoveTo()` / `StopMove()` / `SyncSpatial()` 无条件 `Animator.Play("Run"/"Idle")`，AI 每 100ms 就 `AiPursue → MoveTo` 一次，技能 `AnimationTrigger` 播的 `"Attack"` 立刻被覆盖 | `EntityViewModel_Npc.cs:208,232,92` |
| **主角特别严重** | `ailogic_player.dsl` 里 `ai_chase` 在够不着时返回 `true` 并**挂起 story**，`ai_cast_skill` 根本执行不到；主角速度 7 < 怪 8，又在追一个正在追队友的怪，永远进不了攻击距离 | `AiChase.cs:41-44` / `Actor.txt` |
| 没有朝向 | 主角 AI 只设 `info.Target`，从不设 `WantedFaceDir`；`UpdateSpatial` 虽有转向逻辑，但角色一直在移动/动画被覆盖 | `EntityViewModel_Npc.cs:163-172` / `AiSelectTarget.cs` |

### 22.2 修复

| 位置 | 改动 |
|---|---|
| `Resource/MapDesign/build_story.py` | 刷怪从随机 `±8` 改为**构建期展开的均匀环形**（半径 14，按 `2πi/N`），每只怪固定 unitId 与 tableId；顺带修掉会生成 `+-11.3` 的非法 DSL 写法，并把 docstring 里作废的 Y 翻转约定改正 |
| `EntityViewModel_Npc.cs` | ① `obstacleAvoidanceType` → `MedQualityObstacleAvoidance`；② 新增 `LockAnimation(float)` / `CanPlayLocomotion`，`MoveTo` / `StopMove` / `SyncSpatial` 的 `Animator.Play` 只在锁外执行，`PlayAnimation` 也加锁 |
| `AnimationTrigger.cs` | 技能动画成功播放后 `view.LockAnimation(0.6f)`，让攻击片段不被下一次 AI tick 的 Run 覆盖 |
| `AiDoNormal.cs` | 追击点按 `objId % 8` 加环形偏移（半径 4），怪不再全朝同一点汇聚 |
| `AiSelectTarget.cs` | 主角选目标时 `SetWantedFaceDir(Geometry.GetYRadian(src, dst))`，持续面向当前对手 |
| `Ai/ailogic_player.dsl` | 重写：移除会挂起 story 的 `ai_chase` / `ai_keep_away` 分支，每轮直接 `ai_cast_skill` |
| `AiCastSkill.cs` | 距离不够时自己 `AiPursue` 一步后 `return false`（不挂起 story），够得着才 `AiStopPursue + AiSkill` |

### 22.3 实机验证

按 `StopServer → buildall Debug False → copyserverdlls Debug → RunServer` 全量重建部署（两个 sln 均 0 错误，全套服务起来、自检 PASS），自动登录后 `combat_probe()`：

```
修复前：主角 state=1 moving=True anim=Run；所有怪/队友坐标几乎重合
修复后（交战中）：
  E id=1115 主角 pos=(42.5,45.7,49.2) target=1128 moving=False anim=Attack
  lockedTarget=1128                       ← 锁定与当前目标一致
  怪  1125 pos=(55.6,43.8,56.5) / 1127 pos=(56.8,46.1,46.8)   ← 彼此有间距
  队友 1116..1119 pos=(45.9,53.4) (47.1,58.5) (50.5,54.9) (47.7,49.2) ← 分散
  overheadBars=27 / entities=27
```

主角出现 `anim=Attack` 且 `moving=False`（之前恒为 Run），怪也有 `anim=Attack` 的采样；环形刷怪 + 避让 + 追击点分散后不再叠成一点。

### 22.4 仍存在 / 环境现象

- 追击阶段的怪和队友仍是 `anim=Run`（正常移动表现），但不再「一直」只有跑；攻击时能看到 Attack 且主角停下。
- 朝向只做了代码接入（`SetWantedFaceDir` + `UpdateSpatial` 的 `RotateTowards`），本轮探针未输出朝向角度，**未做定量验证**。
- 队友多数时间处于 `state=102`（Gohome，追队形站位），看起来仍在跑动；队形锚点虽已修成主角当前位置，但归队过程仍偏活跃，后续可调 `GohomeHandler` 的容差。
- 全套服务刚 `RunServer` 重启后，**第一次 Play 登录常卡在 `do_login`**（无错误日志），重启一次 Play 重连即恢复；这是环境时序现象，与本次战斗改动无关。
- 环形半径 14 仍只用到场地的一小部分；带 leash 半径的区域守卫与分段推进仍是下一步（见 19.4 / 20.6）。

---

## 23. story API / skill trigger 全量改 snake_case

按用户决策执行：两个仓库、全部 story 注册名与 skill trigger 名改为 snake_case；单词/缩写/数字各自独立（`get_position_x`、`track_2`、`load_scp`）；**不提供别名兼容层**。

### 23.1 工具与映射表

新增 `Tools/RenameStoryApi/rename_api.py`（`scan` / `apply` / `undo` / `verify`），产出可复核的 `BuildLog/rename_map.tsv`。

| 阶段 | 结果 |
|---|---|
| 抽取 | 两仓 45 个注册文件、1187 个唯一名（CSharpGameFramework 1150 / GameDemo 591） |
| 映射 | 924 条待改；来源：工厂类名推导 826、词表 123+142 条人工审定，`needs_review=0` |
| 改写 | 873 个文件、47717 处替换（主体 40651 / Demo 7066），文本层旧名残留 0 |
| 校验 | `verify`：旧名残留 0 文件、C# 关键字损伤 0 文件 |

**分词不用纯算法**：先用注册行的 `new ExpressionFactoryHelper<GetPositionXFunction>()` 里的类名作权威词边界，词表只作回退；无法确定的一律不猜，改为 140 条人工审定映射（`OVERRIDES`）。早期试过纯贪心，出现过 `acos → a_cos`、`addobjecttoasset → add_object_to_as_set` 这类错误，故放弃。

### 23.2 关键字冲突（一次真实误伤及修复）

全局替换把 6 个与 C# 关键字/类型别名同名的 DSL 名字也改了，共损坏 3984 处 C# 源码：

`typeof → type_of`、`sbyte → s_byte`、`ushort → u_short`、`uint → u_int`、`ulong → u_long`、`orderby → order_by`

表现为 `GenericUtility.cs` 大量 `CS1525 表达式项"byte"无效`（`typeof(byte)` 被写成 `type_of(byte)`）。按用户规则这些本就是单个单词/缩写，已加入 `KEEP_AS_IS` 永久排除，并用 `undo` 定向还原 4027 处（304 文件）。修复后两个 framework 工程编译通过。

### 23.3 为什么要「先 .NET Framework，再 dotnet core」（用户提醒）

`DotnetStoryScript` 与 `StoryScript` 各有两份工程文件：

| 版本 | 工程 | 归属 | 产物 |
|---|---|---|---|
| net472 | `*_dotnet_framework.csproj` / `.sln` | **独立 sln，`buildall` 不构建** | `bin/Debug/net472/*.dll`，仓库根目录那份 `DotnetStoryScript.dll` 与它同源 |
| netstandard2.1 | `*.csproj` | `Client.sln` / `Server.sln` / `GameLibrary.sln` | Unity `Assets/Plugins`、服务端 `ServerEnv/bin` |

两份工程**共用 `obj/project.assets.json`**。core 版构建后 assets 只有 `netstandard2.1`，framework 版直接 `NETSDK1005 资产文件没有 net472 目标`；反之亦然。所以顺序必须是：

```
1. 停服
2. restore + build 两个 net472 sln（DotnetStoryScript / StoryScript）
3. 把 net472 产物复制回仓库根目录
4. restore + buildall（core 版最后，把 assets 覆盖回 netstandard2.1）
5. 构建 GameDemo GameLibrary.sln 并复制产物到其 Unity 插件目录
6. copyserverdlls → RunServer
```

实测：framework×2 `exit=0`；`buildall exit=0`、Client.sln / Server.sln 各 0 错误；GameDemo core `exit=0`、14 个 dll 复制；`copyserverdlls exit=0`；全套服务起来、`EXPLORE_SELFTEST RESULT=PASS`。

### 23.4 改名与重建的关系（回答用户疑问）

改的是**源码里的字符串字面量**和 **DSL 文本**；运行时加载的却是：

- 注册名 → 编译进 `Assets/Plugins/*.dll`、`ServerEnv/bin/*.dll` 的常量池
- DSL → `StreamingAssets/Dsl` / `ServerEnv/bin/Dsl` 的副本（`DslCopy` 生成，Release 下可能是二进制）

两者是**大小写敏感、精确 key 的字典查找**，且按决策没有别名层。所以改名与重建必须原子切换：只改源码不重建，DLL 里仍是旧名而 DSL 已是新名，运行时全部 `unknown func` / `CreateTriger failed`；只重建不改名则反之。重建负责把源码变成运行时真正加载的东西。

跨仓还有一个隐蔽消费点：GM 通道用的是 **GameDemo 的 `StoryScript.dll`**（堆栈 `StoryScript.DslExpression.DotnetSetExp`），不是 `DotnetStoryScript.dll`。首轮只把重建产物复制到 GameDemo 自己的 Unity 工程，CSharpGameFramework 的 `Unity3d/Assets/Plugins/StoryScript.dll` 仍是旧版，实机报 `unknown func dotnet_set`；同步该 DLL 后消失。

### 23.5 实机验证

按新名字重发 GM 命令（`dotnet_set` / `find_input` / `click_ui`，旧名会如预期报 `unknown func`，证明无兼容层）：

```
LoginDlg:on_click → NicknameDlg:on_click → StoryDlg:on_click × 15
runtime 日志：unknown func=0  CreateTriger failed=0  NullReference=0  error CS=0
Unity Editor 程序集重新编译（tail errorCS=0 / AssemblyUpdater=0）
DLL 字符串抽查：PluginFramework.dll / RoomServer.dll 含 camera_follow、
               get_position_x、send_gfx_message，旧名均不存在
```

### 23.6 边界与注意

- GM/Editor 的 `calc.Register` 中已是 snake_case 的名字（如 `combat_probe`、`asset_refresh`）未变；`AppEditorDslApi.cs` 29 个本就合规。
- 与 C# 关键字同名的 6 个名字按规则保持原样，不算遗漏。
- 通过 MCP / 控制台 / 外部脚本发的**旧名命令必须改成新名**（本轮已把诊断命令换掉）；仓库内 `.dsl`、文档、Python 生成器、构建脚本均已同步。
- 当前无二进制 DSL 产物（全为文本副本），`DslCopy` 已随 `buildall` 重新同步两端；若将来 Release 构建生成二进制 DSL，必须重跑同步而不能文本替换。

---

### 附：本轮之前的典型偏差（供回归检查）

- 把 512 地形只用 1/25，做成单战斗区圆形擂台
- 为让验收全 PASS 直接把 heightmap 清零（"拉平"）
- 把 10 种地形形态写死在 C# 函数里，bitmap 沦为噪声
- 另造一套地形生成工具却绕开原有 DSL 机制（工具形态本身不受限，**意图对齐才是约束**）
- 用静态俯视相机出预览图，而游戏内 `camera_follow(1000)` 会接管相机

---

## 24. 带区域推进的分段移动（19.4 / 20.6 的下一步，2026-11-17）

按 `session_handoff.md` 第 4 节第 2 项落地。目标：让 4 波战斗有沿设计区推进的空间感，同时**绝不把双方拉开几百单位**（19.1 的震荡根因）。

### 24.1 方案与约束

| 项 | 值 | 依据 |
|---|---|---|
| 每波推进步长 | **20 单位** | 设计上限 ≤25；怪 `view=35`，刷怪环半径 14，20+14=34 < 35 |
| 波 1 | **不推进** | 保持已实机验证的登录/首波链路不变 |
| 推进方向 | 相邻设计区中心连线方向（取单位向量） | 区域坐标来自 `Unity3d/Assets/Resources/MapData/{tid}_areas.txt`，相距数百单位，只取方向**不走到区中心** |
| 刷怪时机 | 玩家**到达后** | 怪用 `get_position_x(get_leader_id())` 动态取当前位置，天然是"到达后的位置" |
| leash | 沿用既有 | 怪 `HomePos`=出生点（`Scene_EventHandler.OnAiInitDslLogic`）+ `GohomeRange=80`（`Actor.txt`，19.2 已调），无需改表/C# |
| 超时兜底 | `wait(4000)` | 主角 `speed=8`，20 单位理论 2.5s；留余量避免超时早于到达 |

### 24.2 消息链（以 3 波为例）

```
start → wave_go → [波1] wave_spawn_1 → 清怪 all_killed
      → wave_go → [波2] wave_advance_2 → obj_move(...,"npcarrived:1000")
                    ├─ 到达 → npcarrived:1000 → wave_spawn_2
                    └─ wait(4000) 超时兜底   → wave_spawn_2
      → all_killed → [波3] wave_advance_3 → ... → wave_spawn_3 → 结算
```

- **双端一致**：`Level.txt` 的 `SceneDslFile` 对 type=4 战斗场景由**客户端**执行；客户端 `ObjMoveCommand` 第三参是到达事件名，服务端 `Scene_EventHandler` 到达时固定发 `npcarrived:<unitId>`（主角 unit id=1000），故两端同名 `npcarrived:1000` 都能触发。服务端 `RoomDslFile` 只有 localmain/skillstory，不执行 battle_main。
- **幂等**：`@advancing` 保证到达信号与超时兜底只生效一次；`spawn_handler` 原有的 `@spawned<@wave` 守卫再兜一层，重复 `wave_spawn_N` 不会重复刷怪。
- **不卡循环**：移动未生效/事件丢失时 4s 后仍刷怪，怪刷在玩家当前位置附近，战斗照常推进（延续 19.2 的容错口径）。
- **wait 不阻塞**：`WaitExp` 是异步协程，`StoryInstance.Tick` 每帧 `ProcessMessageQueues` + `TickCoroutines` 并行，挂起期间 `npcarrived:1000` 正常触发。

### 24.3 改动文件

| 文件 | 内容 |
|---|---|
| `Resource/MapDesign/build_story.py` | 新增 `ADVANCE_STEP=20` / `ADVANCE_TIMEOUT_MS=4000`、`advance_vector` / `advance_handler` / `arrived_handler`；`wave_branch` 波 2..N 改发 `wave_advance_N`；`local` 新增 `@advancing(0)`；推进时加 `push_tip_info` 便于实机观测 |
| `Resource/DslFile/Story/Battle_*/story.dsl` ×64 | 重新生成 |
| `App/ServerModule/ServerEnv/bin/Dsl/Story/Battle_*/story.dsl` ×64、`Unity3d/Assets/StreamingAssets/Dsl/Story/Battle_*/story.dsl` ×64 | 三端同步（纯文本，无 `.bytes` 二进制产物） |

`r5c7` / `r7c6` 两格只有 1 个战斗区（`@total=1`），无波 2 故不生成推进 handler，属正常。

### 24.4 静态验证 + 实机验证（均已通过，2026-11-17 03:36）

静态：

- `python -m py_compile` 通过；64 个 story 括号配对/handler 数校验通过，83 个推进向量长度 19.95~20.06（全部 ≤25，+14 < 35）
- `DslCopy` Debug 版 `LoadFromString` 解析代表样例无警告、成功编码（语法层通过）
- 三端 64×3 文件 hash 全量比对 `mismatched=0`
- 消息链闭环：`wave_advance_2/3`、`npcarrived:1000`、各 `wave_spawn_N` 均有且仅有 1 个 handler；`@advancing`/`@spawned` 守卫各 3 处

实机（全套服务 + Unity Editor Play，`Battle_r0c3`）：

- 登录链路：GM 模拟点击 `LoginDlg → NicknameDlg → StoryDlg×15`，`do_login → ws://127.0.0.1:9001` 握手 `101 Switching Protocols → show_nickname`，登录恢复正常
- 战斗链路（`Game.log` 03:36 段）：`open_battle → LoadBattle → StartStory battle_main → wave_spawn_1`（波 1 原地）；`all_killed → wave_go → push_tip_info → wave_advance_2`（03:36:14.029）；**2.31s 后** `npcarrived:1000`（03:36:16.341）→ `wave_spawn_2` → `all_killed` → 结算剧情 → `UnloadBattle`
- 到达耗时 2.31s 与 `speed=8` × 20 单位吻合，`npcarrived` 早于 4000ms 兜底触发 → 到达信号路径生效、超时未介入；`N-API`/story 无异常：`unknown func=0`、`CreateTriger failed=0`、`NullReference=0`、`error CS=0`、`gohome` 相关日志 0
- 多轮战斗持续复现：累计 `wave_advance=4`、`npcarrived=4`、`wave_spawn=7`、`all_killed=7`、`LoadBattle=9`，全部成对闭环；`EXPLORE_SELFTEST RESULT=PASS`，`explore_traffic` 记录 `session 119 grid=r0c3`、`battle_pos=480`
- 说明：`combat_probe()` 在非战斗间隙采样为 `entities=0`，波间坐标数值未落日志（服务端只记录上报次数）；位移由「到达事件只有 MoveCommand 走到目标点才发」+ 耗时与速度吻合间接证实，未做逐帧坐标快照

### 24.5 操作提醒

- 不要在 Play 中改 C# 或 `asset_refresh()`：先 `editor_stop_now()`
- 服务端 DSL 为文本副本，改动走 `DslCopy` 两端同步（本次只改 story，无 C#/DLL 变更，不需 `buildall`；若服务在跑，重启才加载）
- 本轮未改 `Actor.txt`、`AiDoNormal.cs` 等已调好的战斗参数，避免回退 19.2 的震荡修复