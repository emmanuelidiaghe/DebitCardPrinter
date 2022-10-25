using NotificationService.Notification.Domain.Entities.SendEmail;

namespace NotificationService.Notification.Domain.Repository
{
    public interface ISendEmail
    {
        public Task<string> SendMail(SendEmailRequest sendemail);
    }
}

