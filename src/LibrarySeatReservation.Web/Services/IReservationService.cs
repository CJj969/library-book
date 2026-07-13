namespace LibrarySeatReservation.Web.Services
{
    public class ReservationItem
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public string Area { get; set; }
        public DateTime ReserveDate { get; set; }
        public string TimeSlot { get; set; }
        public string DisplayStatus { get; set; }
        public bool CanCancel { get; set; }
    }

    public interface IReservationService
    {
        (bool Success, string Message) CreateReservation(int userId, int seatId, DateTime reserveDate, string timeSlot);
        List<ReservationItem> GetMyReservations(int userId);
        (bool Success, string Message) CancelReservation(int reservationId, int userId);
    }
}
