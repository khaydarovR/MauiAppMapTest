using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling.Layers;
using MauiAppMapTest.Infra;
using MauiAppMapTest.Services;

namespace MauiAppMapTest.Page;

public partial class HomePage : ContentPage
{
	private Mapsui.UI.Maui.MapControl mapControl;
	private TileLayer tileLayer;
	private GeoService geo;
	public HomePage(GeoService geo)
	{
		InitializeComponent();

		this.geo = geo;

		mapControl = new Mapsui.UI.Maui.MapControl();
		tileLayer = Mapsui.Tiling.OpenStreetMap.CreateTileLayer();
		mapControl.Map?.Layers.Add(tileLayer);
		Content = mapControl;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		GetLocationAndUpdateMap();

	}
	Mapsui.Layers.MyLocationLayer myLocationLayer;
	private async Task GetLocationAndUpdateMap()
	{
		var userLocation = await geo.GetUserLocationAsync();
		if (userLocation.IsSuccess)
		{
			myLocationLayer = new Mapsui.Layers.MyLocationLayer(mapControl.Map)
			{
				IsCentered = false
			};
			mapControl.Map.Layers.Add(myLocationLayer);
			MPoint sphericalMercatorCoordinate = TransformLocation(userLocation.Data!);

			myLocationLayer.UpdateMyLocation(sphericalMercatorCoordinate);
			mapControl.Map.Info += Map_Info;
		}
		else
		{
			await DisplayAlert("Ошибка", userLocation.ErrorMsg, "OK");
		}
	}

	private static MPoint TransformLocation(Location userLocation)
	{
		var mp = new MPoint(userLocation.Longitude, userLocation.Latitude);
		var sphericalMercatorCoordinate = SphericalMercator
			.FromLonLat(mp.X, mp.Y).ToMPoint();
		return sphericalMercatorCoordinate;
	}

	private async void Map_Info(object? sender, MapInfoEventArgs e)
	{
		myLocationLayer.IsCentered = true;
		myLocationLayer.IsMoving = true;
		var pos = await geo.GetUserLocationAsync();
		if (pos.IsSuccess)
		{
			var tpos = TransformLocation(pos.Data!);
			myLocationLayer.UpdateMyLocation(tpos, true);
			if (pos.Data!.Course != null)
			{
				myLocationLayer.UpdateMyDirection(pos.Data!.Course.Value, myLocationLayer.ViewingDirection, true);
			}
		}
	}
}