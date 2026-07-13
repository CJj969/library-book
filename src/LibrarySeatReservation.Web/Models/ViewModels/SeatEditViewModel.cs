using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class SeatEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入座位编号")]
        public string SeatNumber { get; set; }

        [Required(ErrorMessage = "请输入区域")]
        public string Area { get; set; }

        public string Description { get; set; }
    }
}
