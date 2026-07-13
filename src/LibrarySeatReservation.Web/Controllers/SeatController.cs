using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Controllers;

public class SeatController : Controller
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    public IActionResult Index(string area = null)
    {
        var today = DateTime.Today;
        var seats = _seatService.GetAllSeatsWithStatus(today);

        var areas = seats
            .Select(s => s.Area)
            .Distinct()
            .OrderBy(a => a)
            .ToList();

        if (!string.IsNullOrEmpty(area) && areas.Contains(area))
        {
            seats = seats.Where(s => s.Area == area).ToList();
        }
        else
        {
            area = areas.FirstOrDefault();
        }

        var vm = new SeatListViewModel
        {
            Seats = seats,
            Areas = areas,
            CurrentArea = area
        };

        return View(vm);
    }

    public IActionResult Detail(int id)
    {
        var today = DateTime.Today;
        var seat = _seatService.GetSeatDetail(id, today);

        if (seat == null)
            return NotFound();

        var userId = HttpContext.Session.GetInt32("UserId");

        var vm = new SeatDetailViewModel
        {
            Id = seat.Id,
            SeatNumber = seat.SeatNumber,
            Area = seat.Area,
            Description = seat.Description,
            IsActive = seat.IsActive,
            TimeSlots = seat.TimeSlots,
            CurrentUserId = userId
        };

        return View(vm);
    }
}
