using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class DirectorInfo
    {
        [Key]
        public int Id { get; set; }
        public string DirectorName { get; set; }
        public ICollection<MovieInfoDirector> MoviesInfoDirectors { get; set; } = new List<MovieInfoDirector>();
    }
}
