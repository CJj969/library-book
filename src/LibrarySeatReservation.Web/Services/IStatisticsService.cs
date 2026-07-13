namespace LibrarySeatReservation.Web.Services
{
    public class HomePageStats
    {
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int ReservedToday { get; set; }
    }

    public class AdminStats
    {
        public int TotalSeats { get; set; }
        public int TotalReservations { get; set; }
        public int TodayReservations { get; set; }
        public double UtilizationRate { get; set; }
    }

    public interface IStatisticsService
    {
        HomePageStats GetHomePageStats();
        AdminStats GetAdminStats();
    }
}
