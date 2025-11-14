using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class WriterInfo
    {
        [Key]
        public int Id { get; set; }
        public string WriterName { get; set; }
        public ICollection<MovieInfoWriter> MoviesInfoWriters { get; set; } = new List<MovieInfoWriter>();
    }
}
