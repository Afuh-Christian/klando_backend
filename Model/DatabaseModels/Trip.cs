using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Trip
    {
        public Guid Id { get; set; }
        public string From { get; set; }

        public string To { get; set; }

        public Guid DriverLocationId { get; set; }

        public DateTime DepartureTime { get; set; }
        public int SeatsLeft { get; set; }

        public Guid DriverId { get; set; }  
    }
}
