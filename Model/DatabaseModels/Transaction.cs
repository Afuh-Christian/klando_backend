using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Transaction
    {
        public Guid TransactionId { get; set; }
        public DateTime Time { get; set; }
        public Guid DriverId { get; set; }
        public Guid TripId { get; set; }


    }
}
