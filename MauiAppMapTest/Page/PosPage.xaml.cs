using MauiAppMapTest.ViewModel;

namespace MauiAppMapTest.Page;

public partial class PosPage : ContentPage
{
    PosViewModel viewModel;

    public PosPage()
	{
        InitializeComponent();
        viewModel = new PosViewModel();
        BindingContext = viewModel;

        Resources.Add("PosViewModel", viewModel);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Connect();
    }

    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await viewModel.Disconnect();
    }
}