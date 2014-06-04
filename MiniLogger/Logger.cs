using System;
using System.Collections.Generic;

namespace MiniLogger
{
    /// <summary>
    /// Main working class for a logging system, as a singleton. 
    /// </summary>
    public class Logger
    {
        public static LogSeverities[] Severities = new LogSeverities[] { LogSeverities.Log, LogSeverities.Info, LogSeverities.Warning, LogSeverities.Error, LogSeverities.Fatal };

        //Singleton declaration
        private Logger()
        {

        }

        private Logger(LogUCtrl inCtrl)
        {
            ctrl = inCtrl;
        }

        private static Logger instance;

        public static Logger GetInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }

            return instance;
        }

        public static Logger Reset()
        {
            instance = new Logger();
            return instance;
        }
        
        private static LogUCtrl ctrl;

        int maxLines = 500;
        /// <summary>
        /// Maximum lines of log, the last log will be dropped each time a "maxLines"th log is added
        /// </summary>
        public int MaxLines
        {
            get { return maxLines; }
            set { maxLines = value; }
        }

        private List<Log> _logs = new List<Log>();
        public List<Log> Logs
        {
            get { return _logs; }
            set { _logs = value; }
        }

        /// <summary>
        /// Main method to add a Log to the system
        /// </summary>
        /// <param name="inText">Message to Add as Log</param>
        /// <param name="inSeverity">Severity of the log (used in Filters and to set its Color + some specific treatment for "LogSeverities.Error")</param>
        public static void Log(string inText, LogSeverities inSeverity)
        {
            if (instance != null)
            {
                instance._logs.Add(new Log(inText, inSeverity));
                if (instance.maxLines > 0 && instance._logs.Count > instance.maxLines)
                {
                    instance._logs.RemoveAt(0);
                }
                instance.OnLogAdd(instance, new EventArgs());
            }
            else
            {
                throw new Exception("Logger instance not created, please use \"logUCtrl.Bind(Logger.GetInstance());\" to bind the singleton.");
            }
        }

        public Log LastLog
        {
            get { return _logs[_logs.Count - 1]; }
        }

        #region EVENT LogChangeHandler
        // The delegate
        public delegate void LogChangeHandler(object sender, EventArgs data);

        // The event
        public event LogChangeHandler LogChange;

        // The method which fires the Event
        protected void OnLogChange(object sender, EventArgs data)
        {
            // Check if there are any Subscribers   
            if (LogChange != null)
            {
                // Call the Event
                LogChange(this, data);
            }
        }

        // The delegate
        public delegate void LogAddHandler(object sender, EventArgs data);

        // The event
        public event LogAddHandler LogAdd;

        // The method which fires the Event
        protected void OnLogAdd(object sender, EventArgs data)
        {
            // Check if there are any Subscribers   
            if (LogAdd != null)
            {
                // Call the Event
                LogAdd(this, data);
            }
        }

        #endregion
    }
}
