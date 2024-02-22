using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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



    public class RefreshTokenResponse
    {
        public string Token { get; set; }
    }



    public class GoogleRequestType
    {
        public string ClientId { get; set; }
        public string Credential { get; set; }

        public string ClientSecret { get; set; }


    }

    public class FacebookRequestType
    {
        //public string AppId { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string  PhotoUrl { get; set; }


        public string  PhoneNumber { get; set; }     

        public string AccessToken { get; set; }

    }


    public class FacebookTokenValidationResult
    {
        [JsonPropertyName("data")] public FacebookTokenValidationData Data { get; set; } = null!;
    }

    public class FacebookTokenValidationData
    {
        [JsonPropertyName("app_id")] public string AppId { get; set; } = null!;

        [JsonPropertyName("type")] public string Type { get; set; } = null!;

        [JsonPropertyName("application")] public string Application { get; set; } = null!;

        [JsonPropertyName("data_access_expires_at")] public int DataAccessExpiresAt { get; set; }

        [JsonPropertyName("expires_at")] public int ExpiresAt { get; set; }

        [JsonPropertyName("is_valid")] public bool IsValid { get; set; }

        [JsonPropertyName("scopes")] public List<string> Scopes { get; set; } = null!;

        [JsonPropertyName("user_id")] public string UserId { get; set; } = null!;
    }

    public class FacebookAppAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
    }

}