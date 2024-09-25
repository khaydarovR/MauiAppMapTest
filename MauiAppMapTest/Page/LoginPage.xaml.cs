using MauiAppMapTest.ViewModel;

namespace MauiAppMapTest.Page;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginVM? loginPage)
	{
		InitializeComponent();
		BindingContext = loginPage;
	}
}