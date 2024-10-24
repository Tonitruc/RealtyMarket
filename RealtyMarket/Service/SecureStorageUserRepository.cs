using Firebase.Auth;
using Firebase.Auth.Repository;
using System.Text.Json;

namespace RealtyMarket.Service
{
    public class SecureStorageUserRepository
    {
        public const string CurrentUserInfoKey = "USER_KEY";
        public const string CurrentUserCredentialKey = "CREDENTIAL_KEY";
        public const string CurrentUserStateKey = "USER_STATE_KEY";

        public async Task<(UserInfo userInfo, FirebaseCredential credential)> ReadUser()
        {
            try
            {
                string userInfoJson = await SecureStorage.GetAsync(CurrentUserInfoKey);
                string credntialJson = await SecureStorage.GetAsync(CurrentUserCredentialKey);

                UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson);
                FirebaseCredential credential = JsonSerializer.Deserialize<FirebaseCredential>(credntialJson);

                return (userInfo, credential);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        public async Task SaveUser(User user)
        {
            try
            {
                var userInfoJson = JsonSerializer.Serialize(user.Info);
                var cedentialJson = JsonSerializer.Serialize(user.Credential);

                await SecureStorage.SetAsync(CurrentUserInfoKey, userInfoJson);
                await SecureStorage.SetAsync(CurrentUserCredentialKey, cedentialJson);

                if(user.IsAnonymous)
                {
                    await SecureStorage.SetAsync(CurrentUserStateKey, "Guest");
                }
                else
                {
                    await SecureStorage.SetAsync(CurrentUserStateKey, "Register");
                }
            }
            catch (Exception) { }
        }

        public void DeleteUser()
        {
            SecureStorage.Remove(CurrentUserInfoKey);
            SecureStorage.Remove(CurrentUserCredentialKey);
            SecureStorage.Remove(CurrentUserStateKey);
        }

        public async Task<bool> UserExists()
        {
            var userInfo = await ReadUser();
            if(userInfo.Equals((null, null)))
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetUserState()
        {
            return await SecureStorage.GetAsync(CurrentUserStateKey);
        }
    }
}
