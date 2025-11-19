using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_booking.Models
{
    public class ActorInfo
    {
        [Key]
        public int Id { get; set; }
        public string ActorName { get; set; }

        [JsonIgnore]
        public ICollection<MovieInfo> MovieInfo { get; set; } = new List<MovieInfo>();

        [JsonIgnore]
        public ICollection<MovieInfoActor> MoviesInfoActors { get; set; } = new List<MovieInfoActor>();
    }
}
