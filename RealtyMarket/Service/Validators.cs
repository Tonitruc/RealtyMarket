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

        [DllImport("pmc.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern int is_valid_phone(string phone);

        public static bool IsPhoneValid(string phoneNumber)
        {
            return is_valid_phone(phoneNumber) == 1;
        }

    }
}
