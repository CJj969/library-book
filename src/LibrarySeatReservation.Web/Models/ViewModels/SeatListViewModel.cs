using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class SeatListViewModel
    {
        public List<SeatItem> Seats { get; set; }
        public List<string> Areas { get; set; }
        public string CurrentArea { get; set; }
    }
}
