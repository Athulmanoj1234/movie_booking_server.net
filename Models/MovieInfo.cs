using System.ComponentModel.DataAnnotations;

namespace movie_booking.Models
{
    public class MovieInfo
    {
        [Key]
        public int Id { get; set; }
        public string Genre { get; set; }
        public string MovieName { get; set; }
        public string Audiolanguage { get; set; }
        public string SubtitleLanguage { get; set; }
        public TimeSpan MovieDuration { get; set; }
        public float? ImdbRating { get; set; }
        public float? AppRating { get; set; }
        public string MovieDescription { get; set; }
        public string ClassificationAge { get; set; }
        public string MovieCover { get; set; }
        public string? MovieTrailer { get; set; }
        public ICollection<MovieInfoDirector> MovieInfoDirectors { get; set; } = new List<MovieInfoDirector>();
        public ICollection<MovieInfoActor> MovieInfoActors { get; set; } = new List<MovieInfoActor>();
        public ICollection<MovieInfoWriter> MovieInfoWriters { get; set; } = new List<MovieInfoWriter>();
    }
}
