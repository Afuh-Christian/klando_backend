using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Location
    {
        public Guid Id { get; set; }
        public double Longitude { get; set; } = 0.0;
        public double Latitude { get; set; } = 0.0;
    }
}
