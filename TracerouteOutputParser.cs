using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TracertMap
{
    public class TracerouteOutputParser
    {
        private readonly Regex _lineRegex = new(
            @"^\s*\d+\s*<?\d+\s*ms\s*<?\d+\s*ms\s*<?\d+\s*ms\s*(?<ip>\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b)\s*$"
        );

        public async Task<TraceRoute?> ParseLine(string line)
        {
            return await Task.Run(() =>
            {
                var match = _lineRegex.Match(line);

                if (match.Success)
                {
                    var ip = IPAddress.Parse(match.Groups["ip"].Value);

                    return new TraceRoute(ip, 0);
                }

                return null;
            });
        }
    }
}