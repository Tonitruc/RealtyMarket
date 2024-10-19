using Firebase.Database;
using Firebase.Database.Query;
using RealtyMarket.Models.OtherEntity;
using RealtyMarket.Models.RealtyEntity;
using RealtyMarket.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RealtyMarket.Repository
{
    public class RealtyRepository : IServerRepository<Realty>
    {
        private readonly FirebaseClient _firebaseClient;

        public override string Controller => "realty";

        public RealtyRepository(FirebaseClient fireBase)
        {
            _firebaseClient = fireBase;
        }

        public async Task<bool> Add(Realty entity)
        {
            try
            {
                var entityType = entity.GetType().Name;
                await _firebaseClient
                    .Child(Controller)
                    .Child(entityType)
                    .PostAsync(entity);

                return true;
            }
            catch (Exception) { }

            return false;
        }

        public async Task<Realty> GetById(string id)
        {
            try
            {
                var flat = (await _firebaseClient
                    .Child(Controller)
                    .Child("flat")
                    .OnceAsync<Flat>())
                    .FirstOrDefault(x => x.Key == id)?.Object;

                if (flat != null)
                {
                    return flat;
                }
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
