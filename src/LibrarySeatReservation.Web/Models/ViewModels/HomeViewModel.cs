namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class HomeViewModel
    {
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int ReservedToday { get; set; }
    }
}
