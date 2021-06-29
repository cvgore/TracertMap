using System;
using MapControl;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using MaxMind.GeoIP2;

namespace TracertMap
{
    public class MapViewModel : IDisposable
    {
        public UIElement MapLayer { get; }

        public Location MapCenter { get; set; } = new(53.5, 8.2);

        public ObservableCollection<PointItem> Points { get; } = new ObservableCollection<PointItem>();
        public ObservableCollection<PointItem> Pushpins { get; } = new ObservableCollection<PointItem>();
        public ObservableCollection<Polyline> Polylines { get; } = new ObservableCollection<Polyline>();

        private DatabaseReader _geoIpDbReader;

        public MapViewModel()
        {
            MapLayer = new MapTileLayer
            {
                TileSource = new TileSource {UriFormat = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"},
                SourceName = "OpenStreetMap",
                Description = "http://www.openstreetmap.org/copyright"
            };

            _geoIpDbReader = new DatabaseReader("GeoIP2-City.mmdb");
            // Replace "City" with the appropriate method for your database, e.g.,
            // "Country".
            var city = _geoIpDbReader.City("128.101.101.101");

            // Points.Add(new PointItem
            // {
            //     Name = "Steinbake Leitdamm",
            //     Location = new Location(53.51217, 8.16603)
            // });
            // Points.Add(new PointItem
            // {
            //     Name = "Buhne 2",
            //     Location = new Location(53.50926, 8.15815)
            // });
            // Points.Add(new PointItem
            // {
            //     Name = "Buhne 4",
            //     Location = new Location(53.50468, 8.15343)
            // });
            // Points.Add(new PointItem
            // {
            //     Name = "Buhne 6",
            //     Location = new Location(53.50092, 8.15267)
            // });
            // Points.Add(new PointItem
            // {
            //     Name = "Buhne 8",
            //     Location = new Location(53.49871, 8.15321)
            // });
            // Points.Add(new PointItem
            // {
            //     Name = "Buhne 10",
            //     Location = new Location(53.49350, 8.15563)
            // });
            //
            // Polylines.Add(new Polyline
            // {
            //     Locations = LocationCollection.Parse(
            //         "53.5140,8.1451 53.5123,8.1506 53.5156,8.1623 53.5276,8.1757 53.5491,8.1852 53.5495,8.1877 53.5426,8.1993 53.5184,8.2219 53.5182,8.2386 53.5195,8.2387")
            // });
            // Polylines.Add(new Polyline
            // {
            //     Locations = LocationCollection.Parse(
            //         "53.5978,8.1212 53.6018,8.1494 53.5859,8.1554 53.5852,8.1531 53.5841,8.1539 53.5802,8.1392 53.5826,8.1309 53.5867,8.1317 53.5978,8.1212")
            // });
        }

        public void Dispose()
        {
            _geoIpDbReader.Dispose();
        }
    }
}