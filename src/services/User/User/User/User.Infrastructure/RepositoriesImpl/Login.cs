using System.Net;
using System.Net.Security;
using Newtonsoft.Json;
using User.User.Domain.Entities.Login;
using User.User.Domain.Repositories;
using System.Text.RegularExpressions;

namespace User.User.Infrastructure.RepositoriesImpl
{
    public class Login : ILogin
    {
        private readonly IConfiguration _config;

        public Login(IConfiguration configuration1)
        {
            _config = configuration1;
        }

        public LoginResponse UserLogin(LoginRequest login)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new
                    RemoteCertificateValidationCallback
                    (
                        delegate { return true; }
                    );

                string baseAddress = _config.GetValue<string>("AzureAd:Instance");
                string tenantID = _config.GetValue<string>("OAuthTenantID");
                string endpoint = _config.GetValue<string>("AzureADLogin:EndPoint");
                string client_id = _config.GetValue<string>("AzureAd:ClientId");
                string client_secret = _config.GetValue<string>("AzureAd:ClientSecret");
                string scope = _config.GetValue<string>("AzureADLogin:Scope");
                string grant_type = _config.GetValue<string>("AzureADLogin:GrantType");
                string? username = login.Username;
                string? password = login.Password;
                string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

                Dictionary<string, string> payload = new();

                payload.Add("client_id", client_id);
                payload.Add("client_secret", client_secret);
                payload.Add("scope", scope);
                payload.Add("grant_type", grant_type);
                if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                {
                    payload.Add("username", "");
                    payload.Add("password", "");
                }
                else
                {
                    payload.Add("username", login.Username);
                    payload.Add("password", login.Password);
                }

                string URL = baseAddress + tenantID + endpoint;
                var clientt = new HttpClient();
                var postTask = clientt.PostAsync(URL, new FormUrlEncodedContent(payload)).GetAwaiter().GetResult();
                var responseStr = postTask.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<AzureADResponse>(responseStr);

                if(postTask.IsSuccessStatusCode) if(!string.IsNullOrEmpty(login.Username)) if(Regex.IsMatch(login.Username, pattern)) return new LoginResponse { isSuccessful = true, ErrorCode = "E000", StatusMessage = "Successful login", DevMessage = "", Data = response };

                if(!string.IsNullOrEmpty(login.Username)) if(!Regex.IsMatch(login.Username, pattern, RegexOptions.IgnoreCase)) return new LoginResponse { isSuccessful = false, ErrorCode = "E100", StatusMessage = "Username must be a valid email address", DevMessage = "", Data = response };

                return new LoginResponse { isSuccessful = false, ErrorCode = "E200", StatusMessage = "Wrong username/password", DevMessage = "", Data = response };
            }
            catch (Exception ex)
            {
                //logger.Error(ex, ex.Message, ex.StackTrace);
                return new LoginResponse { isSuccessful = false, ErrorCode = "E400", StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = ex.Message, Data = null };
            }
        }
    }
}