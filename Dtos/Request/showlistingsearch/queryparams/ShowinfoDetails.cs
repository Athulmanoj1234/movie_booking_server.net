using System.ComponentModel.DataAnnotations;

namespace movie_booking.Dtos.Request.showlistingsearch.queryparams
{
    public class Showinfodetails
    {
        [Required(ErrorMessage = "MovieID cannot be Empty or undefined")]
        public int MovieID { get; set; }
        [Required(ErrorMessage = "ShowYear cannot be Empty or undefined")]
        public int ShowYear { get; set; }
        [Required(ErrorMessage = "ShowMonth cannot be Empty or undefined")]
        public int ShowMonth { get; set; }
        [Required(ErrorMessage = "ShowDay cannot be Empty or undefined")]
        public int ShowDay { get; set; }
    }
}
