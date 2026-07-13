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
        private readonly IStatisticsService _statisticsService;

        public AdminController(
            IAdminService adminService,
            IReservationService reservationService,
            ISeatService seatService,
            IStatisticsService statisticsService)
        {
            _adminService = adminService;
            _reservationService = reservationService;
            _seatService = seatService;
            _statisticsService = statisticsService;
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
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateSeat(SeatEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "请填写必填字段";
                return RedirectToAction("Seats");
            }

            var (success, message) = _seatService.CreateSeat(
                model.SeatNumber, model.Area, model.Description);

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Seats");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSeat(SeatEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "请填写必填字段";
                return RedirectToAction("Seats");
            }

            var (success, message) = _seatService.UpdateSeat(
                model.Id, model.SeatNumber, model.Area, model.Description);

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Seats");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSeat(int id)
        {
            var (success, message) = _seatService.DeleteSeat(id);

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Seats");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleSeatStatus(int id)
        {
            var (success, message) = _seatService.ToggleSeatStatus(id);

            if (success)
                TempData["Success"] = message;
            else
                TempData["Error"] = message;

            return RedirectToAction("Seats");
        }

        public IActionResult Statistics()
        {
            var stats = _statisticsService.GetAdminStats();
            var vm = new AdminStatisticsViewModel { Stats = stats };
            return View(vm);
        }
    }
}
