namespace LibrarySeatReservation.Web.Services
{
    public class AdminLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? AdminId { get; set; }
        public string AdminName { get; set; }
    }

    public interface IAdminService
    {
        AdminLoginResult ValidateLogin(string username, string password);
    }
}
