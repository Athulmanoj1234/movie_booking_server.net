using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace movie_booking.Dtos.Request.Theatre.FirstLevelUploadDto
{
    public class FirstLevelScreenInfo
    {
        public string ScreenName { get; set; }
        public int ScreenCapacity { get; set; }
        public string ScreenType { get; set; }
        public string ScreenAspectRatio { get; set; }
        public string ProjectionFormat { get; set; }
        public string Dimension { get; set; }
        public string AudioSpecific { get; set; }
        public bool IsAcSuppported { get; set; }
    }
}
