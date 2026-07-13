using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.Models.ViewModels;
using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStatisticsService _statisticsService;

    public HomeController(ILogger<HomeController> logger, IStatisticsService statisticsService)
    {
        _logger = logger;
        _statisticsService = statisticsService;
    }

    public IActionResult Index()
    {
        var stats = _statisticsService.GetHomePageStats();
        var vm = new HomeViewModel
        {
            TotalSeats = stats.TotalSeats,
            AvailableSeats = stats.AvailableSeats,
            ReservedToday = stats.ReservedToday
        };
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int? statusCode = null)
    {
        var code = statusCode ?? (HttpContext.Response.StatusCode == 404 ? 404 : 500);
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            StatusCode = code
        });
    }
}
