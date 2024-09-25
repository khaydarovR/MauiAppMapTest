using MauiAppMapTest.Infra;
using MauiAppMapTest.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading;
using MauiAppMapTest.Page;


namespace MauiAppMapTest.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
        private const string JwtKey = "JWT";
        private readonly HttpService http;

        public LoginVM(HttpService http)
        {
            this.http = http;

            LoginCommand = new Command(async () => await LoginBtn_Clicked());

        }

        public static void CheckJWT()
        {
            var jwt = Preferences.Get(JwtKey, null);
            if (jwt != null)
            {
                GoBack();
            }
        }

        private static async Task GoBack()
        {
            var r = await Shell.Current.Navigation.PopModalAsync();
            Shell.Current.Navigation.RemovePage(r);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        string email;
        string pwd;

        public string Pwd
        {
            get => pwd;
            set
            {
                pwd = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; private set; }

        async Task LoginBtn_Clicked()
        {
            await SendLoginRequest();


        }

        private async Task SendLoginRequest()
        {
            var e  = Uri.EscapeDataString(Email);
            var p = Uri.EscapeDataString(Pwd);
            var res = await http.Get<SignInResponse>($"api/Auth/SignIn?Email={e}&Pwd={p}");
            if (res.IsSuccess)
            {
                //await App.Current.MainPage.DisplayAlert("Ответ", res.Data.Jwt,"OK");
                Preferences.Set(JwtKey, res.Data!.Jwt);
                await GoBack();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ответ", res.ErrorList.Messages.FirstOrDefault(), "OK");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
