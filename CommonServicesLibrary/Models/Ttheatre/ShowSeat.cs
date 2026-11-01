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
        public DateTime LockExpiry { get; set; }
        public Guid LockToken { get; set; }
    }

    public enum SeatAvailabilityStatus { 
        Available = 1,
        Locked = 2,
        Booked = 3,
    }
}
