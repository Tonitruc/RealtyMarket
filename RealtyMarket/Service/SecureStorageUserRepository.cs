using Firebase.Auth;
using Firebase.Auth.Repository;
using System.Text.Json;

namespace RealtyMarket.Service
{
    public class SecureStorageUserRepository : IUserRepository
    {
        public const string CurrentUserInfoKey = "USER_KEY";
        public const string CurrentUserCredentialKey = "CREDENTIAL_KEY";
        public const string CurrentUserStateKey = "USER_STATE_KEY";

        public (UserInfo userInfo, FirebaseCredential credential) ReadUser()
        {
            try
            {
                string userInfoJson = Task.Run(() => SecureStorage.Default.GetAsync(CurrentUserInfoKey)).Result;
                string credntialJson = Task.Run(() => SecureStorage.Default.GetAsync(CurrentUserCredentialKey)).Result;

                UserInfo userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson);
                FirebaseCredential credential = JsonSerializer.Deserialize<FirebaseCredential>(credntialJson);

                return (userInfo, credential);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }

        public void SaveUser(User user)
        {
            try
            {
                var userInfoJson = JsonSerializer.Serialize(user.Info);
                var cedentialJson = JsonSerializer.Serialize(user.Credential);

                Task.Run(() => SecureStorage.Default.SetAsync(CurrentUserInfoKey, userInfoJson));
                Task.Run(() => SecureStorage.Default.SetAsync(CurrentUserCredentialKey, cedentialJson));

                if(user.IsAnonymous)
                {
                    Task.Run(() => SecureStorage.Default.SetAsync(CurrentUserStateKey, "Guest"));
                }
                else
                {
                    Task.Run(() => SecureStorage.Default.SetAsync(CurrentUserStateKey, "Register"));
                }
            }
            catch (Exception) { }
        }

        public void DeleteUser()
        {
            SecureStorage.Default.Remove(CurrentUserInfoKey);
            SecureStorage.Default.Remove(CurrentUserCredentialKey);
            SecureStorage.Default.Remove(CurrentUserStateKey);
        }

        public bool UserExists()
        {
            var userInfo = ReadUser();
            if(userInfo.Equals((null, null)))
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetUserState()
        {
            return await SecureStorage.Default.GetAsync(CurrentUserStateKey);
        }
    }
}
