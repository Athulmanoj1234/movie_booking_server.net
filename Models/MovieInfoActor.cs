using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class MovieInfoActor
    {
       
        public int MovieInfoId { get; set; }
        public MovieInfo MovieInfo { get; set; }

        public int ActorId { get; set; }
        public ActorInfo ActorInfo { get; set; }
    }
}
