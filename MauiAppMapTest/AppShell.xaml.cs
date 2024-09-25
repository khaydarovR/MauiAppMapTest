using MauiAppMapTest.Page;
using MauiAppMapTest.Services;
using MauiAppMapTest.ViewModel;
using Microsoft.Extensions.Logging;

namespace MauiAppMapTest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Register all routes
            //Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("home", typeof(HomePage));
            Routing.RegisterRoute("settings", typeof(SettingsPage));
            Routing.RegisterRoute("deliv", typeof(DelivPage));
            Routing.RegisterRoute("acc", typeof(AccPage));
            Routing.RegisterRoute("login", typeof(LoginPage));        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.Current.GoToAsync("//login");
        }
    }
}
