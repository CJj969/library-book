using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Controllers;

public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly AppDbContext _context;

    public ReservationController(IReservationService reservationService, AppDbContext context)
    {
        _reservationService = reservationService;
        _context = context;
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
    public IActionResult Switch(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null)
        {
            TempData["Error"] = "账号不存在";
            return RedirectToAction("Index", "Home");
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role);

        TempData["Success"] = $"已切换至 {user.Name}";
        var referer = Request.Headers["Referer"].FirstOrDefault();
        return Redirect(referer ?? "/");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Remove("UserRole");

        TempData["Success"] = "已退出登录";
        return RedirectToAction("Index", "Home");
    }
}
