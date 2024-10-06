using Microsoft.Maui.Controls.Platform;
using RealtyMarket.Controls;
using RealtyMarket.Service;
using RealtyMarket.Views;

namespace RealtyMarket
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage = appShell;

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

        }
    }
}
