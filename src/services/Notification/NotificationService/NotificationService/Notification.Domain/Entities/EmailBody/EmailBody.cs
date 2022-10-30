using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;

namespace NotificationService.Notification.Domain.Entities.EmailBody
{
    public class EmailBody
    {
        private readonly string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

        public string SetEmailBody(string message)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "providus.html");
            StreamReader reader = new(path); 

            string readFile = reader.ReadToEnd();
            string myString = readFile;

            myString = myString.Replace("textToBeReplaced", message);

            return myString;
        }

        public bool IsValidEmail(List<EmailAddress> addresses)
        {
            int count = 0; bool[] result = new bool[addresses.Count];
            addresses.ForEach(item => result[count++] = Regex.IsMatch(item.Email, pattern));

            foreach (var resp in result) if (!resp) return false;
            return true;
        }
    }
}