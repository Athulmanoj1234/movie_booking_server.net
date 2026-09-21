namespace movie_booking.Models
{
    public class Admin
    {
        public Guid AdminId { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public string? Role { get; set; }
        public string? AdminRefreshToken { get; set; }
        public DateTime? AdminRefreshTokenExpiryTime { get; set; }
    }
}
