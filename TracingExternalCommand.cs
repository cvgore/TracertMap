using System;
using System.Diagnostics;

namespace TracertMap
{
    public class Tracing
    {
        private string _command;
        private 
        
        public Tracing()
        {
            _command = "tracert";
        }

        public string MakeParams(string host)
        {
            return $"-d {host}";
        }

        public TraceRoute QueryRoute(string host)
        {
            var p = new Process
            {
                EnableRaisingEvents = true
            };

            p.StartInfo = new ProcessStartInfo
            {
                Arguments = MakeParams(host),
                CreateNoWindow = true,
                FileName = _command,
                WorkingDirectory = Environment.CurrentDirectory,
                UseShellExecute = true,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            p.OutputDataReceived += TracertCmd_OutputReceived;
        }

        private void TracertCmd_OutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                
            }
        }
    }
}