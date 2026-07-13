# SeatBook — 图书馆座位预约系统

一个面向高校图书馆的轻量级座位预约 Web 应用，支持学生在线查座、预约与取消，管理员管理座位与预约记录。

## 技术栈

| 技术 | 版本 | 用途 |
|------|------|------|
| ASP.NET Core MVC | .NET 8 | Web 框架 |
| Razor | .NET 8 | 服务端页面模板 |
| Entity Framework Core | 8.x | ORM / 数据访问 |
| SQL Server LocalDB | 随 VS/SSDT 安装 | 本地开发数据库 |
| Bootstrap | 5.x | 前端 UI 框架 |
| 自定义 CSS | — | 覆盖 Bootstrap 默认样式 |

## 目录结构

### 当前已存在（设计文档阶段）

```
SeatBook/
├── docs/
│   ├── 01-项目立项单.md                    # 项目发起与范围
│   ├── 02-需求分析与MVP确认.md             # 需求优先级矩阵
│   ├── 03-PRD-Lite.md                     # 产品需求文档
│   ├── 04-页面树与业务流程.md               # 页面导航与状态机
│   ├── 05-页面卡与UI规范.md                # 页面卡与色值规范
│   ├── 06-静态原型与原型评审.md             # 原型说明
│   ├── 07-系统设计说明.md                  # 架构与分层设计
│   ├── 08-数据库设计.md                    # 表结构与索引
│   ├── 09-关键链路详细设计.md              # 主链路逐步骤设计
│   └── 10-开发准备与Sprint0.md            # ← 当前阶段
├── prototype/
│   ├── static-v1/                         # 静态 HTML 原型（9 页）
│   └── review-1/                          # 原型评审清单
├── README.md                              # ← 当前阶段
├── .gitignore                             # ← 当前阶段
└── 项目任务板与迭代记录.md                # ← 当前阶段（见 docs/）
```

### 后续计划 / 待生成（开发阶段）

```
SeatBook/
├── Controllers/
│   ├── HomeController.cs
│   ├── SeatController.cs
│   ├── ReservationController.cs
│   ├── AdminController.cs
│   └── AdminBaseController.cs
├── Models/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Seat.cs
│   │   └── Reservation.cs
│   └── ViewModels/
│       ├── HomeViewModel.cs
│       ├── SeatListViewModel.cs
│       ├── SeatDetailViewModel.cs
│       ├── ReservationViewModel.cs
│       ├── AdminLoginViewModel.cs
│       ├── AdminReservationsViewModel.cs
│       └── AdminStatisticsViewModel.cs
├── Services/
│   ├── ISeatService.cs / SeatService.cs
│   ├── IReservationService.cs / ReservationService.cs
│   ├── IAdminService.cs / AdminService.cs
│   └── IStatisticsService.cs / StatisticsService.cs
├── Data/
│   ├── AppDbContext.cs
│   └── SeedData.cs
├── Views/
│   ├── Home/Index.cshtml
│   ├── Seat/Index.cshtml, Detail.cshtml
│   ├── Reservation/MyReservations.cshtml
│   ├── Admin/Login.cshtml, Reservations.cshtml, Seats.cshtml, Statistics.cshtml
│   └── Shared/_Layout.cshtml, _AdminLayout.cshtml, Error.cshtml
├── wwwroot/
│   ├── css/site.css
│   └── js/site.js
├── appsettings.json
├── Program.cs
└── SeatBook.csproj
```

## 运行前提

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server LocalDB（随 Visual Studio 安装，或通过 [SqlLocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) 独立安装）
- 浏览器（Chrome / Edge 最新版）

## 当前阶段

**Sprint 0 — 开发准备阶段**。详见 [docs/10-开发准备与Sprint0.md](docs/10-开发准备与Sprint0.md)。

里程碑：
| 里程碑 | 状态 |
|--------|------|
| M1 需求与设计 | ✅ 已完成 |
| M2 核心预约闭环 | ⏳ Sprint 1 |
| M3 管理端功能 | 📅 Sprint 2 |
| M4 集成验收与提交 | 📅 Sprint 3 |

---

*以下为后续持续维护段落，随开发推进增量更新。*

## 已实现范围

⚠️ 待开发阶段补充。预计于 Sprint 1~3 逐项标注完成状态。

## 数据库初始化方式

⚠️ 待开发阶段补充。首次运行前需执行 `dotnet ef database update`，启动时通过 `SeedData.Initialize()` 自动插入测试数据。

## 演示账号

| 身份 | 用户名 | 密码 | 说明 |
|------|--------|------|------|
| 管理员 | admin | 123456 | 可管理座位与预约 |
| 学生 A | 学生A | — | 体验账号（导航栏切换） |
| 学生 B | 学生B | — | 体验账号（导航栏切换） |
| 学生 C | 学生C | — | 体验账号（导航栏切换） |

## 已知限制

- ⚠️ 待开发阶段补充。
