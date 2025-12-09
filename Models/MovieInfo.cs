using movie_booking.Models.Ttheatre;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public bool IsMovieCommingSoon { get; set; }
        public bool IsMovieCurrentlyRunning { get; set; }
        public bool IsBookingStarted { get; set; }

        public ICollection<DirectorInfo> DirectorInfo { get; set; } = new List<DirectorInfo>();

        public ICollection<WriterInfo> WriterInfo { get; set; } = new List<WriterInfo>();
        public ICollection<ActorInfo> ActorInfo { get; set; } = new List<ActorInfo>();

        //[JsonIgnore]
        public ICollection<MovieInfoDirector> MovieInfoDirectors { get; set; } = new List<MovieInfoDirector>();

        public ICollection<MovieInfoActor> MovieInfoActors { get; set; } = new List<MovieInfoActor>();
        public ICollection<MovieInfoWriter> MovieInfoWriters { get; set; } = new List<MovieInfoWriter>();
        public ICollection<ShowsList> MovieShowList { get; set; } = new List<ShowsList>();
    }
}
