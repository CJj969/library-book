using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Role { get; set; }

        [MaxLength(100)]
        public string? Password { get; set; }
    }
}
