using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_booking.Models
{
    public class MovieInfoWriter
    {
        
        public int MovieInfoId { get; set; }

        [JsonIgnore]
        public MovieInfo MovieInfo { get; set; }

        public int WriterId { get; set; }

        public WriterInfo WriterInfo { get; set; }
    }
}
