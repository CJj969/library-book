using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IAdminService _adminService;

    public ReservationController(IReservationService reservationService, IAdminService adminService)
    {
        _reservationService = reservationService;
        _adminService = adminService;
    }

    public IActionResult MyReservations()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            TempData["Error"] = "请先切换体验账号";
            return RedirectToAction("Index", "Home");
        }

        var reservations = _reservationService.GetMyReservations(userId.Value);
        var vm = new MyReservationsViewModel
        {
            Reservations = reservations
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateReservationViewModel model)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            TempData["Error"] = "请先切换体验账号";
            return RedirectToAction("Index", "Seat");
        }

        var (success, message) = _reservationService.CreateReservation(
            userId.Value, model.SeatId, model.ReserveDate, model.TimeSlot);

        if (success)
        {
            TempData["Success"] = message;
            return RedirectToAction("MyReservations");
        }
        else
        {
            TempData["Error"] = message;
            return RedirectToAction("Detail", "Seat", new { id = model.SeatId });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancel(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            TempData["Error"] = "请先切换体验账号";
            return RedirectToAction("Index", "Home");
        }

        var (success, message) = _reservationService.CancelReservation(id, userId.Value);

        if (success)
            TempData["Success"] = message;
        else
            TempData["Error"] = message;

        return RedirectToAction("MyReservations");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Switch(int userId)
    {
        var (found, userName, role) = _adminService.GetSwitchUser(userId);
        if (!found)
        {
            TempData["Error"] = "账号不存在";
            return RedirectToAction("Index", "Home");
        }

        HttpContext.Session.SetInt32("UserId", userId);
        HttpContext.Session.SetString("UserName", userName);
        HttpContext.Session.SetString("UserRole", role);

        TempData["Success"] = $"已切换至 {userName}";
        var referer = Request.Headers["Referer"].FirstOrDefault();
        return Redirect(referer ?? "/");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Remove("UserRole");

        TempData["Success"] = "已退出登录";
        return RedirectToAction("Index", "Home");
    }
}
