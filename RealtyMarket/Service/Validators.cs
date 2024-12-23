﻿using Microsoft.Maui.ApplicationModel.Communication;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace RealtyMarket.Service
{
    public static class Validators
    {
        [DllImport("libmylibrary.so", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IsValidEmail(int param);


        private const string NumberPattern = @"^\+375 \(\d{2}\) \d{3}-\d{2}-\d{2}$";

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return Regex.IsMatch(phoneNumber, NumberPattern);
        }

    }
}
