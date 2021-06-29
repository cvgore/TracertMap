using System;
using System.Diagnostics.Tracing;

namespace TracertMap
{
    public class Log
    {
        public EventLevel Level { get; }
        public DateTime OccurredAt { get; }
        public string Text { get; }

        public Log(EventLevel level, string text)
        {
            Level = level;
            OccurredAt = DateTime.Now;
            Text = text;
        }

        private static string GetLevelIdent(EventLevel level)
        {
            return level switch
            {
                EventLevel.Informational => "INFO",
                EventLevel.Error => "ERR",
                EventLevel.Warning => "WARN",
                _ => "?"
            };
        }

        public override string ToString()
        {
            return $"[{OccurredAt}] - {GetLevelIdent(Level),5}: {Text}";
        }
    }
}