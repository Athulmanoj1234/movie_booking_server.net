namespace movie_booking.Models.Ttheatre
{
    public class TheatreSeat
    {
        public int Id { get; set; }
        public string RowValue { get; set; } // will be like A, B, C or 1, 2, 3
        public string SeatNumber { get; set; } // will be like A1, A2, B1, B2
        public string SeatType { get; set; } //  will be like (standarad, gold, plattinum)
        public string SeatAvailabilityStatus { get; set; }  // will be like isAvailable, cancelled, seatDeffective
        public int? ScreenId { get; set; }
        //public int? TheatreId { get; set; }
        public Screen Screen { get; set; }
        //public TheatreInfo TheatreInfo { get; set; }
    }
}
