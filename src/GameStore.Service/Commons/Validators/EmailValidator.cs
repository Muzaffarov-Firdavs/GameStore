using System.Text.RegularExpressions;

namespace GameStore.Service.Commons.Validators
{
    public class EmailValidator 
    {
        public static bool IsValid(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
    }
}
