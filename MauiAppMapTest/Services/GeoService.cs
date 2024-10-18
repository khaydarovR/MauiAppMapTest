
using MauiAppMapTest.DTO;
using MauiAppMapTest.Infra;
using System.Text.Json;

namespace MauiAppMapTest.Services;

public class GeoService
{
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

}

