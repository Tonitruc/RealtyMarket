using Firebase.Database;
using RealtyMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using RealtyMarket.Models.RealtyEntity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtyMarket.Converters;

namespace RealtyMarket.Repository
{
    public class AdvertisementRepository : IServerRepository<Advertisement>
    {
        private readonly FirebaseClient _firebaseClient;

        private readonly RealtyRepository _realtyRepository;

        public override string Controller => "advertisement";

        public AdvertisementRepository(FirebaseClient fireBase, 
            RealtyRepository realtyRepository)
        {
            _realtyRepository = realtyRepository;
            _firebaseClient = fireBase;
        }

        public async Task<bool> Add(Advertisement entity)
        {
            try
            {
                string type = "realty";
                if(entity.Realty is Flat)
                {
                    type = "flat";
                }
                else if(entity.Realty is PrivateHouse)
                {
                    type = "privateHouse";
                }

                await _firebaseClient
                    .Child(Controller)
                    .Child(type)
                    .PostAsync(entity);

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public async Task<List<Advertisement>> GetByEmail(string email)
        {
            try
            {
                var result = await GetAll();

                return result
                    .Select(item => item)
                    .Where(ad => ad.UserEmail == email)
                    .ToList();
            }
            catch (Exception) { }

            return null;
        }

        public async Task<List<Advertisement>> GetAll()
        {
            var advertisements = new List<Advertisement>();

            var firebaseAds = await _firebaseClient
                .Child(Controller)
                .OnceAsync<Dictionary<string, object>>();

            foreach (var item in firebaseAds)
            {
                var jsonString = JsonConvert.SerializeObject(item.Object);
                JObject rootObject = JObject.Parse(jsonString);

                var firstProperty = rootObject.Properties().FirstOrDefault();

                if (firstProperty != null)
                {
                    JObject jsonObject = (JObject)firstProperty.Value;

                    Advertisement ad = JsonConvert.DeserializeObject<Advertisement>(jsonObject.ToString(), new AdvertisementJsonConverter());

                    advertisements.Add(ad);
                }
            }

            return advertisements;
        }
    }
}
