namespace LibrarySeatReservation.Web.Services
{
    public class SeatItem
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsReservedToday { get; set; }
    }

    public class TimeSlotOccupancy
    {
        public string SlotName { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsPast { get; set; }
    }

    public class SeatDetailResult
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<TimeSlotOccupancy> TimeSlots { get; set; }
    }

    public interface ISeatService
    {
        List<SeatItem> GetAllSeatsWithStatus(DateTime today);
        SeatDetailResult GetSeatDetail(int seatId, DateTime today);
    }
}
