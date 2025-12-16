namespace movie_booking.Dtos.Request.Theatre.FirstLevelUploadDto
{
    public class FirstLevelTheaterInfoDto
    {
        // first level of theatre info upload ie theatre title and theatre screen details
        public string TheatreTitle { get; set; }
        public ICollection<FirstLevelScreenInfo> Screen { get; set; } = new List<FirstLevelScreenInfo>();
    }
}
