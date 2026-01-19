using System.ComponentModel.DataAnnotations;

namespace movie_booking.Dtos.Request.Booking
{
    public class OfflineBookingDto
    {
        [Required(ErrorMessage = "ShowId is required")]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "TheatreId is required")]
        public int TheatreId { get; set; }

        [Required(ErrorMessage = "ScreenId is required")]
        public int ScreenId { get; set; }

        [Required(ErrorMessage = "RowId is required")]
        public int RowId { get; set; }

        [Required(ErrorMessage = "TicketQuantity is required")]
        public int TicketQuantity { get; set; }
    }
}
