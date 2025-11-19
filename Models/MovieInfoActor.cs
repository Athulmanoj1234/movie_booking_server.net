using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_booking.Models
{
    public class MovieInfoActor
    {
       
        public int MovieInfoId { get; set; }

        [JsonIgnore]
        public MovieInfo MovieInfo { get; set; }

        public int ActorId { get; set; }

        public ActorInfo ActorInfo { get; set; }
    }
}
