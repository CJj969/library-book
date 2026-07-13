using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Models.Entities;

namespace LibrarySeatReservation.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasIndex(e => e.SeatNumber).IsUnique();
                entity.HasIndex(e => e.Area);

            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasOne(e => e.Seat)
                    .WithMany()
                    .HasForeignKey(e => e.SeatId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ReserveDate);
                entity.HasIndex(e => new { e.UserId, e.Status });

                entity.Property(e => e.Status).HasDefaultValue("已预约");

                entity.HasIndex(e => new { e.SeatId, e.ReserveDate, e.TimeSlot })
                    .IsUnique()
                    .HasDatabaseName("UQ_Reservation_Active");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Role).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasMaxLength(100);
            });
        }
    }
}
