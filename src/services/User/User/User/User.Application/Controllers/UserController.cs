using System.Net;
using Microsoft.AspNetCore.Mvc;
using User.User.Domain.Entities.Login;
using User.User.Domain.Repositories;

namespace User.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogin _login;

        public UserController(ILogin login)
        {
            _login = login;
        }

        [HttpPost, Route("[controller]/Login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                var LoginResult = _login.UserLogin(login);
                if(!string.IsNullOrEmpty(LoginResult.ResponseCode))
                {
                    return LoginResult.ResponseCode[..4] switch
                    {
                        "E000" => Ok(new LoginResponse { isSuccessful = true, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Successful Login", DevMessage = "", Data = LoginResult.Response }), //successful login
                        "E100" => BadRequest(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Username must be a valid email address", DevMessage = "", Data = null }), //invalid email address
                        "E200" => Unauthorized(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Wrong username/password", DevMessage = "", Data = null }), //wrong username or password
                        "E300" => BadRequest(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = "", Data = null }), //uncaught exception
                        "E400" => BadRequest(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = LoginResult.ResponseCode[4..], Data = null }), //uncaught exception
                        "E500" => BadRequest(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = "", Data = null }),//uncaught exception
                        _ => BadRequest(new LoginResponse { isSuccessful = false, ErrorCode = LoginResult.ResponseCode[..4], StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = "", Data = null })//uncaught exception
                    };
                }

                return Unauthorized(new LoginResponse { isSuccessful = false, ErrorCode = "E400", StatusMessage = "Cannot authenticate at this time. Please try again later", DevMessage = "", Data = null });//uncaught exception
            }

            catch (Exception ex)
            {
                //logger.Error(ex, ex.Message, ex.StackTrace);
                return BadRequest(new LoginResponse {isSuccessful = false, Data = null, DevMessage = ex.Message, ErrorCode = "E400", StatusMessage = "Cannot authenticate at this time. Please try again later" });
            }
        }
    }
}