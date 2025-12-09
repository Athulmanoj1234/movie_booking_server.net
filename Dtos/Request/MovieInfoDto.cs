namespace movie_booking.Dtos.Request
{
    public class MovieInfoDto
    {
        public string? Genre { get; set; }
        public string? MovieName { get; set; }
        public string? Audiolanguage { get; set; }
        public string? SubtitleLanguage { get; set; }
        public MovieDurationInfo? MovieDurationInfo { get; set; }
        public float? ImdbRating { get; set; }
        public float? AppRating { get; set; }
        public string? MovieDescription { get; set; }
        public string? ClassificationAge { get; set; }
        public IFormFile? MovieCover { get; set; }
        public IFormFile? MovieTrailer { get; set; }
        public string? DirectorName { get; set; }
        public string? ActorName { get; set; }
        public string? WriterName { get; set; }
        public bool IsMovieCommingSoon { get; set; }
        public bool IsMovieCurrentlyRunning { get; set; }
        public bool IsBookingStarted { get; set; }
    }
}
