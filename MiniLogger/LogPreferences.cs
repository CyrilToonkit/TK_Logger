using System;
using System.Collections.Generic;

namespace MiniLogger
{
    /// <summary>
    /// Main Preferences of the Logger (Logs visibility by type + overall visibility)
    /// </summary>
    public class LogPreferences
    {
        bool showLogs = true;
        public bool ShowLogs
        {
            get { return showLogs; }
            set { showLogs = value; }
        }

        bool showInfo = true;
        public bool ShowInfos
        {
            get { return showInfo; }
            set { showInfo = value; }
        }

        bool showWarnings = true;
        public bool ShowWarnings
        {
            get { return showWarnings; }
            set { showWarnings = value; }
        }

        bool showError = true;
        public bool ShowErrors
        {
            get { return showError; }
            set { showError = value; }
        }
    }
}
