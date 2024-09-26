
using MauiAppMapTest.Infra;

namespace MauiAppMapTest.Services;

public class GeoService
{
	public async Task<Res<Point>> GetUserLocationAsync()
	{
		try
		{
			var location = await Geolocation.Default.GetLocationAsync();
			if (location == null) location = await Geolocation.Default.GetLastKnownLocationAsync();
			
			if (location != null)
			{
				var p = new Point(location.Longitude, location.Latitude);
				return new Res<Point>(p);
			}
			return new Res<Point>("location null");
		}
		catch (Exception ex)
		{
			// Handle any errors that occurred
			return new Res<Point>(ex.Message);
		}
	}
}

