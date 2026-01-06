using System.ComponentModel.DataAnnotations;

namespace movie_booking.Dtos.Request.Theatre.FourthLevelUploadDto
{
    public class ShowListUploadDto
    {
        [Required(ErrorMessage="Date cannot be Empty or undefined")]
        public string Date { get; set; }

        [Required(ErrorMessage = "ShowStartTime cannot be Empty or undefined")]
        public string ShowStartTime { get; set; }

        [Required(ErrorMessage = "ShowEndTime cannot be Empty or undefined")]
        public string ShowEndTime { get; set; }

        [Required(ErrorMessage = "MovieInfoId cannot be Empty or undefined")]
        public int MovieInfoId { get; set; }

        [Required(ErrorMessage = "ScreenId cannot be Empty or undefined")]
        public int ScreenId { get; set; }
    }
}
