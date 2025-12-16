namespace movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto
{
    public class ScreenRowsDto
    {
        // row level
        public string RowName { get; set; }
        public string RowType { get; set; } //like gold, platinum which ran
        //public string 
        public int RowTicketPrice { get; set; }
        public int RowSeatsCount { get; set; }
        // seat level
        public TheatreSeatAvailability TheatreSeatAvailability { get; set; }
        //show time and dates level
        public ShowListInfo ShowListInfo { get; set; }
    }
}
