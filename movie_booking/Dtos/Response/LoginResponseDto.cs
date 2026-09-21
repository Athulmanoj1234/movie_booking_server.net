namespace movie_booking.Dtos.Response
{
    public class LoginResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public string? AdminAccessToken { get; set; }
        public string? AdminRefreshToken { get; set; }

    }
}
