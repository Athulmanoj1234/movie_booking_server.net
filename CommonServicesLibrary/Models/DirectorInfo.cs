using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CommonServicesLibrary.Models
{
    public class DirectorInfo
    {
        [Key]
        public int Id { get; set; }
        public string? DirectorName { get; set; }

        [JsonIgnore]
        public ICollection<MovieInfo> MovieInfo { get; set; } = new List<MovieInfo>();

        [JsonIgnore]
        public ICollection<MovieInfoDirector> MoviesInfoDirectors { get; set; } = new List<MovieInfoDirector>();
    }
}
