namespace movie_booking.Dtos.Response.Theatre
{
    public class TheatreResponse
    {
        public TheatreInfoResponseDto TheatreDetails { get; set; }
        public ICollection<TheatreScreenResponseInfoDto> TheatreScreenResponseDetails { get; set; } = new List<TheatreScreenResponseInfoDto>();
        public ICollection<MovieInfoResponseDto> MovieInfoResponseDetails { get; set; } = new List<MovieInfoResponseDto>();

    }
}
