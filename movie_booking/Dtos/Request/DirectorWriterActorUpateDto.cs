using movie_booking.Models;

namespace movie_booking.Dtos.Request
{
    public class DirectorWriterActorUpdateDto
    {
        public ICollection<DirectorInfo> DirectorData { get; set; } = new List<DirectorInfo>();
        public ICollection<WriterInfo> WriterData { get; set; } = new List<WriterInfo>();
        public ICollection<ActorInfo> ActorData { get; set; } = new List<ActorInfo>();

        //public int DirectorId { get; set; }
    }
}
