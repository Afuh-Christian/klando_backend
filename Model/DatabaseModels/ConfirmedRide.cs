using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class ConfirmedRide
    {
        public Guid Id { get; set; }

      //  public Guid ConfirmedRideId { get; set; }

        public Guid ClientLocationId { get; set; }

        public DateTime DepartureTime { get; set; } = DateTime.Now;

        public Guid DriverId { get; set; }

        public Guid TripId { get; set; }


    }
}
