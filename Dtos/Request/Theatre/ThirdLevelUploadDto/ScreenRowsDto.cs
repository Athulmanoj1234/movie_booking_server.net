using movie_booking.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto
{
    public class ScreenRowsDto
    {
        // row level
        [Required(ErrorMessage = "row name cannot be empty/null")]
        public string RowName { get; set; }

        [Required(ErrorMessage = "row type cannot be empty/null")]
        public string RowType { get; set; } //like gold, platinum which ran
        //public string 

        [Required(ErrorMessage = "row ticket price cannot be empty/null")]
        [CheckMinimumRange(50)]
        public int RowTicketPrice { get; set; }

        [Required(ErrorMessage = "row name cannot be empty/null")]
        [CheckMinimumRange(1)]
        public int RowSeatsCount { get; set; }
        // seat level

        //[Required(ErrorMessage = "row name cannot be empty")]
        public TheatreSeatAvailability TheatreSeatAvailability { get; set; }
        //show time and dates level

        //[Required(ErrorMessage = "row name cannot be empty")]
        public ShowListInfo ShowListInfo { get; set; }
    }
}
