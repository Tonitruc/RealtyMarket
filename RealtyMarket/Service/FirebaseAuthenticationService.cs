using Firebase.Auth;
using RealtyMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyMarket.Service
{
    public class FirebaseAuthenticationService
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

        private readonly SecureStorageUserRepository _storageUserRepository;


        public FirebaseAuthenticationService(FirebaseAuthClient firebaseAuthClient
            , SecureStorageUserRepository storageUserRepository)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _storageUserRepository = storageUserRepository;
        }

        public async Task<SingInResultEnum> RegisterUserAsync(string email, string password)
        {
            try
            {
                var userCredential = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);
                User user = GetCurrentUser();
                _storageUserRepository.SaveUser(user);
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.EmailExists)
                {
                    return SingInResultEnum.EmailIsBusy;
                }
                else
                {
                    return SingInResultEnum.InValidError;
                }
            }
            catch (Exception) { }
            return SingInResultEnum.Ok;
        }


        public async Task<SingInResultEnum> SignInUserAsync(string email, string password)
        {
            try
            {
                var userCredential = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(email, password);
                User user = GetCurrentUser();
                _storageUserRepository.SaveUser(user);
            }
            catch (FirebaseAuthException ex)
            {
                if (ex.Reason == AuthErrorReason.Unknown)
                {
                    return SingInResultEnum.EmailNotExist;
                }
                else
                {
                    return SingInResultEnum.InValidError;
                }
            }
            catch (Exception) { }
            return SingInResultEnum.Ok;
        }


        public void SignOutUser()
        {
            _firebaseAuthClient.SignOut();
            _storageUserRepository.DeleteUser();
        }

        public User GetCurrentUser()
        {
            return _firebaseAuthClient.User;
        }
    }
}
