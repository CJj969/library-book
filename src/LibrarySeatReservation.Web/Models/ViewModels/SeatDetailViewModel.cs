using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class SeatDetailViewModel
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public string Area { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<TimeSlotOccupancy> TimeSlots { get; set; }
        public int? CurrentUserId { get; set; }
    }
}
