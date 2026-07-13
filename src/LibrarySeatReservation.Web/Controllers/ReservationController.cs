using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class ReservationController : Controller
{
    public IActionResult MyReservations()
    {
        return View();
    }
}
