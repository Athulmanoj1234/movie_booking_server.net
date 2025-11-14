using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class MovieInfoDirector
    {
      
        public int MovieInfoId { get; set; }
        public MovieInfo MovieInfo { get; set; }

        public int DirectorId { get; set; }
        public DirectorInfo DirectorInfo { get; set; }
    }
}
