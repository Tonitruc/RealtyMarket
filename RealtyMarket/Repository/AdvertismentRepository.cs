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
using System.Runtime.InteropServices;
using Microsoft.Maui.ApplicationModel.Communication;

namespace RealtyMarket.Repository
{
    public class AdvertisementRepository : IServerRepository<Advertisement>
    {
        private readonly FirebaseClient _firebaseClient;

        public override string Controller => "advertisement";

        public AdvertisementRepository(FirebaseClient fireBase)
        {
            _firebaseClient = fireBase;
        }

        public async Task<bool> Add(Advertisement entity, bool active = true)
        {
            var status = active ? "active" : "closed";

            try
            {
                string type = "realty";
                if (entity.Realty is Flat)
                {
                    type = "flat";
                }
                else if (entity.Realty is PrivateHouse)
                {
                    type = "privateHouse";
                }

                await _firebaseClient
                    .Child(Controller)
                    .Child(status)
                    .Child(type)
                    .PostAsync(entity);

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public async Task<List<Advertisement>> GetByEmail(string email, bool active = true)
        {
            try
            {
                var result = await GetAll(active);

                return result
                    .Select(item => item)
                    .Where(ad => ad.UserEmail == email)
                    .ToList();
            }
            catch (Exception) { }

            return null;
        }

        public async Task<List<Advertisement>> GetFavorites(RegisteredUser user)
        {
            var result = await GetAll();

            return result
                .Select(item => item)
                .Where(ad => user.Favorites.Contains(ad.Id))
                .ToList();
        }

        public async Task<List<Advertisement>> GetAll(bool active = true)
        {
            var advertisements = new List<Advertisement>();
            var status = active ? "active" : "closed";

            var firebaseAds = await _firebaseClient
                .Child(Controller)
                .Child(status)
                .OnceAsync<Dictionary<string, object>>();

            foreach (var items in firebaseAds)
            {
                foreach (var item in items.Object)
                {
                    var jsonString = JsonConvert.SerializeObject(item.Value);
                    JObject rootObject = JObject.Parse(jsonString);

                    Advertisement ad = JsonConvert.DeserializeObject<Advertisement>(rootObject.ToString(), new AdvertisementJsonConverter());
                    ad.Id = item.Key;

                    advertisements.Add(ad);
                }
            }

            return advertisements;
        }

        public async Task AdStatusChange(Advertisement ad, bool close = true)
        {
            var newStatus = close ? "closed" : "active";
            var oldStatus = close ? "active" : "closed";
            var category = ad.RealtyCategory;

            if (category == "Квартира")
            {
                category = "flat";
            }
            else if (category == "Дом")
            {
                category = "privateHouse";
            }

            await _firebaseClient
                .Child(Controller)
                .Child(newStatus)
                .Child(category)
                .Child(ad.Id)
                .PutAsync(ad);

            await _firebaseClient
                .Child(Controller)
                .Child(oldStatus)
                .Child(category)
                .Child(ad.Id)
                .DeleteAsync();
        }

        public async Task DeleteAd(Advertisement ad, bool active = true)
        {
            var status = !active ? "closed" : "active";
            var category = ad.RealtyCategory;

            if (category == "Квартира")
            {
                category = "flat";
            }
            else if (category == "Дом")
            {
                category = "privateHouse";
            }

            await _firebaseClient
                 .Child(Controller)
                 .Child(status)
                 .Child(category)
                 .Child(ad.Id)
                 .DeleteAsync();
        }
    }
}
