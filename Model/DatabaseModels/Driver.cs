using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class Driver
    {

        public Guid DriverId { get; set; }      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; }

        public Guid ProfilePhotoId { get; set; }

        public Guid DriversLicenseId { get; set; }

        public Guid CurrentLocationId { get; set; }

        public Guid CarPhotoId { get; set; }

        public string CarModel { get; set; }

        public string CarVersion { get; set; }

        public int CarCapacity {  get; set; } 

    }
}
