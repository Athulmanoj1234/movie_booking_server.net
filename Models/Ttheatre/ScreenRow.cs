namespace movie_booking.Models.Ttheatre
{
    public class ScreenRow
    {
        public int Id { get; set; }
        public string RowName { get; set; }
        public string RowType { get; set; }  //like gold platinum etc
        public int RowTicketPrice { get; set; }
        public int RowSeatsCount { get; set; }
        public int? ScreenId { get; set; }
        public int? TheatreId { get; set; }
        public Screen Screen { get; set; }
        public TheatreInfo Theatre { get; set; }
        public ICollection<TheatreSeat> TheatreSeats { get; set; } = new List<TheatreSeat>();
        
    }
}
