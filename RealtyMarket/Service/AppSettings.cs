using RealtyMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Service
{
    public static class AppSettings
    {
        public const string Name = "RealtyMarket";

        public readonly static string BaseApiUri = "https://realtymarket-e4db0-default-rtdb.firebaseio.com";
    }
}
