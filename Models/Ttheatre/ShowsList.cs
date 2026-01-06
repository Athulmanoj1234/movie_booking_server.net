namespace movie_booking.Models.Ttheatre
{
    public class ShowsList
    {
        public int Id { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeSpan ShowStart { get; set; }
        public TimeSpan ShowEnd { get; set; }
        public int? ScreenId { get; set; }
        public int? MovieInfoId { get; set; }
        public Screen Screen { get; set; }
        public MovieInfo Movie { get; set; }
    }
}
