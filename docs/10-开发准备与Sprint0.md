# 图书馆座位预约系统 —— 开发准备与 Sprint 0

## 1. 仓库结构

```
SeatBook/                      ← 仓库根目录（GitHub: CJj969/library-book）
├── docs/                      ← 设计文档（全部已完成，不再修改）
├── prototype/                 ← 静态原型（参考素材）
├── Controllers/               ← [待生成]
├── Models/
│   ├── Entities/              ← [待生成]
│   └── ViewModels/            ← [待生成]
├── Services/                  ← [待生成]
├── Data/                      ← [待生成]
├── Views/                     ← [待生成]
├── wwwroot/
│   ├── css/                   ← [待生成]（从 prototype 迁移样式）
│   └── js/                    ← [待生成]
├── appsettings.json           ← [待生成]
├── Program.cs                 ← [待生成]
├── SeatBook.csproj / .sln     ← [待生成]
├── README.md                  ← 本阶段产物
└── .gitignore                 ← 本阶段产物
```

## 2. 分支策略

```
main              ← 稳定可发布版本，只从 dev 合并
  └─ dev          ← 开发主分支，日常集成
       ├─ feat/sprint0-setup      ← Sprint 0：项目骨架搭建（完成后合并 dev）
       ├─ feat/sprint1-user       ← Sprint 1：用户预约链路
       ├─ feat/sprint2-admin      ← Sprint 2：管理端功能
       └─ feat/sprint3-polish     ← Sprint 3：视觉对齐 + 测试 + 文档
```

**策略要点：**
- 每个 feat 分支完成后，提 PR 合并到 `dev`，经评审后合入
- `dev` 稳定后合并到 `main` 作为里程碑发布
- 禁止直接向 `main` 推送

## 3. 提交规范

```
格式：<type>(<scope>): <subject>

类型（type）:
  init     — 项目初始化
  feat     — 新功能
  fix      — Bug 修复
  docs     — 文档变更
  style    — 样式/UI 调整
  refactor — 重构
  test     — 测试相关
  chore    — 构建/工具/依赖

范围（scope）:
  controller / service / view / model / data / db / doc / css

示例：
  feat(service): 实现预约冲突检测逻辑
  feat(controller): 提交预约 POST 处理
  style(css): 实现 mobile-first 480px 布局
  docs(doc): 补充异常边界表格
  chore: 添加 .gitignore
```

## 4. Sprint 0 目标

**Sprint 0 是开发准备 Sprint，不做业务功能。**

| 目标 | 验收标准 |
|------|----------|
| 搭建 ASP.NET Core MVC 项目骨架 | `dotnet build` 编译通过 |
| 配置数据库连接字符串 | `appsettings.json` 包含 LocalDB 连接串 |
| 创建 Entity 类（User/Seat/Reservation） | 三个实体类与数据库设计一致 |
| 创建 AppDbContext + SeedData | `dotnet ef database update` 生成表，Seed 写入测试数据 |
| 首次运行验证 | `dotnet run` 启动成功，浏览器可访问首页（空白页或 Hello World） |
| 远端仓库推送完成 | `git push origin main` 成功 |
| 分支策略落地 | `dev` 分支已创建并推送 |

### Sprint 0 任务清单

| 任务 | 预估时间 | 依赖 |
|------|----------|------|
| 创建 ASP.NET Core MVC 项目 (`dotnet new mvc`) | 10 min | — |
| 安装 EF Core + SqlServer NuGet 包 | 10 min | 项目创建完成 |
| 配置 `appsettings.json` 连接字符串 | 5 min | 项目创建完成 |
| 创建 3 个 Entity 类（User/Seat/Reservation） | 30 min | NuGet 安装完成 |
| 创建 AppDbContext | 15 min | Entity 类完成 |
| 创建 SeedData | 30 min | DbContext 完成 |
| 创建 `.gitignore` | 5 min | — |
| `Add-Migration` + `Update-Database` | 10 min | DbContext 完成 |
| 调整 `Program.cs`（注册 DI + Session + Seed 调用） | 15 min | Service 层就绪 |
| 调整默认 `_Layout.cshtml`（用户端布局框架） | 20 min | 项目创建完成 |
| 首次 `dotnet build` + `dotnet run` | 5 min | 全部代码就绪 |
| Git 初始化 + 首次提交 + 推送到远端 | 10 min | 全部就绪 |

## 5. 主 Sprint 粗计划

| Sprint | 对应里程碑 | 目标 | 最低完成线 | 预计轮次数 |
|--------|-----------|------|-----------|-----------|
| Sprint 1 | M2 核心预约闭环 | 用户主链路：首页→座位列表→座位详情→提交预约→我的预约→取消 | 用户端 4 页全部可访问，预约/取消数据真实写入 DB | 2~3 轮 |
| Sprint 2 | M3 管理端功能 | 管理员登录→预约管理→座位管理 CRUD→统计页 | 管理端 4 页全部可访问，CRUD 操作真实写入 DB | 2~3 轮 |
| Sprint 3 | M4 集成验收与提交 | UI 视觉对齐 + 全链路手工测试 + 文档最终化 + 推送 | 验收测试清单逐条通过 | 1~2 轮 |

> 每个主 Sprint 内允许多轮推进，每轮可以包含：计划 → 开发 → 验证 → 复盘。

## 6. 里程碑节点

| 里程碑 | 时间窗口 | 交付物 | 完成标志 |
|--------|---------|--------|----------|
| M1 需求与设计 | ✅ 已完成 | 10 份设计文档 + 9 页静态原型 + 评审清单 | 审计报告确认可进入开发 |
| M2 核心预约闭环 | Sprint 1 结束 | 用户端 4 页全功能 + 种子数据 | 手工走通：首页→查座→预约→我的预约→取消 |
| M3 管理端功能 | Sprint 2 结束 | 管理端 4 页全功能 | 管理员可登录、管理预约、管理座位、查看统计 |
| M4 集成验收与提交 | Sprint 3 结束 | 视觉对齐 + 全链路测试通过 + 仓库推送 | 验收清单 11 条全部通过，git push 成功 |

## 7. 默认补足项 / 当前假设

| 补充项 | 假设内容 | 理由 |
|--------|----------|------|
| 分支策略 | 采用 main/dev/feat-xxx 三级结构 | 前序文档未明确分支策略 |
| 提交规范 | Conventional Commits 子集（type(scope): subject） | 前序文档未规定提交格式 |
| 任务卡编号 | T{阶段}-{序号} 格式，如 T10-01 | 前序文档未规定编号规则 |
| Sprint 轮次定义 | 每个主 Sprint 内允许多轮推进，每轮包含计划→开发→验证→复盘 | 确保复杂 Sprint 可分步完成 |
| .gitignore | 基于 dotnet 官方模板 + macOS 补充规则 | 标准实践 |
