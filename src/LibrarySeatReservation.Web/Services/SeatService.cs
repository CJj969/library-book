using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entities;
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

        public List<SeatAdminItem> GetAllSeats()
        {
            return _context.Seats
                .OrderBy(s => s.Area)
                .ThenBy(s => s.SeatNumber)
                .Select(s => new SeatAdminItem
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Area = s.Area,
                    Description = s.Description,
                    IsActive = s.IsActive,
                    ReservationCount = _context.Reservations.Count(r => r.SeatId == s.Id)
                })
                .ToList();
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

        public (bool Success, string Message) CreateSeat(string seatNumber, string area, string description)
        {
            if (string.IsNullOrWhiteSpace(seatNumber))
                return (false, "座位编号不能为空");

            if (string.IsNullOrWhiteSpace(area))
                return (false, "区域不能为空");

            var exists = _context.Seats.Any(s => s.SeatNumber == seatNumber);
            if (exists)
                return (false, "座位编号已存在");

            var seat = new Seat
            {
                SeatNumber = seatNumber,
                Area = area,
                Description = description ?? "",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Seats.Add(seat);
            _context.SaveChanges();

            return (true, "新增成功");
        }

        public (bool Success, string Message) UpdateSeat(int id, string seatNumber, string area, string description)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null)
                return (false, "座位不存在");

            if (string.IsNullOrWhiteSpace(seatNumber))
                return (false, "座位编号不能为空");

            if (string.IsNullOrWhiteSpace(area))
                return (false, "区域不能为空");

            var duplicate = _context.Seats.Any(s => s.SeatNumber == seatNumber && s.Id != id);
            if (duplicate)
                return (false, "座位编号已存在");

            seat.SeatNumber = seatNumber;
            seat.Area = area;
            seat.Description = description ?? "";
            _context.SaveChanges();

            return (true, "修改成功");
        }

        public (bool Success, string Message) DeleteSeat(int id)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null)
                return (false, "座位不存在");

            var hasActiveReservation = _context.Reservations.Any(r =>
                r.SeatId == id && r.Status == "已预约");
            if (hasActiveReservation)
                return (false, "该座位有预约记录，无法删除");

            _context.Seats.Remove(seat);
            _context.SaveChanges();

            return (true, "删除成功");
        }

        public (bool Success, string Message) ToggleSeatStatus(int id)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null)
                return (false, "座位不存在");

            seat.IsActive = !seat.IsActive;
            _context.SaveChanges();

            return (true, seat.IsActive ? "已启用" : "已设为维修中");
        }
    }
}
