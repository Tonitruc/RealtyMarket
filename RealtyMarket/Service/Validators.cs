using System.Text.RegularExpressions;

namespace RealtyMarket.Service
{
    public static class Validators
    {
        public const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return Regex.IsMatch(email, EmailPattern);
        }

    }
}
