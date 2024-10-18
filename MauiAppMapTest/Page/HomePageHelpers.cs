using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;

public static class HomePageHelpers
{

    public static MPoint TransformLocation(Location userLocation)
    {
        var mp = new MPoint(userLocation.Longitude, userLocation.Latitude);
        var sphericalMercatorCoordinate = SphericalMercator
            .FromLonLat(mp.X, mp.Y).ToMPoint();
        return sphericalMercatorCoordinate;
    }

    public static MPoint TransformLocation(double x, double y)
    {
        var mp = new MPoint(x, y);
        var sphericalMercatorCoordinate = SphericalMercator
            .FromLonLat(mp.X, mp.Y).ToMPoint();
        return sphericalMercatorCoordinate;
    }
}