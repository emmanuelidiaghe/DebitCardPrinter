using System.Net;
using System.Web;
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

        //private IActionResult Badrequest<T>(T obj) => Content(HttpStatusCode.BadRequest.ToString(), obj);

        //private IActionResult Unauthorized<T>(T obj) => Content(HttpStatusCode.BadRequest.ToString(), obj);

        [HttpPost, Route("[controller]/Login")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            try
            {
                var LoginResult = _login.UserLogin(login);

                if(LoginResult.isSuccessful && LoginResult.ErrorCode == "E000") return Ok(LoginResult);

                if(!LoginResult.isSuccessful && LoginResult.ErrorCode == "E100") return BadRequest(LoginResult);

                if(!LoginResult.isSuccessful && LoginResult.ErrorCode == "E200") return Unauthorized(LoginResult);

                if(!LoginResult.isSuccessful && LoginResult.ErrorCode == "E400") return BadRequest(LoginResult);

                return Unauthorized(LoginResult);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, ex.Message, ex.StackTrace);
                return BadRequest(new LoginResponse { Data = null, DevMessage = ex.Message, ErrorCode = "E400", StatusMessage = "Unable to login, please try again"});
            }
        }
    }
}

