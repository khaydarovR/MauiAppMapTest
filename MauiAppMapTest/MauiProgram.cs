using CommunityToolkit.Maui;
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
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<GeoService>();
            builder.Services.AddTransient<DelivPage>();
            builder.Services.AddTransient<OrderService>();

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://gd9b3wbt-7265.euw.devtunnels.ms/") });
            builder.Services.AddSingleton<HttpService, HttpService>();

            return builder.Build();
        }
    }
}
