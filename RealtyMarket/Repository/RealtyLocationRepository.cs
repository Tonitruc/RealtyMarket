using RealtyMarket.Models.OtherEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace RealtyMarket.Repository
{
    public class RealtyLocationRepository : IServerRepository<RealtyLocation>
    {
        private readonly FirebaseClient _firebaseClient;

        public override string Controller => "location";


        public RealtyLocationRepository(FirebaseClient fireBase)
        {
            _firebaseClient = fireBase;
        }

        public async Task<RealtyLocation> Add(RealtyLocation entity)
        {
            try
            {
                var result = await _firebaseClient
                                .Child(Controller)
                                .PostAsync(entity);

                return result.Object;
            }
            catch (Exception) { }

            return null;
        }

        public async Task<RealtyLocation> GetById(string id)
        {
            try
            {
                var advertisement = (await _firebaseClient
                    .Child(Controller)
                    .OnceAsync<RealtyLocation>())
                    .FirstOrDefault(x => x.Key == id)?.Object;

                return advertisement;
            }
            catch (Exception) { }

            return null;
        }

        public async Task<RealtyLocation> GetByAddress(string address)
        {
            try
            {
                var location = (await _firebaseClient
                    .Child(Controller)
                    .OnceAsync<RealtyLocation>())
                    .FirstOrDefault(x => x.Object.Address == address)?.Object;

                return location;
            }
            catch (Exception) { }

            return null;
        }

        public Task<bool> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
