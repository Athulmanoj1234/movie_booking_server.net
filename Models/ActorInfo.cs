using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class ActorInfo
    {
        [Key]
        public int Id { get; set; }
        public string ActorName { get; set; }
        public ICollection<MovieInfoActor> MoviesInfoActors { get; set; } = new List<MovieInfoActor>();
    }
}
