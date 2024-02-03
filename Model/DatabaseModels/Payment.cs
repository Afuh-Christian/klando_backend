using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Payment
    {
        public Guid Id { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public Guid TransactionId { get; set; }
        public Guid DriverId { get; set; }
    }
}
