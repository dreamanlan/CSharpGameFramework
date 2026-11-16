# 注册名全量改 snake_case：可复用 SOP

来源：2026-11-16 在 `CSharpGameFramework` + `GameDemo` 两仓完成的一次全量改名（1187 个唯一注册名、924 条映射、873 个文件、47717 处替换）。工具实现见 `Tools/RenameStoryApi/rename_api.py`，本文档是给**其它工程/新会话**用的操作手册。

---

## 0. 一句话给新会话

> 注册名是**大小写敏感、精确字典 key、无别名层**的字符串；改名必须同时改「C# 注册点 + 所有 DSL/脚本/文档引用」并**重建全部相关 DLL、同步所有运行时 DSL 副本**，否则运行时 `unknown func` / `CreateTriger failed`。分词不要靠算法猜，用注册行的工厂类名作权威词边界；与 C# 关键字同名的名字保持原样；多 TargetFramework 工程共用 `obj/` 时注意构建顺序。

---

## 1. 先摸清链路（不要跳过）

改名对象通常是：

| 类别 | 典型注册 API | 定义位置特征 |
|---|---|---|
| story 命令/函数 | `registry.Register("name", "doc", factory)` | `GfxStorySystem` / `ServerStorySystem` / `*StoryExpressionRegistrar` |
| skill trigger | `RegisterTrigerFactory("name", ...)` | `GfxSkillSystem` / `*TriggerRegister` |
| GM / Editor API | `calc.Register("name", ...)` | `AppEditorDslApi` / `EditorDslApi` |
| 内建表达式 | `Register("name", ...)` | `DslCalculatorApiRegistry.Init()` |

必须查清：

1. **名字怎么被解析**：`TryGetFactory(name)` / `m_TrigerFactories.TryGetValue(type)` 是否大小写敏感、有无别名/兼容层、有无 `switch(name)`。
2. **运行时读什么**：注册名编译进 DLL；DSL 读取的是 `StreamingAssets/Dsl`、`ServerEnv/bin/Dsl` 等**产物副本**，不是 `Resource/DslFile` 源目录。
3. **DSL 是否会被编码成二进制**：`DslCopy.exe` 的 Debug 版可能 `SaveBinaryFile`；二进制产物**不能文本替换**，只能重新生成。
4. **有几套 DLL 消费者**：跨仓/跨 Unity 工程尤其容易漏（本轮就漏了 `Unity3d/Assets/Plugins/StoryScript.dll`）。

---

## 2. 工具（`rename_api.py`）

四个子命令，`BuildLog/` 下产出可复核文件：

| 命令 | 作用 | 产物 |
|---|---|---|
| `scan` | 抽取注册名、生成映射 | `rename_map.tsv`、`rename_review.tsv`、`rename_registry.tsv` |
| `apply` | 按映射全仓改写 | — |
| `undo` | 定向还原被误改的 C# 关键字 | — |
| `verify` | 旧名残留 / 关键字损伤检查 | — |

迁移到新工程时改这几处：

- `ROOTS`：目标仓库绝对路径（可多个）
- `REG` / `TRIGGER` 正则：适配新工程的注册 API 名
- `TEXT_EXT` / `SKIP_DIRS`：按新工程目录结构调整
- `WORDS`：词表（只作回退）
- `OVERRIDES`：逐条人工审定的显式映射
- `KEEP_AS_IS` / `UNDO`：按目标语言的关键字表调整
- `SELF`：`apply` 必须跳过脚本自身，否则会把 `OVERRIDES` 的 key 改掉

---

## 3. 分词：绝不能纯算法

实测纯贪心词表会产生：

```
acos              -> a_cos                    （错，是单个单词）
addobjecttoasset  -> add_object_to_as_set    （错，asset 被切成 as+set）
average           -> a_v_era_ge              （错，是单个单词）
```

正确做法（按优先级）：

1. **工厂类名作权威词边界**：注册行 `new ExpressionFactoryHelper<GetPositionXFunction>()` → `GetPositionX` → `get_position_x`。本轮 826/924 条由此推导，准确率最高。
2. 词表最长匹配，且**要求完全覆盖**；出现未知段**不猜**，整体保留并列入 `rename_review.tsv`。
3. 所有 `needs_review` 项由人逐条写进 `OVERRIDES`，直到 `needs_review=0`。
4. 数字一律独立 token：`track2 → track_2`、`unitid2objid → unit_id_2_obj_id`。注意类名切出的 `Id2` 要二次拆成 `id` + `2`。
5. 单词/缩写不拆：`byte`、`guid`、`quaternion`、`loadscp` 的 `scp` 保持独立词。
6. 已是 snake_case 的不动；camelCase 的 `implClass` 字符串不是注册名，排除。

---

## 4. 改写前必须排除的东西

| 排除项 | 原因 |
|---|---|
| **C# 关键字 / 类型别名** | 本轮 `typeof/sbyte/ushort/uint/ulong/orderby` 被改名，损坏 3984 处 C# 源码（`typeof(byte)` → `type_of(byte)`，`CS1525`） |
| 类名工厂字符串 | `RegisterSimpleStoryApi` / `Program.cs` 的 `RegisterObjectFactory("AiDoNormal", ...)` 第一参是类名 |
| 二进制 DSL / `.dll`/`.pdb`/`.dat` | 文本替换无效或破坏文件；靠重建 |
| 消息名 / 资源名 / 自定义函数名 | `onmessage("...")`、`call("setcolor")` 里的字符串不是注册名（按注册表白名单替换可自然避开） |
| 脚本自身 | 否则映射表 key 被改，`undo` 失效 |
| `.git` / `Library` / `Temp` / `obj` | — |

