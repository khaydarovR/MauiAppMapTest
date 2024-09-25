using MauiAppMapTest.ViewModel;

namespace MauiAppMapTest.Page;

public partial class LoginPage : ContentPage
{
    private readonly LoginVM loginPage;

    public LoginPage(LoginVM loginPage)
	{
		InitializeComponent();
		BindingContext = loginPage;
        this.loginPage = loginPage;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoginVM.CheckJWT();
    }
}