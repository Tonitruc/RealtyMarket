using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtyMarket.Models;
using RealtyMarket.Models.RealtyEntity;

namespace RealtyMarket.Converters
{
    public class AdvertisementJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Advertisement);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var category = jsonObject["RealtyCategory"].ToString();

            Advertisement ad = new();

            if (category == "Квартира")
            {
                ad.Realty = new Flat();
            }
            else if (category == "Дом")
            {
                ad.Realty = new PrivateHouse();
            }

            serializer.Populate(jsonObject.CreateReader(), ad);

            return ad;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
