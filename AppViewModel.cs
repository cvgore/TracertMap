using System;
using MapControl;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using MaxMind.GeoIP2;

namespace TracertMap
{
    public class AppViewModel : INotifyPropertyChanged, IDisposable
    {
        public UIElement MapLayer { get; }

        public Location MapCenter { get; set; } = new(49.8122898,18.9672228);

        public string Host { get; set; }

        public Logger Logger { get; }

        public bool LogsVisible { get; set; }

        public ObservableCollection<PointItem> Points { get; } = new ObservableCollection<PointItem>();
        public ObservableCollection<Polyline> Polylines { get; } = new ObservableCollection<Polyline>();

        public bool TracingExecuting { get; set; }

        public DatabaseReader GeoIpDbReader { get; }

        public AppViewModel(Logger logger)
        {
            MapLayer = new MapTileLayer
            {
                TileSource = new TileSource {UriFormat = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"},
                SourceName = "OpenStreetMap",
                Description = "http://www.openstreetmap.org/copyright"
            };

            GeoIpDbReader = new DatabaseReader(
                Path.Join(AppContext.BaseDirectory, "dbip-city-lite-2021-06.mmdb")
            );

            Logger = logger;
        }

        public void Dispose()
        {
            GeoIpDbReader.Dispose();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}