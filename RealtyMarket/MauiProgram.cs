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
using CommunityToolkit.Maui;
using Firebase.Database;

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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Pavelt.ttf", "Pavelt");
                    fonts.AddFont("FuturaPrimer-Regular.ttf", "Futura");
                    fonts.AddFont("VeryBold-Bold.ttf", "VeryBold");

                    //Nexa Fonts
                    fonts.AddFont("Nexa Bold.ttf", "NexaBold");
                    fonts.AddFont("Nexa Light.ttf", "NexaLight");
                    fonts.AddFont("NexaDemo-Light.ttf", "NexaDemoLight");
                    fonts.AddFont("NexaTextDemo-Bold.ttf", "NexaTextDemoBold");
                    fonts.AddFont("NexaTextDemo-Light.ttf", "NexaTextDemoLight");
                    fonts.AddFont("NexaText-Trial-Black.ttf", "NexaTextTrialBlack");
                    fonts.AddFont("NexaText-Trial-BlackItalic.ttf", "NexaTextTrialBlackItalic");
                    fonts.AddFont("NexaText-Trial-Book.ttf", "NexaTextTrialBook");
                    fonts.AddFont("NexaText-Trial-BookItalic.ttf", "NexaTextTrialBookItalic");
                    fonts.AddFont("NexaText-Trial-ExtraBold.ttf", "NexaTextTrialExtraBold");
                    fonts.AddFont("NexaText-Trial-ExtraBoldItalic.ttf", "NexaTextTrialExtraBoldItalic");
                    fonts.AddFont("NexaText-Trial-Heavy.ttf", "NexaTextTrialHeavy");
                    fonts.AddFont("NexaText-Trial-HeavyItalic.ttf", "NexaTextTrialHeavyItalic");
                    fonts.AddFont("Nexa-Trial-Bold.ttf", "NexaTrialBold");
                    fonts.AddFont("Nexa-Trial-Book.ttf", "NexaTrialBook");
                    fonts.AddFont("Nexa-Trial-ExtraBold.ttf", "NexaTrialExtraBold");
                    fonts.AddFont("Nexa-Trial-ExtraBoldItalic.ttf", "NexaTrialExtraBoldItalic");
                    fonts.AddFont("Nexa-Trial-Heavy.ttf", "NexaTrialHeavy");
                    fonts.AddFont("Nexa-Trial-HeavyItalic.ttf", "NexaTrialHeavyItalic");
                });

            string key = "Ngo9BigBOggjHTQxAR8/V1NDaF5cWWtCf1JpR2FGfV5ycEVDalhYTnZZUj0eQnxTdEFiWX1dcXNQRmBaUk12Xg==";
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
            builder.Services.AddTransient<SecureStorageUserRepository>();

            builder.Services.AddSingleton(services => new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyDdGSLe9YGHgFlZfzj5NhlTgNkJ-jxXuxY",
                AuthDomain = "realtymarket-e4db0.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                },
            }));

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddTransient<ConnectivityService>();
            builder.Services.AddTransient<ConnectionLostPage>();

            builder.Services.AddSingleton<FirebaseAuthenticationService>();
            builder.Services.AddTransient<SecureStorageUserRepository>();

            builder.Services.AddTransient<LoginRegisterViewModel>();
            builder.Services.AddTransient<LoginRegisterPage>();

            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfilePage>();

            builder.Services.AddTransient<AddAdViewModel>();
            builder.Services.AddSingleton<AddAdPage>();

            builder.Services.AddTransient<CatalogViewModel>();
            builder.Services.AddTransient<CatalogPage>();

            builder.Services.AddTransient<MyAdPageViewModel>();
            builder.Services.AddTransient<MyAdPage>();

            builder.Services.AddTransient<UserSettingsViewModel>();
            builder.Services.AddTransient<UserSettingsPage>();

            builder.Services.AddTransient<FavoriteViewModel>();
            builder.Services.AddTransient<FavoritesPage>();

            builder.Services.AddTransient<AdvertisementViewModel>();
            builder.Services.AddTransient<AdvertisementPage>();

            builder.Services.AddScoped<RegisteredUserRepository>();
            builder.Services.AddScoped<AdvertisementRepository>();
            builder.Services.AddTransient<ImageBBRepository>();

            builder.Services.AddHttpClient<ImageBBRepository>();

            //SecureStorage.RemoveAll();

            builder.Services.AddSingleton<FirebaseClient>(sp =>
                new FirebaseClient("https://realtymarket-e4db0-default-rtdb.firebaseio.com/"));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
