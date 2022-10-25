using System.ComponentModel.DataAnnotations;

namespace NotificationService.Notification.Domain.Entities.SendEmail
{
    public class SendEmailRequest
    {
        [Required]
        public string? EmailType { get; set; }

        [Required]
        public List<Recipients>? ToRecipients { get; set; }

        public List<Recipients>? CcRecipients { get; set; }

        public List<Recipients>? BccRecipients { get; set; }

        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Body { get; set; }
    }

    public class Recipients
    {
        [Required]
        public string? ToAddress { get; set; }

        public string? ToName { get; set; }
    }
}