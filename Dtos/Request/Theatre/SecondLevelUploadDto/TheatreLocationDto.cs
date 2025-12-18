namespace movie_booking.Dtos.Request.Theatre.SecondLevelUploadDto
{
    public class TheatreLocationDto
    {
        public string City { get; set; }    
        public string State { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
