using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TracertMap
{
    public class TracingExternalCommand
    {
        private string _command = "tracert.exe";

        public event EventHandler<string>? OutputReceived;

        private string MakeParams(string host)
        {
            return $"-d {host}";
        }

        public async Task<bool> Execute(string host)
        {
            var p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = MakeParams(host),
                    CreateNoWindow = true,
                    FileName = _command,
                    WorkingDirectory = Environment.CurrentDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            
            p.OutputDataReceived += TracertCmd_OutputReceived;
            p.Start();
            p.BeginOutputReadLine();

            await p.WaitForExitAsync();

            return p.ExitCode == 0;
        }

        private void TracertCmd_OutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                OutputReceived?.Invoke(null, e.Data);
            }
        }
    }
}