namespace movie_booking.Dtos.Request.Theatre.ThirdLevelUploadDto
{
    public class ShowListInfo
    {
        public int MovieId { get; set; }
        public ICollection<ShowListDateTimeInfo> ShowLists { get; set; } = new List<ShowListDateTimeInfo>();
    }
}
