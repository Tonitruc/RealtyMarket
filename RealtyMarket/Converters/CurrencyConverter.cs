using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RealtyMarket.Converters
{
    public static class CurrencyConverter
    {
        private const string ApiKey = "0563713728a35819b6f3587f7c4d67b4";

        private static string GetUrl(string sourceCurrency, string destinationCurrency)
        {
            return $"http://apilayer.net/api/live?access_key={ApiKey}" +
            $"&currencies={destinationCurrency}&source={sourceCurrency}&format=1";
        }

        public static async Task<double> USDToBYN(double usd)
        {
            string url = GetUrl("USD", "BYN");

            double BYN = 3.27;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var jsonObject = JObject.Parse(json);

                    var quotes = jsonObject["quotes"] as JObject;

                    BYN = quotes.First.First.Value<double>();
                }
            }
            catch (HttpRequestException) { }
            catch (Exception) { }

            return usd * BYN;
        }

        public static async Task<double> BYNToUSD(double byn)
        {
            string url = GetUrl("BYN", "USD");

            double USD = 0.30;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    var jsonObject = JObject.Parse(json);

                    var quotes = jsonObject["quotes"] as JObject;

                    USD = quotes.First.First.Value<double>();
                }
            }
            catch (HttpRequestException) { }
            catch (Exception) { }

            return byn * USD;
        }
    }
}
