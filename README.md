# SeatBook — 图书馆座位预约系统

一个面向高校图书馆的轻量级座位预约 Web 应用，支持学生在线查座、预约与取消，管理员管理座位与预约记录。

## 技术栈

| 技术 | 版本 | 用途 |
|------|------|------|
| ASP.NET Core MVC | .NET 6 | Web 框架 |
| Razor | .NET 6 | 服务端页面模板 |
| Entity Framework Core | 6.0.x | ORM / 数据访问 |
| SQLite | 6.0.x | 本地开发数据库 |
| Bootstrap | 5.x | 前端 UI 框架 |
| 自定义 CSS | — | 覆盖 Bootstrap 默认样式 |

## 目录结构

```
src/
└── LibrarySeatReservation.Web/
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── SeatController.cs
    │   ├── ReservationController.cs
    │   ├── AdminController.cs          # Sprint 2
    │   └── AdminBaseController.cs       # Sprint 2
    ├── Models/
    │   ├── Entities/
    │   │   ├── User.cs
    │   │   ├── Seat.cs
    │   │   └── Reservation.cs
    │   └── ViewModels/
    │       ├── HomeViewModel.cs
    │       ├── SeatListViewModel.cs
    │       ├── SeatDetailViewModel.cs
    │       ├── MyReservationsViewModel.cs
    │       ├── CreateReservationViewModel.cs
    │       ├── AdminLoginViewModel.cs    # Sprint 2
    │       └── AdminStatisticsViewModel.cs  # Sprint 2
    ├── Services/
    │   ├── ISeatService.cs / SeatService.cs
    │   ├── IReservationService.cs / ReservationService.cs
    │   ├── IAdminService.cs / AdminService.cs  # Sprint 2
    │   └── IStatisticsService.cs / StatisticsService.cs
    ├── Data/
    │   ├── AppDbContext.cs
    │   └── SeedData.cs
    ├── Views/
    │   ├── Home/Index.cshtml
    │   ├── Seat/Index.cshtml, Detail.cshtml
    │   ├── Reservation/MyReservations.cshtml
    │   ├── Admin/Login.cshtml, Reservations.cshtml, Seats.cshtml, Statistics.cshtml  # Sprint 2
    │   └── Shared/_Layout.cshtml, Error.cshtml
    ├── wwwroot/
    │   ├── css/site.css
    │   └── js/site.js
    ├── appsettings.json
    ├── Program.cs
    └── LibrarySeatReservation.Web.csproj
docs/
├── 01-项目立项单.md
├── 02-需求分析与MVP确认.md
├── 03-PRD-Lite.md
├── 04-页面树与业务流程.md
├── 05-页面卡与UI规范.md
├── 06-静态原型与原型评审.md
├── 07-系统设计说明.md
├── 08-数据库设计.md
├── 09-关键链路详细设计.md
├── 10-开发准备与Sprint0.md
├── 11-开发前一致性总审计.md
├── 12-开发起步与骨架记录.md
├── 13-用户端主链路开发记录.md          # ← 当前阶段
└── 项目任务板与迭代记录.md
prototype/
├── static-v1/
└── review-1/
```

## 运行前提

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)（当前使用 6.0.428）
- SQLite（无需额外安装，由 NuGet 包提供）
- 浏览器（Chrome / Edge 最新版）

## 当前阶段

**Sprint 3 — 视觉对齐与全链路测试 ✅ 已交付。** 404 错误页、用户端/管理端视觉对齐、全链路验收 17 项 + 边界 8 项全部通过。
详见 [docs/14-管理端与权限开发记录.md](docs/14-管理端与权限开发记录.md)。

仓库已推送至：`https://github.com/CJj969/library-book`（main + dev）

安装依赖并启动：
```bash
export PATH="$HOME/.dotnet:$PATH"
cd src/LibrarySeatReservation.Web
dotnet run
```
访问 `http://localhost:5258`。

里程碑：
| 里程碑 | 状态 |
|--------|------|
| M1 需求与设计 | ✅ 已完成 |
| M2 核心预约闭环 | ✅ Sprint 1 已完成 |
| M3 管理端功能 | ✅ Sprint 2 已完成 |
| M4 集成验收与提交 | ✅ Sprint 3（100%） |

---

*以下为后续持续维护段落，随开发推进增量更新。*

## 已实现功能

### 用户端

| 功能 | 路由 | 状态 |
|------|------|------|
| 首页统计 | GET / | ✅ |
| 座位列表（区域 Tab + 状态着色） | GET /Seat/Index | ✅ |
| 座位详情（时段占用图 + 预约表单） | GET /Seat/Detail/{id} | ✅ |
| 提交预约（冲突检测） | POST /Reservation/Create | ✅ |
| 我的预约（状态标签 + 取消按钮） | GET /Reservation/MyReservations | ✅ |
| 取消预约（归属 + 状态 + 时间校验） | POST /Reservation/Cancel/{id} | ✅ |
| 体验账号切换（导航栏下拉） | POST /Reservation/Switch | ✅ |
| 退出登录 | POST /Reservation/Logout | ✅ |

### 管理端

| 功能 | 路由 | 状态 |
|------|------|------|
| 管理员登录（账号密码验证） | GET/POST /Admin/Login | ✅ |
| 权限拦截（未登录跳转登录页） | AdminBaseController | ✅ |
| 管理员退出 | POST /Admin/Logout | ✅ |
| 预约管理列表 + 筛选 | GET /Admin/Reservations | ✅ |
| 管理员取消预约 | POST /Admin/Reservations/Cancel/{id} | ✅ |
| 座位管理列表 | GET /Admin/Seats | ✅ |
| 新增座位 | POST /Admin/Seats/Create | ✅ |
| 编辑座位 | POST /Admin/Seats/Edit/{id} | ✅ |
| 删除座位 | POST /Admin/Seats/Delete/{id} | ✅ |
| 座位状态切换 | POST /Admin/Seats/ToggleStatus/{id} | ✅ |
| 统计页 | GET /Admin/Statistics | ✅ |

## 数据库初始化方式

首次运行时自动建表（`EnsureCreated`）+ 种子数据填充（`SeedData.Initialize()`）。无需手动执行 migration。

数据文件位于：`src/LibrarySeatReservation.Web/SeatBook.db`（已加入 `.gitignore`）

## 演示账号

| 身份 | 用户名 | 密码 | 说明 |
|------|--------|------|------|
| 管理员 | 管理员 | 123456 | 可管理座位与预约（登录 /Admin/Login） |
| 学生 A | 学生A | — | 体验账号（导航栏切换） |
| 学生 B | 学生B | — | 体验账号（导航栏切换） |
| 学生 C | 学生C | — | 体验账号（导航栏切换） |
| 学生 D | 学生D | — | 体验账号（导航栏切换） |

## 演示路径

### 用户端
打开首页 → 点击"查座" → 选择空闲座位查看详情 → 选择日期和时段 → 提交预约 → 查看"我的预约" → 点击"取消预约"

### 管理端
访问 `/Admin/Login` → 输入 管理员 / 123456 → 登录后进入预约管理（预约管理/座位管理/统计，部分待实现）

## 已知限制
- **`dotnet-ef` 不可用**：.NET 6 SDK（arm64）与 `dotnet-ef`（x86_64）架构不匹配，使用 `EnsureCreated()` 替代。
- **手机端适配**：管理端页面为桌面优先设计，手机端未做完整适配（Sprint 3 计划）。
