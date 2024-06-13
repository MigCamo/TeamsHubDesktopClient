namespace ItalianPizza.Auxiliary
{
    public class RegexChecker
    {
        public static bool CheckEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        public static bool CheckPhoneNumber(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^(\d{10})$");
        }

        public static bool CheckPassword(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
        }

        public static bool CheckName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]{1,40}$");
        }

        public static bool CheckLastName(string lastName)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(lastName, @"^[a-zA-Z\s]{1,40}$");
        }

        public static bool CheckSecondLastName(string secondLastName)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(secondLastName, @"^[a-zA-Z\s]{1,40}$");
        }

        public static bool CheckUserName(string userName)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(userName, @"^[a-zA-Z0-9]{1,40}$");
        }
    }
}
