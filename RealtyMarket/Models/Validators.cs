using Microsoft.Maui.ApplicationModel.Communication;
using System.Runtime.InteropServices;
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

        private const string NumberPattern = @"^\+375 \(\d{2}\) \d{3}-\d{2}-\d{2}$";

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, NumberPattern);
        }

    }
}
