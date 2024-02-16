using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResponseTypes
{

  
    public record class LoginResponseithTokens(string AccessToken, int Life , string RefreshToken , DateTime LifeSpan );

    public class LoginResponse
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }


    public class RegisterResponse
    {
        public string Name { get; set;}
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CurrentLocationId { get; set; }
        public Guid ProfilePhotoId { get; set; }

        //public string UserName { get; set; }
    }





    






    public class RefreshToken
    {

        public required string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
    }

}
