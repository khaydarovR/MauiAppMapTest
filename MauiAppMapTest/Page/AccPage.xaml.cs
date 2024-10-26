using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Maui.Core.Extensions;
using MauiAppMapTest.DTO;
using MauiAppMapTest.Services;

namespace MauiAppMapTest.Page;

public partial class AccPage : ContentPage, INotifyPropertyChanged
{
	private string _email;
	private string _role;
	private string _id;

	public ObservableCollection<CStatResponse> Stat { get; set; } = new ObservableCollection<CStatResponse>();

	private readonly OrderService orderService;

	public string Email
	{
		get { return _email; }
		set
		{
			if (_email != value)
			{
				_email = value;
				OnPropertyChanged();
			}
		}
	}

	public string Role
	{
		get { return _role; }
		set
		{
			if (_role != value)
			{
				_role = value;
				OnPropertyChanged();
			}
		}
	}

	public string Id
	{
		get { return _id; }
		set
		{
			if (_id != value)
			{
				_id = value;
				OnPropertyChanged();
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public AccPage(OrderService orderService)
	{
		InitializeComponent();
		BindingContext = this;
		AuthProvider.JwtSetted += UpdateUserData;
		this.orderService = orderService;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		UpdateUserData();
	}

	private void UpdateUserData()
	{
		Email = AuthProvider.UserToken.email;
		Role = AuthProvider.UserToken.roles;
		Id = AuthProvider.UserToken.id.ToString();
		GetStatCourier();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Preferences.Clear();
		var w = App.Current.Windows.ToList();

        foreach (var a in w)
		{
            App.Current.CloseWindow(a);
        }
    }


	private async void GetStatCourier()
	{
		var res = await orderService.GetWorkStat();

		if (res.IsSuccess == false)
		{
			await DisplayAlert("Ошибка", res.ErrorMsg, "OK");
		}
		else
		{
			Stat = res.Data.ToObservableCollection()!;
			OnPropertyChanged(nameof(Stat));
		}
	}

}
