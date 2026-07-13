using LibrarySeatReservation.Web.Data;

namespace LibrarySeatReservation.Web.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public AdminLoginResult ValidateLogin(string username, string password)
        {
            var admin = _context.Users.FirstOrDefault(u =>
                u.Name == username && u.Role == "Admin");

            if (admin == null)
                return new AdminLoginResult { Success = false, Message = "账号或密码错误" };

            if (admin.Password != password)
                return new AdminLoginResult { Success = false, Message = "账号或密码错误" };

            return new AdminLoginResult
            {
                Success = true,
                AdminId = admin.Id,
                AdminName = admin.Name
            };
        }

        public (bool Found, string UserName, string Role) GetSwitchUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
                return (false, "", "");

            return (true, user.Name, user.Role);
        }
    }
}
