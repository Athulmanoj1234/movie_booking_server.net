namespace movie_booking.Models.Ttheatre
{
    public class TheatreInfo
    {
        public int Id { get; set; }
        public string TheatreTitle { get; set; }
        public int? TheatreLocationId { get; set; }
        public TheatreLocation TheatreLocation { get; set; } = null!;
        public ICollection<Screen> Screen { get; set; } = new List<Screen>();
    }
}
