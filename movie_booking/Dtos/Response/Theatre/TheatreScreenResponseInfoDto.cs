namespace movie_booking.Dtos.Response.Theatre
{
    public class TheatreScreenResponseInfoDto
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
    }
}
