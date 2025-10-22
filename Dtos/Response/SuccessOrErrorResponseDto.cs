namespace movie_booking.Dtos.Response
{
    public class SuccessOrErrorResponseDto<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string? Messege { get; set; }
        public T? Data { get; set; }
    }
}
