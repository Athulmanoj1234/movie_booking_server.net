namespace movie_booking.Models.Ttheatre
{
    public class Screen
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public int ScreenCapacity { get; set; }
        public string ScreenType { get; set; }
        public string AspectRatio { get; set; }
        public string ProjectionFormat { get; set; }
        public string Dimension { get; set; }
        public string Audio { get; set; }
        public bool IsAirConditioner { get; set; }
        public int? TheatreInfoId { get; set; }
        public TheatreInfo TheatreInfo { get; set; }
        public ICollection<TheatreSeat> TheatreSeats { get; set; } = new List<TheatreSeat>();
        public ICollection<ShowsList> ShowsLists { get; set; } = new List<ShowsList>();
        public ICollection<ScreenRow> ScreenRows { get; set; } = new List<ScreenRow>();
    }
}
