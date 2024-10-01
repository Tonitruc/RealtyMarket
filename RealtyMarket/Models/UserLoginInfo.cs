using MvvmHelpers;

namespace RealtyMarket.Models
{
    public class UserLoginInfo : ObservableObject
    {
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}
