using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Windows.Threading;

namespace TracertMap
{
    public class Logger
    {
        public ObservableCollection<Log> Logs { get; }
        private readonly Dispatcher _dispatcher;

        public Logger(Dispatcher dispatcher)
        {
            Logs = new();
            _dispatcher = dispatcher;
        }

        public void Info(string text)
        {
            _dispatcher.Invoke(() => Logs.Add(new Log(EventLevel.Informational, text)));
        }

        public void Warn(string text)
        {
            _dispatcher.Invoke(() => Logs.Add(new Log(EventLevel.Warning, text)));
        }

        public void Error(string text)
        {
            _dispatcher.Invoke(() => Logs.Add(new Log(EventLevel.Error, text)));
        }
    }
}