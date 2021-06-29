using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MapControl;
using MapControl.Caching;

namespace TracertMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppViewModel _ctx;
        private TracerouteOutputParser _outputParser;
        private Logger _logger;

        public MainWindow()
        {
            InitializeComponent();

            _logger = new Logger(Dispatcher);
            DataContext = new AppViewModel(_logger);

            _ctx = (AppViewModel) DataContext;
            _outputParser = new TracerouteOutputParser();

            ImageLoader.HttpClient.DefaultRequestHeaders.Add("User-Agent", "TracertMap");
            TileImageLoader.Cache = new ImageFileCache(TileImageLoader.DefaultCacheFolder);
        }

        private async void ExecuteTracingExternalCommand(object s, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => _ctx.Points.Clear());
            Dispatcher.Invoke(() => _ctx.Polylines.Clear());

            var cmd = new TracingExternalCommand();

            _ctx.TracingExecuting = true;

            cmd.OutputReceived += TracingExternalCommandOutput;

            _ctx.Logger.Info($"Tracing started");

            var success = await cmd.Execute(_ctx.Host);

            _ctx.TracingExecuting = false;

            if (success)
            {
                _ctx.Polylines.Add(new Polyline
                {
                    Locations = new(_ctx.Points.Select(x => x.Location))
                });
                
                _ctx.Logger.Info("Tracing finished successfully");
            }
            else
            {
                _ctx.Logger.Error("Tracing finished with error");
            }
        }

        private async void TracingExternalCommandOutput(object? sender, string line)
        {
            var tr = await _outputParser.ParseLine(line);

            if (tr == null)
            {
                _logger.Info($"Skipping invalid line '{line}'");
                return;
            }
            if (tr.IsIpInternal())
            {
                _logger.Info($"Skipping local address {tr.IpAddress}");
                return;
            }

            var city = _ctx.GeoIpDbReader.City(tr.IpAddress.ToString());

            if (city.Location.Latitude != null && city.Location.Longitude != null)
            {
                _logger.Info($"Matched city {tr.IpAddress} - {city.City} @ {city.Location}");

                Dispatcher.Invoke(() =>
                {
                    _ctx.Points.Add(new PointItem
                    {
                        Location = new(city.Location.Latitude.Value, city.Location.Longitude.Value),
                        Name = $"{tr.IpAddress} @ {city.City.Name}"
                    });
                });

                return;
            }

            _logger.Info($"No matches found in DB for {tr.IpAddress}");
        }

        private void ToggleLogs(object sender, RoutedEventArgs e)
        {
            _ctx.LogsVisible = !_ctx.LogsVisible;
        }

        private void AlwaysScrollToBottom(object sender, ScrollChangedEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer scrollViewer &&
                Math.Abs(e.ExtentHeightChange) > 0.0)
            {
                scrollViewer.ScrollToBottom();
            }
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            var wnd = new AboutWindow();

            wnd.ShowDialog();
        }

        private void CopyLogsToClipboard(object sender, RoutedEventArgs e)
        {
            var logs = string.Join(Environment.NewLine, _logger.Logs);
            
            Clipboard.SetText(logs);
        }
    }
}