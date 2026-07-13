using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Controllers
{
    public class AdminController : AdminBaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserRole") == "Admin")
                return RedirectToAction("Reservations");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = _adminService.ValidateLogin(model.Username, model.Password);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(model);
            }

            HttpContext.Session.SetInt32("AdminId", result.AdminId!.Value);
            HttpContext.Session.SetString("AdminName", result.AdminName!);
            HttpContext.Session.SetString("UserRole", "Admin");

            TempData["Success"] = "登录成功";
            return RedirectToAction("Reservations");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminId");
            HttpContext.Session.Remove("AdminName");
            HttpContext.Session.Remove("UserRole");

            TempData["Success"] = "已退出登录";
            return RedirectToAction("Login");
        }

        public IActionResult Reservations()
        {
            return Content("预约管理页 - 待实现");
        }
    }
}
