namespace movie_booking.Models.Ttheatre
{
    public class ShowsList
    {
        public int ShowId { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public int? Screenid { get; set; }
        public int? MovieId { get; set; }
        public Screen Screen { get; set; }
        public MovieInfo Movie { get; set; }
    }
}
