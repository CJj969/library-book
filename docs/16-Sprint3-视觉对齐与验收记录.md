# Sprint 3 — 视觉对齐与全链路验收记录

## 概述

Sprint 3 为最终冲刺轮，聚焦 UI 视觉对齐、定制 404 错误页、全链路手工验证和边界情况测试。全部 5 张卡完成。

## 第 1 轮（已完成）

### T13-01 — 用户端 4 页视觉对齐

**改动：**
- `wwwroot/css/site.css` — 全面改写。色值/圆角/间距/字号与 UI 规范一致：
  - 主色 #2563EB（按钮/链接/激活态）
  - 成功色 #059669（空闲卡片）
  - 危险色 #DC2626（取消/删除按钮）
  - 圆角：卡片 12px / 按钮 8px / 输入框 8px
  - 内容区 max-width: 480px（Phone-first）
  - 导航栏背景 #EFF6FF，高 56px
  - 状态 Badge 颜色对齐规范
- 用户端所有视图通过 Bootstrap 类 + site.css 继承样式

### T13-02 — 管理端 4 页视觉对齐

**改动：**
- 管理端视图使用 `container-fluid`（max-width: 1200px）配合 `card border-0 shadow-sm`、`table table-hover`、`modal-content` 等 Bootstrap 类
- site.css 统一样式覆盖所有管理端页面
- 发现并修复 bug：`AdminBaseController.OnActionExecuting` 未尊重 `[AllowAnonymous]` 导致登录页无限重定向

**修复：** `AdminBaseController.cs:11` — 加入 `IAllowAnonymousFilter` 检查，Login GET/POST 不再被拦截。

### T13-03 — 定制 404 错误页

**改动：**
- `Models/ErrorViewModel.cs` — 新增 `StatusCode` 属性
- `Controllers/HomeController.cs:Error()` — 新增 `statusCode` 路由参数
- `Views/Shared/Error.cshtml` — 完全重写为中文 404/500 页，包含说明文字和"回到首页"链接
- `Program.cs` — 新增 `UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}")`

### T13-04 — 全链路手工测试

**成果：** 创建 `docs/15-验收测试报告.md`。

17 项 PRD 验收（T01–T17）全部通过：

| 编号 | 操作 | 结果 |
|------|------|------|
| T01-T09 | 用户端 9 项 | ✅ 全部通过 |
| T10-T17 | 管理端 8 项 | ✅ 全部通过 |

### T13-05 — 边界情况专项测试

8 项边界测试全部通过：

| 编号 | 场景 | 结果 |
|------|------|------|
| B01 | 预约冲突 | ✅ |
| B02 | 过去时段预约 | ✅ |
| B03 | 取消已取消的预约 | ✅ |
| B04 | 删除有活跃预约的座位 | ✅ |
| B05 | 未登录直连 URL | ✅ |
| B06 | 同座位多时段 | ✅ |
| B07 | 管理员退出 | ✅ |
| B08 | 管理员访问用户端 | ✅ |

## 统计

| Sprint | 卡数 | 完成 | 完成率 |
|--------|------|------|--------|
| Sprint 1（用户主链路） | 12 | 12 | 100% |
| Sprint 2（管理端功能） | 12 | 12 | 100% |
| Sprint 3（视觉对齐+验收） | 5 | 5 | 100% |
| **总计** | **29** | **29** | **100%** |

## 已知限制

1. .NET 6.0 SDK arm64 与 dotnet-ef x86_64 架构不匹配，无法使用 EF 迁移工具 — 改用 `EnsureCreated()`
2. 学生账号切换无密码校验，仅用于演示
3. 所有输出为简体中文，未国际化
4. "已完成"状态为计算字段（到期日 < 今天 && 状态=已预约），不持久化到数据库
