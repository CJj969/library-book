using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models.ViewModels
{
    public class CreateReservationViewModel
    {
        public int SeatId { get; set; }

        [Required(ErrorMessage = "请选择预约日期")]
        public DateTime ReserveDate { get; set; }

        [Required(ErrorMessage = "请选择时段")]
        public string TimeSlot { get; set; }
    }
}
