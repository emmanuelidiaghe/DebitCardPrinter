using NotificationService.Notification.Domain.Entities.SendEmail;
using NotificationService.Notification.Domain.Entities.EmailBody;
using SendGrid;
using System.Net;
using SendGrid.Helpers.Mail;
using NotificationService.Notification.Domain.Repository;

namespace NotificationService.Notification.Infrastructure.RepositoryImpl
{
    public class SendEmail : ISendEmail
    {
        private readonly IConfiguration _configuration;
        private readonly EmailBody emailBody = new ();

        public SendEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static bool IsEmailDuplicate(List<EmailAddress> addresses)
        {
            if (addresses.GroupBy(item => item.Email).Any(item2 => item2.Count() > 1)) return true;
            return false;
        }

        public async Task<string> SendMail(SendEmailRequest request)
        {
            try
            {
                string base64 = _configuration["Key:Base64"];
                string ccAddress = _configuration["Key:CopyAddress"];
                string fromName = _configuration["Key:FromName"];
                string fromAddress = _configuration["Key:FromAddress"];
                string SubjectName = _configuration["Key:SubjectName"];
                string AzureSendGridKey = _configuration["Key:AzureSendGridKey"];

                var client = new SendGridClient(AzureSendGridKey);
                var msg = new SendGridMessage();
                var ToRecipients = new List<EmailAddress>();
                var CcRecipients = new List<EmailAddress>();
                var BccRecipients = new List<EmailAddress>();
                var AllAddresses = new List<EmailAddress>();

                msg.SetFrom(new EmailAddress(fromAddress, fromName));

                if (request.ToRecipients != null)
                {
                    for (int index = 0; index < request.ToRecipients.Count; index++)
                    {
                        ToRecipients.Add(new EmailAddress(request.ToRecipients[index].ToAddress, request.ToRecipients[index].ToName));
                    }
                }

                if (request.CcRecipients != null)
                {
                    for (int index = 0; index < request.CcRecipients.Count; index++)
                    {
                        CcRecipients.Add(new EmailAddress(request.CcRecipients[index].ToAddress, request.CcRecipients[index].ToName));
                    }
                }

                if (request.BccRecipients != null)
                {
                    for (int index = 0; index < request.BccRecipients.Count; index++)
                    {
                        BccRecipients.Add(new EmailAddress(request.BccRecipients[index].ToAddress, request.BccRecipients[index].ToName));
                    }
                }

                BccRecipients.Add(new EmailAddress(ccAddress, fromName));

                if (ToRecipients != null) ToRecipients.ForEach(item => AllAddresses.Add(item));
                if (BccRecipients != null) BccRecipients.ForEach(item => AllAddresses.Add(item));
                if (CcRecipients != null) CcRecipients.ForEach(item => AllAddresses.Add(item));
                if (!emailBody.IsValidEmail(AllAddresses)) return "E100";//Invalid email address(es)
                if (IsEmailDuplicate(AllAddresses)) return "E300";//Validation for duplicate email addresses

                if (CcRecipients != null) if (CcRecipients.Count > 0) msg.AddCcs(CcRecipients);
                msg.AddTos(ToRecipients); //a recipient sees other recipients
                msg.AddBccs(BccRecipients);
                msg.SetSubject(request.Subject);

                if (!string.IsNullOrEmpty(request.EmailType) && !string.IsNullOrEmpty(request.Body))
                {
                    if (request.EmailType.ToLower() == "html") msg.AddContent(MimeType.Html, emailBody.SetEmailBody(request.Body));
                    else if (request.EmailType.ToLower() == "text") msg.AddContent(MimeType.Text, request.Body);
                    else return "E200";//choose email type - html or text
                }

                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);//Hit SendGrid system
                if (response.IsSuccessStatusCode && emailBody.IsValidEmail(AllAddresses)) return "E000";//Successful request

                return "E500" + " " + $"Response: Status Code ==> {response.StatusCode}, Body ==> {response.Body.Headers}";//Uncaught exception
            }

            catch (Exception e)
            {
                return "E400" + " " + e.Message;//For developer's use
            }
        }
    }
}