**保留要改的**：注册调用第二参的 doc 串（`"camerafollow() - ..."`）、异常消息里的名字、Python 生成器内嵌的命令名、文档命令清单、构建脚本。

替换用 `\b(old)\b` 且按长度降序，大小写敏感。

---

## 5. 重建顺序（本轮最重要的教训）

### 5.1 为什么必须重建

改名改的是源码字符串与 DSL 文本；运行时加载的是 DLL 常量池 + DSL 产物副本。二者是精确 key 匹配且无别名层，**必须原子切换**。只改源码不重建 = 源码新、运行时旧（要么立刻 `unknown func`，要么更隐蔽地等下次 rebuild 才炸）。

### 5.2 多 TargetFramework 顺序

同一目录存在 `X.csproj`（netstandard2.1）与 `X_dotnet_framework.csproj`（net472）时，两者**共用 `obj/project.assets.json`**：

- core 构建后 assets 无 `net472` → framework 构建报 `NETSDK1005 资产文件没有 net472 的目标`
- 所以顺序必须是：**先 framework，后 core**（让 core 最后把 assets 覆盖回主工程需要的目标）

本轮实际顺序：

```
1. 停服（StopServer；否则 DLL 被占用无法复制）
2. msbuild <framework.sln> /t:Restore          # 单独 restore，不能只 Rebuild
3. msbuild <framework.sln> /t:Rebuild /p:Configuration=Debug
4. 把 net472 产物复制回它原有的消费位置（常见：仓库根目录）
5. msbuild <core.sln> /t:Restore ...            # 恢复 assets
6. buildall（core 全量）→ 编译日志 0 错误
7. 其它仓库/工程的 core 版单独构建并复制产物到 Unity 插件目录
8. copyserverdlls → RunServer
9. Unity: asset_refresh → 重启 Play → 实机冒烟
```

**注意**：`buildall` 通常只包含 core 版 sln，`*_dotnet_framework.sln` 是独立 sln，全仓构建脚本从不引用它——必须手动补。

### 5.3 产物复制要覆盖所有消费者

逐一定位同名 DLL 的所有副本，确认哪个是运行时真正加载的。本轮漏项：GM 通道用的是 **GameDemo 的 `StoryScript.dll`**（命名空间 `StoryScript`），不是 `DotnetStoryScript.dll`；重建后只复制到 GameDemo 自己的 Unity 工程，CSharpGameFramework 的 `Unity3d/Assets/Plugins/StoryScript.dll` 仍是旧版 → 实机 `unknown func dotnet_set`。

---

## 6. 校验清单（"完全处理好"的判据）

- [ ] `needs_review=0`，映射表可复核（`rename_map.tsv`）
- [ ] `apply` 的替换数与预期量级相符；`verify` 旧名残留 0
- [ ] **每个相关工程编译 0 错误**（编译能抓出关键字误伤，比文本检查可靠）
- [ ] DLL 字符串抽查：新名存在、旧名不存在（注意 .NET 字符串是 UTF-16LE，两种编码都查）
- [ ] 改名后注册名清单（`rename_registry.tsv`）：剩余全小写连写项都能解释为「单个单词 / 缩写 / implClass」
- [ ] 服务端日志无 `unknown func` / `CreateTriger failed` / 解析异常
- [ ] Unity 实机：登录 → 进战斗 → 用**新名**发 GM 命令，无解析错误
- [ ] 外部脚本 / 控制台 / 文档中的旧名已同步（无兼容层时旧名必然报 unknown func）
- [ ] 若 Release 会生成二进制 DSL：重跑 DslCopy，不要用文本替换

`verify` 有个坑：映射表为空时（幂等重跑）正则 `\b()\b` 会匹配所有单词边界，产生**海量假报警**；实现里要在 map 为空时跳过旧名检查。

---

## 7. 常见故障速查

| 现象 | 原因 |
|---|---|
| `CS1525 表达式项 "byte" 无效` 等大量语法错 | 把 C# 关键字/类型名当注册名改了 → `KEEP_AS_IS` + `undo` |
| `NETSDK1005 ... 没有 net472 的目标` | 共用 `obj/`，core 构建覆盖了 assets → 先 framework 后 core，且各自 `/t:Restore` |
| 实机 `unknown func <oldname>` | 调用方还是旧名（预期，无兼容层）；若用新名仍报，则对应 DLL 没重建/没复制 |
| 某个通道仍认旧名 | 该通道加载的是另一份 DLL 副本，逐仓逐目录排查 |
| `apply` 后脚本自身失效 | 没排除 `SELF`，`OVERRIDES` 的 key 被改 |
| `verify` 报几千个残留 | 空映射导致正则退化，不是真残留 |

---

## 8. 迁移到新工程的最小改动清单

1. 复制 `rename_api.py`，改 `ROOTS`、注册正则、目录排除。
2. 跑 `scan`，看 `unique names` / `needs_review` 量级。
3. 把 `needs_review` 全部审定进 `OVERRIDES`（这一步省不掉）。
4. 按目标语言补 `KEEP_AS_IS`（至少覆盖关键字与类型别名）。
5. `apply` 前先确认 git 可回滚；`apply` 后立刻跑一次全工程编译。
6. 列出该工程所有 TargetFramework 与所有 DLL 消费者，按第 5 节顺序重建。
7. 按第 6 节逐项打勾。
