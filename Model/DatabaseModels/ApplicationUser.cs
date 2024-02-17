using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public Guid? CurrentLocationId { get; set; }
        public Guid? ProfilePhotoId { get; set; }
    }
}
