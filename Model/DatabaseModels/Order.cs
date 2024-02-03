using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Order
    {

        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public Guid ClientLocationId { get; set; }

        public Guid ClientId { get; set; }
    }
}
