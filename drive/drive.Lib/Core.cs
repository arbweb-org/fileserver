using System.Text.RegularExpressions;

namespace drive.Lib
{
    public static class Core
    {
        public static Boolean IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) { return false; }

            // Use Regex to validate the email format
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
            return emailRegex.IsMatch(email);
        }

        public static Boolean IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) { return false; }
            if (password.Length > 16) { return false; }

            // Password must be at least 8 characters long, contain at least one uppercase letter,
            // one lowercase letter, one digit, and one special character.
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]{8,}$";
            var passwordRegex = new Regex(pattern, RegexOptions.Compiled);
            return passwordRegex.IsMatch(password);
        }
    }
}