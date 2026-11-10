namespace movie_booking.Dtos.Response.Theatre
{
    public class SeatLayoutForShow
    {
        public int ShowId { get; set; }
        public DateOnly ShowDate { get; set; }
        public DateOnly ShowTime { get; set; }
        public TimeSpan ShowStart { get; set; }
        public TimeSpan ShowEnd { get; set; }   
        public required string ShowDimension { get; set; }
        public int ScreenId { get; set; }
        public required string ScreenName { get; set; }
        public ICollection<RowDetails> RowDetails { get; set; } = new List<RowDetails>();
    }

    public class RowDetails {
        public int RowId { get; set; }
        public required string RowName { get; set; }
        public int RowSeatsCount { get; set; }
        public int RowTicketPrice { get; set; }
        public required string RowType { get; set; }
        public ICollection<SeatDetails> SeatDetails { get; set; } = new List<SeatDetails>();
    }

    public class SeatDetails { 
        public int SeatId { get; set; }
        public required string SeatNumber { get; set; }
        public required string AvailabilityStatus { get; set; }
        public DateTime LockExpiry { get; set; }
        public Guid LockToken { get; set; }
    }
}
