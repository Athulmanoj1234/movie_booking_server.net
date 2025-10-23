namespace movie_booking.Dtos.Response
{
    public class RefreshResponseDto
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string? AccessToken { get; set; }
    }
}
