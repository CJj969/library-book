using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class AdminController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Reservations()
    {
        return View();
    }

    public IActionResult Seats()
    {
        return View();
    }

    public IActionResult Statistics()
    {
        return View();
    }
}
