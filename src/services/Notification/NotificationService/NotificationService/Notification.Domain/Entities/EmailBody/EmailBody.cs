using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;

namespace NotificationService.Notification.Domain.Entities.EmailBody
{
    public class EmailBody
    {
        private readonly string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        //private IWebHostEnvironment _hostEnvironment;

        //public EmailBody(IWebHostEnvironment webHostEnvironment)
        //{
        //    _hostEnvironment = webHostEnvironment;
        //}

        public string SetEmailBody()
        {

            //string path = Path.Combine(_hostEnvironment.ContentRootPath, "Notification.Domain/Entities/EmailBody/providus.html");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Notification.Domain/Entities/EmailBody/providus.html");
            StreamReader reader = new(path); 

            string readFile = reader.ReadToEnd();
            string myString = readFile;

            //myString = myString.Replace("", "");
            //myString = myString.Replace("", "");
            //myString = myString.Replace("", "");

            return myString;
        }

        public bool ValidateAddress(List<EmailAddress> addresses)
        {
            int count = 0; bool[] result = new bool[addresses.Count];
            addresses.ForEach(item => result[count++] = Regex.IsMatch(item.Email, pattern));

            foreach (var resp in result) if (!resp) return false;
            return true;
        }
    }
}