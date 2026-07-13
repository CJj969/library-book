namespace LibrarySeatReservation.Web.Services
{
    public class HomePageStats
    {
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int ReservedToday { get; set; }
    }

    public interface IStatisticsService
    {
        HomePageStats GetHomePageStats();
    }
}
