using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySeatReservation.Web.Models.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int SeatId { get; set; }

        public int UserId { get; set; }

        public DateTime ReserveDate { get; set; }

        [Required, MaxLength(10)]
        public string TimeSlot { get; set; }

        [Required, MaxLength(10)]
        public string Status { get; set; } = "已预约";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey(nameof(SeatId))]
        public Seat Seat { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
