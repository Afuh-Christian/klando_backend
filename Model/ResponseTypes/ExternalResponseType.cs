using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResponseTypes
{
    public class ExternalResponseType
    {
        public record class Token(string? token);

        public record class GeneralResponseB(int Status , string Message);
        public record class GeneralResponse(bool Flag, string Message);


        public record UserSession(string? Id, string? Name, string? Email, string? Role);
    }

    public class GoogleRequestType
    {
        public string ClientId { get; set; }
        public string Credential { get; set; }

        public string ClientSecret { get; set; }


    }

    public class FacebookRequestType
    {
        public string AppId { get; set; }

        public string AccessToken { get; set; }

    }




    public class UserDto 
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; } 
    }
}