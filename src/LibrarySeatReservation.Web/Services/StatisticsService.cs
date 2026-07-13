using LibrarySeatReservation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public HomePageStats GetHomePageStats()
        {
            var today = DateTime.Today;
            return new HomePageStats
            {
                TotalSeats = _context.Seats.Count(),
                AvailableSeats = _context.Seats.Count(s => s.IsActive),
                ReservedToday = _context.Reservations
                    .Count(r => r.ReserveDate == today && r.Status == "已预约")
            };
        }

        public AdminStats GetAdminStats()
        {
            var today = DateTime.Today;

            var totalSeats = _context.Seats.Count();
            var availableSeats = _context.Seats.Count(s => s.IsActive);
            var totalReservations = _context.Reservations.Count();
            var todayReservations = _context.Reservations
                .Where(r => r.ReserveDate == today && r.Status == "已预约")
                .Select(r => r.SeatId)
                .Distinct()
                .Count();

            var utilizationRate = availableSeats > 0
                ? Math.Round((double)todayReservations / availableSeats * 100, 1)
                : 0.0;

            return new AdminStats
            {
                TotalSeats = totalSeats,
                TotalReservations = totalReservations,
                TodayReservations = todayReservations,
                UtilizationRate = utilizationRate
            };
        }
    }
}
