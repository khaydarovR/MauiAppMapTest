
using MauiAppMapTest.Infra;

namespace MauiAppMapTest.Services;

public class GeoService
{
	public async Task<Res<Location>> GetUserLocationAsync()
	{
		try
		{
			var r = new GeolocationRequest(GeolocationAccuracy.Best);
			var location = await Geolocation.Default.GetLocationAsync(r);
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
}

