using CommunityToolkit.Maui;
using GD.Shared.Common;
using MauiAppMapTest.Page;
using MauiAppMapTest.Services;
using MauiAppMapTest.ViewModel;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MauiAppMapTest
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                 .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginVM>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<GeoService>();
            builder.Services.AddTransient<DelivPage>();
            builder.Services.AddTransient<OrderService>();
            builder.Services.AddTransient<AccPage>();

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(Const.BASE_URL) });
            builder.Services.AddSingleton<HttpService, HttpService>();

            return builder.Build();
        }
    }
}

