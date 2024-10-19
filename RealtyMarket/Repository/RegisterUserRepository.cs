using Firebase.Database;
using Microsoft.Maui.ApplicationModel.Communication;
using RealtyMarket.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Firebase.Database.Query;

namespace RealtyMarket.Repository
{
    public class RegisteredUserRepository : IServerRepository<RegisteredUser>
    {
        private readonly FirebaseClient _firebaseClient;

        public override string Controller => "registeredUsers";


        public RegisteredUserRepository(FirebaseClient fireBase)
        {
            _firebaseClient = fireBase;
        }

        public async Task<RegisteredUser> GetByEmail(string email)
        {
            try
            {
                var users = await _firebaseClient
                                    .Child(Controller)
                                    .OrderBy("Email")
                                    .EqualTo(email)
                                    .OnceAsync<RegisteredUser>();

                return users.FirstOrDefault()?.Object;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public Task<bool> GetAll()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> Add(RegisteredUser entity)
        {
            await _firebaseClient.Child(Controller).PostAsync(entity);
            return true;
        }
    }
}
