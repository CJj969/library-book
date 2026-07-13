using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        private static readonly string[] ValidTimeSlots = { "上午", "下午", "晚上" };
        private static readonly Dictionary<string, int> TimeSlotEndHours = new()
        {
            ["上午"] = 12,
            ["下午"] = 17,
            ["晚上"] = 22
        };

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public (bool Success, string Message) CreateReservation(
            int userId, int seatId, DateTime reserveDate, string timeSlot)
        {
            var seat = _context.Seats.Find(seatId);
            if (seat == null)
                return (false, "座位不存在");

            if (!seat.IsActive)
                return (false, "该座位当前维修中，无法预约");

            if (reserveDate <= DateTime.Today.AddDays(-1))
                return (false, "不能预约过去的日期");

            if (reserveDate == DateTime.Today && IsTimeSlotPast(timeSlot))
                return (false, "该时段已过，请选择其他时段");

            if (!ValidTimeSlots.Contains(timeSlot))
                return (false, "无效的时段");

            var conflict = _context.Reservations.Any(r =>
                r.SeatId == seatId &&
                r.ReserveDate == reserveDate &&
                r.TimeSlot == timeSlot &&
                r.Status == "已预约");
            if (conflict)
                return (false, "该时段已被预约，请选择其他时段");

            var reservation = new Reservation
            {
                SeatId = seatId,
                UserId = userId,
                ReserveDate = reserveDate,
                TimeSlot = timeSlot,
                Status = "已预约",
                CreatedAt = DateTime.Now
            };
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return (true, "预约成功");
        }

        public List<ReservationItem> GetMyReservations(int userId)
        {
            var now = DateTime.Now;
            var today = DateTime.Today;

            var reservations = _context.Reservations
                .Include(r => r.Seat)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return reservations.Select(r =>
            {
                var isExpired = r.ReserveDate < today ||
                    (r.ReserveDate == today &&
                     TimeSlotEndHours.TryGetValue(r.TimeSlot, out var endHour) &&
                     now.Hour >= endHour);

                string displayStatus;
                bool canCancel;

                if (r.Status == "已取消")
                {
                    displayStatus = "已取消";
                    canCancel = false;
                }
                else if (r.Status == "已预约" && isExpired)
                {
                    displayStatus = "已完成";
                    canCancel = false;
                }
                else if (r.Status == "已预约")
                {
                    displayStatus = "已预约";
                    canCancel = true;
                }
                else
                {
                    displayStatus = r.Status;
                    canCancel = false;
                }

                return new ReservationItem
                {
                    Id = r.Id,
                    SeatNumber = r.Seat?.SeatNumber ?? "",
                    Area = r.Seat?.Area ?? "",
                    ReserveDate = r.ReserveDate,
                    TimeSlot = r.TimeSlot,
                    DisplayStatus = displayStatus,
                    CanCancel = canCancel
                };
            }).ToList();
        }

        public (bool Success, string Message) CancelReservation(int reservationId, int userId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation == null)
                return (false, "预约记录不存在");

            if (reservation.UserId != userId)
                return (false, "无权取消他人的预约");

            if (reservation.Status != "已预约")
                return (false, "该预约已取消或已完成，无需再次操作");

            if (reservation.ReserveDate < DateTime.Today)
                return (false, "已过期的预约无法取消");

            if (reservation.ReserveDate == DateTime.Today && IsTimeSlotPast(reservation.TimeSlot))
                return (false, "当前时段已过，无法取消");

            reservation.Status = "已取消";
            _context.SaveChanges();

            return (true, "取消成功");
        }

        public List<AdminReservationItem> GetAllReservations(
            string statusFilter, DateTime? dateFilter, string areaFilter)
        {
            var query = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Seat)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
                query = query.Where(r => r.Status == statusFilter);

            if (dateFilter.HasValue)
                query = query.Where(r => r.ReserveDate == dateFilter.Value);

            if (!string.IsNullOrEmpty(areaFilter))
                query = query.Where(r => r.Seat.Area == areaFilter);

            var list = query.OrderByDescending(r => r.CreatedAt).ToList();

            var now = DateTime.Now;
            var today = DateTime.Today;

            return list.Select(r =>
            {
                var isExpired = r.ReserveDate < today ||
                    (r.ReserveDate == today &&
                     TimeSlotEndHours.TryGetValue(r.TimeSlot, out var endHour) &&
                     now.Hour >= endHour);

                var canCancel = r.Status == "已预约" && !isExpired;

                return new AdminReservationItem
                {
                    Id = r.Id,
                    UserName = r.User?.Name ?? "",
                    SeatNumber = r.Seat?.SeatNumber ?? "",
                    Area = r.Seat?.Area ?? "",
                    ReserveDate = r.ReserveDate,
                    TimeSlot = r.TimeSlot,
                    Status = r.Status,
                    CanCancel = canCancel
                };
            }).ToList();
        }

        public (bool Success, string Message) AdminCancelReservation(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId);
            if (reservation == null)
                return (false, "预约记录不存在");

            if (reservation.Status != "已预约")
                return (false, "该预约已取消或已完成，无需再次操作");

            if (reservation.ReserveDate < DateTime.Today)
                return (false, "已过期的预约无法取消");

            if (reservation.ReserveDate == DateTime.Today && IsTimeSlotPast(reservation.TimeSlot))
                return (false, "当前时段已过，无法取消");

            reservation.Status = "已取消";
            _context.SaveChanges();

            return (true, "取消成功");
        }

        private static bool IsTimeSlotPast(string timeSlot)
        {
            var now = DateTime.Now;
            return TimeSlotEndHours.TryGetValue(timeSlot, out var endHour)
                && now.Hour >= endHour;
        }
    }
}
