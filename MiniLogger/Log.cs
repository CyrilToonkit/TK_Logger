using System;
using System.Collections.Generic;

namespace MiniLogger
{
    /// <summary>
    /// Simple pure data class that stands for a logging Message
    /// </summary>
    public class Log
    {
        public Log(string intext, LogSeverities inSeverity)
        {
            text = intext;
            severity = inSeverity;
            time = DateTime.Now;
        }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        LogSeverities severity;
        public LogSeverities Severity
        {
            get { return severity; }
            set { severity = value; }
        }

        DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
