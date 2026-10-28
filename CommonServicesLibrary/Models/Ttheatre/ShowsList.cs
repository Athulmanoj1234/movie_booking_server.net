using System.Text.Json.Serialization;

namespace CommonServicesLibrary.Models.Ttheatre
{
    public class ShowsList
    {
        public int Id { get; set; }
        public DateOnly ShowDate { get; set; }
        public TimeSpan ShowStart { get; set; }
        public TimeSpan ShowEnd { get; set; }
        public int? ScreenId { get; set; }
        public int? MovieInfoId { get; set; }
        public Screen Screen { get; set; }
        public string ShowDimension { get; set; } = "2d";

        [JsonIgnore]
        public MovieInfo Movie { get; set; }
        public ICollection<TheatreSeat>? Seats { get; set; } = new List<TheatreSeat>();
    }
}
