using Newtonsoft.Json;
using System.Net;

namespace User.User.Domain.Entities.Login
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            
        }

        public bool isSuccessful { get; set; }
        public string? ErrorCode { get; set; }
        public string? StatusMessage { get; set; }
        public string? DevMessage { get; set; }
        public AzureADResponse? Data { get; set; }
    }

    public class LoginUtilityResponse
    {
        public string? ResponseCode { get; set; }
        public AzureADResponse? Response { get; set; }
    }

    public class AzureADResponse
    {
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [JsonProperty("scope")]
        public string? Scope { get; set; }

        [JsonProperty("expires_in")]
        public string? ExpiresIn { get; set; }

        [JsonProperty("ext_expires_in")]
        public string? ExtExpiresIn { get; set; }

        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
    }
}

