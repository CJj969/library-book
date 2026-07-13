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
    }
}
