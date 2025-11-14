using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class MovieInfoWriter
    {
        
        public int MovieInfoId { get; set; }
        public MovieInfo MovieInfo { get; set; }

        public int WriterId { get; set; }
        public WriterInfo WriterInfo { get; set; }
    }
}
