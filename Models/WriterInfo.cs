using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_booking.Models
{
    public class WriterInfo
    {
        [Key]
        public int Id { get; set; }
        public string? WriterName { get; set; }

        [JsonIgnore]
        public ICollection<MovieInfo> MovieInfo { get; set; } = new List<MovieInfo>();

        [JsonIgnore]
        public ICollection<MovieInfoWriter> MoviesInfoWriters { get; set; } = new List<MovieInfoWriter>();
    }
}
