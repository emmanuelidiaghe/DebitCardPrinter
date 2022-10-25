using System;

namespace NotificationService.Notification.Domain.Entities.SendEmail
{
    public class SendEmailResponse
    {
        public SendEmailResponse()
        {
        }

        public string? ErrorCode { get; set; }
        public string? StatusMessage { get; set; }
        public string? DevMessage { get; set; }
    }
}