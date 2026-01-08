namespace movie_booking.Dtos.Response.Theatre
{
    public class MovieInfoResponseDto
    {
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
        //public DateOnly MovieReleaseDate { get; set; }
        public bool IsMovieCommingSoon { get; set; }
        public bool IsMovieCurrentlyRunning { get; set; }
        public bool IsBookingStarted { get; set; }
    }
}
