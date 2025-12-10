namespace movie_booking.Models.Ttheatre
{
    public class Screen
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCapacity { get; set; }
        public string ScreenType { get; set; }
        public string? TheatreInfoId { get; set; }
        public TheatreInfo TheatreInfo { get; set; }
        public ICollection<TheatreSeat> TheatreSeats { get; set; } = new List<TheatreSeat>();
        public ICollection<ShowsList> ShowsLists { get; set; } = new List<ShowsList>();
    }
}
