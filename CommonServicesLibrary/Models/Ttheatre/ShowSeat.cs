using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonServicesLibrary.Models.Ttheatre
{
    public class ShowSeat
    {
        public int ShowListId { get; set; }
        public ShowsList ShowList { get; set; }
        public int ShowTheatreSeatId { get; set; }
        public TheatreSeat ShowTheatreSeat { get; set; }
        public string AvailabilityStatus { get; set; } = SeatAvailabilityStatus.Available.ToString(); // will be like isAvailable, cancelled, seatDeffective
        // at booking time only lock expiry is required at locking time for the seat also to be decides its expiry time
        public DateTime? LockExpiry { get; set; }
        // lock token acquired by an specific user at seat locking time
        public Guid? LockToken { get; set; }
    }

    public enum SeatAvailabilityStatus { 
        Available = 1,
        Locked = 2,
        Booked = 3,
    }
}
