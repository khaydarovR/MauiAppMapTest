
using GD.Shared.Common;
using MauiAppMapTest.DTO;
using MauiAppMapTest.Infra;
using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text.Json;

namespace MauiAppMapTest.Services;

public class GeoService
{
	private HubConnection hubConnection;

	public bool IsConnected { get; private set; }
	public bool IsBusy { get; private set; }

    public GeoService()
    {

	}

    public async Task<Res<Location>> GetUserLocationAsync()
	{
		try
		{
			var r = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));
			var location = await Geolocation.GetLocationAsync(r);
			if (location == null) location = await Geolocation.Default.GetLastKnownLocationAsync();

			if (location != null)
			{

				return new Res<Location>(location);
			}
			return new Res<Location>("location null");
		}
		catch (Exception ex)
		{
			// Handle any errors that occurred
			return new Res<Location>(ex.Message);
		}
	}

	bool isListenPos = false;

	public async Task<Res<bool>> StartShareMyPosWithServ()
	{
		if (isListenPos)
		{
			return new Res<bool>(true);
		}
		try
		{
			CreateConnection();
			var c = await Connect();
			
			if (!c)
			{
				return new Res<bool>("Connect false");
			}
			var r = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));

			var isStart = await Geolocation.StartListeningForegroundAsync(r);
			if (isStart)
			{
				Geolocation.LocationChanged += Geolocation_LocationChanged;
				isListenPos = true;
				return new Res<bool>(true);
			}

			return new Res<bool>("StartListeningForegroundAsync false");
		}
		catch (Exception ex)
		{
			return new Res<bool>("Не смог начать слушать гео: " + ex.Message);
		}
	}

    public async Task<Res<Order>> GetOrder()
    {
        // Получаем строку с данными заказа из Preferences
        var orderJson = Preferences.Get("order", null);

        // Проверка на наличие данных
        if (string.IsNullOrEmpty(orderJson))
        {
            return new Res<Order>("Нет заказов взятых на работу");
        }

        try
        {
            // Десериализация строки в объект Order
            var order = JsonSerializer.Deserialize<Order>(orderJson);

            // Проверяем на случай, если десериализация не удалась
            if (order == null)
            {
                return new Res<Order>("Serialize error");
            }

            return new Res<Order>(order);
        }
        catch (Exception ex)
        {
            // Обработка ошибок
            return new Res<Order>(ex.Message);
        }
    }


	private async Task ShareMyLocation(HubPosInfo pos)
	{
		await hubConnection.InvokeAsync("SendPos", pos);

	}

	public void CreateConnection()
	{
		// создание подключения
		hubConnection = new HubConnectionBuilder()
			.WithUrl(Const.BASE_URL + "poshub")
			.Build();

		IsConnected = false;    // по умолчанию не подключены
		IsBusy = false;         // отправка сообщения не идет



		hubConnection.Closed += async (error) =>
		{
			IsConnected = false;
			await Task.Delay(5000);
			await Connect();
		};
	}

	public async Task Disconnect()
	{
		if (!IsConnected) return;

		await hubConnection.StopAsync();
		IsConnected = false;

	}

	public async Task<bool> Connect()
	{
		if (IsConnected)
			return true;
		try
		{
			await hubConnection.StartAsync();

			IsConnected = true;
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	public Action<string> OnPosChanged;
	HubPosInfo pos = new HubPosInfo(){ UserId = AuthProvider.UserToken.id };
	private async void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
	{
		pos.TargetPosLong = e.Location.Longitude;
		pos.TargetPosLati = e.Location.Latitude;
		//OnPosChanged.Invoke(pos.TargetPosLong + " : " +  pos.TargetPosLong);
		await ShareMyLocation(pos);
	}
}



