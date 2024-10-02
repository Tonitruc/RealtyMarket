using Firebase.Auth;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Firebase.Auth.Providers;
using RealtyMarket.Service;
using RealtyMarket.ViewModels;
using RealtyMarket.Views;
using Firebase.Auth.Repository;
using Microsoft.Extensions.Http;
using RealtyMarket.Repository;

namespace RealtyMarket
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("NexaDemo-Bold.ttf", "NexaDemoBold");
                    fonts.AddFont("Pavelt.ttf", "Pavelt");
                });

            builder.Services.AddSingleton<IUserRepository, SecureStorageUserRepository>();

            builder.Services.AddSingleton(services => new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyDdGSLe9YGHgFlZfzj5NhlTgNkJ-jxXuxY",
                AuthDomain = "realtymarket-e4db0.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
                UserRepository = services.GetRequiredService<IUserRepository>()
            }));

            builder.Services.AddSingleton<FirebaseAuthenticationService>();
            builder.Services.AddTransient<SecureStorageUserRepository>();

            builder.Services.AddTransient<LoginRegisterViewModel>();
            builder.Services.AddTransient<LoginRegisterPage>();

            builder.Services.AddHttpClient<RegisteredUserRepository>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
