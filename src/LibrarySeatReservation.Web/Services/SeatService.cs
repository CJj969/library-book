using LibrarySeatReservation.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services
{
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;

        private static readonly string[] ValidTimeSlots = { "上午", "下午", "晚上" };

        private static readonly Dictionary<string, int> TimeSlotEndHours = new()
        {
            ["上午"] = 12,
            ["下午"] = 17,
            ["晚上"] = 22
        };

        public SeatService(AppDbContext context)
        {
            _context = context;
        }

        public List<SeatItem> GetAllSeatsWithStatus(DateTime today)
        {
            var seats = _context.Seats.ToList();
            var reservedSeatIds = _context.Reservations
                .Where(r => r.ReserveDate == today && r.Status == "已预约")
                .Select(r => r.SeatId)
                .Distinct()
                .ToHashSet();

            return seats.Select(s => new SeatItem
            {
                Id = s.Id,
                SeatNumber = s.SeatNumber,
                Area = s.Area,
                Description = s.Description,
                IsActive = s.IsActive,
                IsReservedToday = !s.IsActive || reservedSeatIds.Contains(s.Id)
            }).ToList();
        }

        public SeatDetailResult GetSeatDetail(int seatId, DateTime today)
        {
            var seat = _context.Seats.Find(seatId);
            if (seat == null) return null;

            var todayReservations = _context.Reservations
                .Where(r => r.SeatId == seatId && r.ReserveDate == today)
                .ToList();

            var reservedSlots = todayReservations
                .Where(r => r.Status == "已预约")
                .Select(r => r.TimeSlot)
                .ToHashSet();

            var now = DateTime.Now;
            var timeSlots = ValidTimeSlots.Select(slot => new TimeSlotOccupancy
            {
                SlotName = slot,
                IsOccupied = reservedSlots.Contains(slot),
                IsPast = TimeSlotEndHours.TryGetValue(slot, out var endHour) && now.Hour >= endHour
            }).ToList();

            return new SeatDetailResult
            {
                Id = seat.Id,
                SeatNumber = seat.SeatNumber,
                Area = seat.Area,
                Description = seat.Description,
                IsActive = seat.IsActive,
                TimeSlots = timeSlots
            };
        }
    }
}
