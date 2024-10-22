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

        public async Task UpdateUser(RegisteredUser user, string userId)
        {
            try
            {
                await _firebaseClient
                    .Child(Controller)
                    .Child(userId)
                    .PutAsync(user);
            }
            catch (Exception) { }
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

                var userEntity = users.FirstOrDefault();
                RegisteredUser user = userEntity.Object;
                user.Id = userEntity.Key;
                return user;
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

        public async Task AddFavorite(string email, string adId)
        {
            var user = await GetByEmail(email);

            user.Favorites.Add(adId);

            await _firebaseClient
                    .Child(Controller)
                    .Child(user.Id)
                    .PutAsync(new RegisteredUser()
                    {
                        UserImageUrl = user.UserImageUrl,
                        Password = user.Password,
                        Name = user.Name,
                        Email = user.Email,
                        Favorites = user.Favorites
                    });
        }

        public async Task DeleteFavorite(string email, string adId)
        {
            var user = await GetByEmail(email);

            user.Favorites.Remove(adId);

            await _firebaseClient
                    .Child(Controller)
                    .Child(user.Id)
                    .PutAsync(new RegisteredUser()
                    {
                        UserImageUrl = user.UserImageUrl,
                        Password = user.Password,
                        Name = user.Name,
                        Email = user.Email,
                        Favorites = user.Favorites
                    });
        }
    }
}
