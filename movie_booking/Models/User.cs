namespace movie_booking.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string MobileNumber { get; set; }
        public string? MobileNumberPrefix { get; set; }
        public string? RefreshToken { get; set; } 
        public DateTime? RefreshTokenExpirytime { get; set; } 
    }
}
