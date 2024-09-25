using MauiAppMapTest.Page;

namespace MauiAppMapTest
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {

            InitializeComponent();
/*
            var mapControl = new Mapsui.UI.Maui.MapControl();
            var r = Mapsui.Tiling.OpenStreetMap.CreateTileLayer();
            mapControl.Map?.Layers.Add(r);
            Task.Delay(1000);
            Content = mapControl;
            Task.Delay(1000);*/
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void OnClickToPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PosPage());

        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            if (await isAuthenticated())
            {
                await Shell.Current.GoToAsync("///home");
            }
            else
            {
                await Shell.Current.GoToAsync("login");
            }
            base.OnNavigatedTo(args);
        }

        async Task<bool> isAuthenticated()
        {
            var hasAuth = await SecureStorage.GetAsync("JWT");
            return !(hasAuth == null);
        }
    }

}
