using LibrarySeatReservation.Web.Models.Entities;

namespace LibrarySeatReservation.Web.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any() || context.Seats.Any())
                return;

            var users = new List<User>
            {
                new User { Name = "学生A", Role = "Student", Password = null },
                new User { Name = "学生B", Role = "Student", Password = null },
                new User { Name = "学生C", Role = "Student", Password = null },
                new User { Name = "学生D", Role = "Student", Password = null },
                new User { Name = "管理员", Role = "Admin", Password = "123456" },
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var seats = new List<Seat>
            {
                new Seat { SeatNumber = "A-01", Area = "2楼A区", Description = "靠窗、有插座", IsActive = true },
                new Seat { SeatNumber = "A-02", Area = "2楼A区", Description = "靠窗", IsActive = true },
                new Seat { SeatNumber = "A-03", Area = "2楼A区", Description = "中间", IsActive = true },
                new Seat { SeatNumber = "A-04", Area = "2楼A区", Description = "中间", IsActive = true },
                new Seat { SeatNumber = "A-05", Area = "2楼A区", Description = "靠墙、有插座", IsActive = true },
                new Seat { SeatNumber = "A-06", Area = "2楼A区", Description = "靠墙", IsActive = true },

                new Seat { SeatNumber = "B-01", Area = "2楼B区", Description = "靠窗", IsActive = true },
                new Seat { SeatNumber = "B-02", Area = "2楼B区", Description = "靠窗、有插座", IsActive = true },
                new Seat { SeatNumber = "B-03", Area = "2楼B区", Description = "中间", IsActive = true },
                new Seat { SeatNumber = "B-04", Area = "2楼B区", Description = "中间", IsActive = true },
                new Seat { SeatNumber = "B-05", Area = "2楼B区", Description = "靠墙", IsActive = true },
                new Seat { SeatNumber = "B-06", Area = "2楼B区", Description = "维修中", IsActive = false },

                new Seat { SeatNumber = "C-01", Area = "3楼A区", Description = "靠窗、有插座", IsActive = true },
                new Seat { SeatNumber = "C-02", Area = "3楼A区", Description = "靠窗", IsActive = true },
                new Seat { SeatNumber = "C-03", Area = "3楼A区", Description = "中间", IsActive = true },
                new Seat { SeatNumber = "C-04", Area = "3楼A区", Description = "中间、有插座", IsActive = true },
                new Seat { SeatNumber = "C-05", Area = "3楼A区", Description = "靠墙", IsActive = true },
                new Seat { SeatNumber = "C-06", Area = "3楼A区", Description = "靠墙", IsActive = true },
                new Seat { SeatNumber = "C-07", Area = "3楼A区", Description = "靠窗", IsActive = false },
                new Seat { SeatNumber = "C-08", Area = "3楼A区", Description = "中间", IsActive = true },
            };
            context.Seats.AddRange(seats);
            context.SaveChanges();
        }
    }
}
