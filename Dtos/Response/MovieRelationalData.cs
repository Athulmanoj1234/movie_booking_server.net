using movie_booking.Models;

namespace movie_booking.Dtos.Response
{
    public class MovieRelationalData
    {
        public DirectorInfo DirectorData { get; set; }
        public ActorInfo ActorData { get; set; }
        public WriterInfo WriterData { get; set; }
    }
}
