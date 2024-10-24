using Firebase.Auth;
using RealtyMarket.Models;

namespace RealtyMarket.Service
{
    public class FirebaseAuthenticationService
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

        private readonly SecureStorageUserRepository _storageUserRepository;


        public FirebaseAuthenticationService(FirebaseAuthClient firebaseAuthClient
            ,SecureStorageUserRepository storageUserRepository)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _storageUserRepository = storageUserRepository;
        }

        public async Task<SignInResultEnum> RegisterUserAsync(string email, string password)
        {
            try
            {
                var userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
                User user = GetCurrentUser();
                await _storageUserRepository.SaveUser(user);
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    return SignInResultEnum.EmailIsBusy;
                }
                else
                {
                    return SignInResultEnum.InValidError;
                }
            }
            catch (Exception) { }
            return SignInResultEnum.Ok;
        }


        public async Task<SignInResultEnum> SignInUserAsync(string email, string password)
        {
            try
            {
                var userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                User user = GetCurrentUser();
                await _storageUserRepository.SaveUser(user);
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.Unknown)
                {
                    return SignInResultEnum.EmailNotExist;
                }
                else
                {
                    return SignInResultEnum.InValidError;
                }
            }
            catch (Exception) { }
            return SignInResultEnum.Ok;
        }

        public async Task<SignInResultEnum> SignInAnonymousUserAsync()
        {
            try
            {
                var userCredential = _firebaseAuthClient.SignInAnonymouslyAsync().Result;
                User user = GetCurrentUser();
                await _storageUserRepository.SaveUser(user);
                if (userCredential.User != null)
                {
                    return SignInResultEnum.Ok;
                }
            }
            catch (Exception) { }

            return SignInResultEnum.InValidError;
        }

        public async Task SignOutUser()
        {
            //_firebaseAuthClient.SignOut();

            var userStatus = await _storageUserRepository.GetUserState();
            _storageUserRepository.DeleteUser();
        }

        public User GetCurrentUser()
        {
            return _firebaseAuthClient.User;
        }
    }
}
