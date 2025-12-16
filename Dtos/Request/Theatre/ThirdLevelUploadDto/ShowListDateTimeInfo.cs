namespace movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto
{
    public class ShowListDateTimeInfo
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public ICollection<ShowListTimeInfo> ShowListsTime { get; set; } = new List<ShowListTimeInfo>();
    }
}
