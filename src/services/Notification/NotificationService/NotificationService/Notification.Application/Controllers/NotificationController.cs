using Microsoft.AspNetCore.Mvc;
using NotificationService.Notification.Domain.Repository;
using NotificationService.Notification.Domain.Entities.SendEmail;

namespace NotificationService.Controllers
{
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly ISendEmail _sendEmail;
        private readonly IWebHostEnvironment webHostEnvironment1;

        public NotificationController(ISendEmail sendEmail, IWebHostEnvironment webHostEnvironment)
        {
            _sendEmail = sendEmail;
            webHostEnvironment1 = webHostEnvironment;
        }

        [HttpPost, Route("[controller]/SendEmail")]
        public async Task<IActionResult> SendEmailPlainText([FromBody] SendEmailRequest request)
        {
            try
            {
                var result = await _sendEmail.SendMail(request);
                return result[..4] switch
                {
                    "E000" => Ok(new SendEmailResponse { ErrorCode = result, StatusMessage = "Message sent successfully", DevMessage = "" }),
                    "E100" => BadRequest(new SendEmailResponse { ErrorCode = result, StatusMessage = "Invalid email address(es)", DevMessage = "" }),
                    "E200" => BadRequest(new SendEmailResponse { ErrorCode = result, StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = "EmailType property must either be HTML or TEXT" }),
                    "E300" => BadRequest(new SendEmailResponse { ErrorCode = result, StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = "Duplicate email addresses" }),
                    "E400" => BadRequest(new SendEmailResponse { ErrorCode = result[..4], StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = result[4..] }),
                    "E500" => BadRequest(new SendEmailResponse { ErrorCode = result[..4], StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = result[4..] }),
                    _ => BadRequest(new SendEmailResponse { ErrorCode = result, StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = "" }),
                };
            }

            catch (Exception ex)
            {
                return BadRequest(new SendEmailResponse { ErrorCode = "E400", StatusMessage = "Message cannot be sent at the moment. Please try again", DevMessage = ex.Message });
            }
        }
    }
}