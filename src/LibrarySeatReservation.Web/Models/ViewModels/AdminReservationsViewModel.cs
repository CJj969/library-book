using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class AdminReservationsViewModel
    {
        public List<AdminReservationItem> Reservations { get; set; }
        public string StatusFilter { get; set; }
        public DateTime? DateFilter { get; set; }
        public string AreaFilter { get; set; }
        public List<string> Areas { get; set; }
    }
}
