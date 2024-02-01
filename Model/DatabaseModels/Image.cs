using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Image
    {
        public Guid ImageId { get; set; }
        public byte[] Photo { get; set; }
    }
}
