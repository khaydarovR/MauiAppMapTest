using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Manipulations;
using Mapsui.Nts;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling.Layers;
using MauiAppMapTest.Infra;
using MauiAppMapTest.Services;
using Microsoft.Maui.Controls.Shapes;
using NetTopologySuite.Geometries;
using System.Net.WebSockets;
using Point = NetTopologySuite.Geometries.Point;

namespace MauiAppMapTest.Page;

public partial class HomePage : ContentPage
{
    private Mapsui.UI.Maui.MapControl mapControl;
    private TileLayer tileLayer;
    private GenericCollectionLayer<List<IFeature>> orderLayer;
    private GeoService geo;
    public HomePage(GeoService geo)
    {
        InitializeComponent();

        this.geo = geo;

        mapControl = new Mapsui.UI.Maui.MapControl();

        tileLayer = Mapsui.Tiling.OpenStreetMap.CreateTileLayer();
        mapControl.Map?.Layers.Add(tileLayer);

        orderLayer = new GenericCollectionLayer<List<IFeature>>
        {
            Style = SymbolStyles.CreatePinStyle(),
            Name = "orderLayer",
        };
        mapControl.Map?.Layers.Add(orderLayer);
        Content = mapControl;

        geo.OnPosChanged += (s) => DisplayAlert(s, "", "OK"); 
    }

    protected override async void OnAppearing()
    {
		base.OnAppearing();

		await GetLocationAndUpdateMap();
        await SetOrderPos();

		try
		{
			var r = await geo.StartShareMyPosWithServ();
            if (!r.IsSuccess)
            {
				await DisplayAlert("Ошибка отправки геолокации", r.ErrorMsg, "OK");
			}
		}
		catch (Exception e)
		{
			await DisplayAlert("Ошибка отправки геолокации", e.Message, "OK");
		}
	}

    private async Task SetOrderPos()
    {
        var order = await geo.GetOrder();
        if (order.IsSuccess)
        {
            MPoint mp = HomePageHelpers
                .TransformLocation(order.Data.TargetPosLong, order.Data.TargetPosLati);

            orderLayer?.Features.Add(new GeometryFeature
            {
                Geometry = new Point(mp.X, mp.Y)
            });
            // To notify the map that a redraw is needed.
            orderLayer?.DataHasChanged();
        }

    }

    Mapsui.Layers.MyLocationLayer myLocationLayer;


	private async Task GetLocationAndUpdateMap()
    {
        var userLocation = await geo.GetUserLocationAsync();
        if (userLocation.IsSuccess)
        {
            myLocationLayer = new Mapsui.Layers.MyLocationLayer(mapControl.Map)
            {
                IsCentered = true,
            };

            mapControl.Map.Layers.Add(myLocationLayer);

            // Преобразование координат
            MPoint sphericalMercatorCoordinate = HomePageHelpers.TransformLocation(userLocation.Data!);
            myLocationLayer.UpdateMyLocation(sphericalMercatorCoordinate);

            // Центрирование карты на местоположении пользователя
            mapControl.Map.Navigator.CenterOn(sphericalMercatorCoordinate);
            mapControl.Map?.Navigator.ZoomTo(120, 3000);

            mapControl.Map.Info += Map_Info;
        }
        else
        {
            await DisplayAlert("Ошибка", userLocation.ErrorMsg, "OK");
        }
    }

    private async void Map_Info(object? sender, MapInfoEventArgs e)
    {
        myLocationLayer.IsCentered = true;
        myLocationLayer.IsMoving = true;

        // Получаем текущее местоположение
        var pos = await geo.GetUserLocationAsync();
        if (pos.IsSuccess)
        {
            var tpos = HomePageHelpers.TransformLocation(pos.Data!);
            myLocationLayer.UpdateMyLocation(tpos, true);

            // Обновление направления
            if (pos.Data!.Course != null)
            {
                myLocationLayer.UpdateMyDirection(pos.Data!.Course.Value, myLocationLayer.ViewingDirection, true);
            }

            // Центрирование карты на текущих координатах
            var sc = new ScreenPosition(tpos.X, tpos.Y);
            mapControl.Map.Navigator.CenterOn(tpos.X, tpos.Y);
            mapControl.Map.Navigator.ZoomTo(20, 2000); // Установите желаемый масштаб
        }
    }
}