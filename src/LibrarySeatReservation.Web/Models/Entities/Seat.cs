using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models.Entities
{
    public class Seat
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string SeatNumber { get; set; }

        [Required, MaxLength(50)]
        public string Area { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
