namespace movie_booking.Dtos.Request
{
    public class UserRequestDto
    {
        public Guid? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? MobileNumberPrefix { get; set; }
    }
}
