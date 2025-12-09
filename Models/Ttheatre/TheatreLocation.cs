namespace movie_booking.Models.Ttheatre
{
    public class TheatreLocation
    {
        public int LocationId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int? TheatreId { get; set; }
        public TheatreInfo TheatreInfo { get; set; }
    }
}
