using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Controllers
{
    public class AdminController : AdminBaseController
    {
        private readonly IAdminService _adminService;
        private readonly IReservationService _reservationService;
        private readonly ISeatService _seatService;

        public AdminController(
            IAdminService adminService,
            IReservationService reservationService,
            ISeatService seatService)
        {
            _adminService = adminService;
            _reservationService = reservationService;
            _seatService = seatService;
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

        public IActionResult Reservations(string statusFilter, DateTime? dateFilter, string areaFilter)
        {
            var reservations = _reservationService.GetAllReservations(statusFilter, dateFilter, areaFilter);

            var areas = reservations
                .Select(r => r.Area)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            var availableSeats = _seatService.GetAllSeats();
            var allAreas = availableSeats
                .Select(s => s.Area)
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            var vm = new AdminReservationsViewModel
            {
                Reservations = reservations,
                StatusFilter = statusFilter,
                DateFilter = dateFilter,
                AreaFilter = areaFilter,
                Areas = allAreas
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult CancelReservation(int id)
        {
            var (success, message) = _reservationService.AdminCancelReservation(id);

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Reservations");
        }

        public IActionResult Seats()
        {
            var seats = _seatService.GetAllSeats();
            return View(seats);
        }
    }
}
