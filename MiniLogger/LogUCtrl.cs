using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MiniLogger
{
    /// <summary>
    /// Simple Control containing a multiline RichTextBox used to display Logging Messages
    /// @todo {diff}2{/diff}{prio}3{/prio}Custom paint the messages instead of using a heavy RichTextBox
    /// </summary>
    public partial class LogUCtrl : UserControl
    {
        public delegate void RegularCallback(object sender, EventArgs data);

        LogPreferences prefs = new LogPreferences();
        public LogPreferences LoggingPreferences
        {
            get { return prefs; }
            set { prefs = value; }
        }

        public LogUCtrl()
        {
            InitializeComponent();

            LogColors.Add(LogSeverities.Error, Color.Maroon);
            LogColors.Add(LogSeverities.Fatal, Color.Red);
            LogColors.Add(LogSeverities.Info, Color.DarkGreen);
            LogColors.Add(LogSeverities.Log, Color.Gray);
            LogColors.Add(LogSeverities.Warning, Color.DarkOrange);

            verboseToolStripMenuItem.CheckedChanged += delegate { prefs.ShowLogs = verboseToolStripMenuItem.Checked; RefreshLogs(); };
            infoToolStripMenuItem.CheckedChanged += delegate { prefs.ShowInfos = infoToolStripMenuItem.Checked; RefreshLogs(); };
            warningToolStripMenuItem.CheckedChanged += delegate { prefs.ShowWarnings = warningToolStripMenuItem.Checked; RefreshLogs(); };
            errorToolStripMenuItem.CheckedChanged += delegate { prefs.ShowErrors = errorToolStripMenuItem.Checked; RefreshLogs(); };
        }

        private Dictionary<LogSeverities, Color> LogColors = new Dictionary<LogSeverities, Color>();

        Logger logger;

        private void RefreshLogs()
        {
            LogRTB.Clear();
            LogRTB.SuspendLayout();

            for (var i = 0; i < logger.Logs.Count; i++ )
            {
                if (IsVisible(logger.Logs[i]))
                {
                    AppendLog(logger.Logs[i]);
                }
            }

            LogRTB.ResumeLayout(true);
        }

        private bool IsVisible(Log log)
        {
            if (log.Severity == LogSeverities.Log && !prefs.ShowLogs) { return false; }
            if (log.Severity == LogSeverities.Info && !prefs.ShowInfos) { return false; }
            if (log.Severity == LogSeverities.Warning && !prefs.ShowWarnings) { return false; }
            if (log.Severity == LogSeverities.Error && !prefs.ShowErrors) { return false; }

            return true;
        }

        /// <summary>
        /// Append Log of the given color.
        /// </summary>
        /// <param name="inLog">new Log to add to the Log Control</param>
        void AppendLog(Log inLog)
        {
            if (!LogRTB.IsDisposed)
            {
                int lengthBefore = LogRTB.TextLength;
                string pad = "";
                for (int i = 8 - inLog.Severity.ToString().Length; i > 0; i--)
                {
                    pad += " ";
                }

                string log = String.Format("{0}{1}: {2}\r\n", inLog.Severity, pad, inLog.Text);

                LogRTB.SelectionLength = 0; // clear
                LogRTB.SelectionStart = 0;
                LogRTB.SelectedText = log;

                int length = LogRTB.TextLength - lengthBefore;

                // Textbox may transform chars, so (end-start) != text.Length
                LogRTB.Select(0, length);
                {
                    LogRTB.SelectionColor = LogColors[inLog.Severity];
                }
            }
        }

        public void Bind(Logger inLogger)
        {
            logger = inLogger;
            logger.LogChange += logger_LogChange;
            logger.LogAdd += logger_LogAdd;
        }

        void logger_LogChange(object sender, EventArgs data)
        {
            if (InvokeRequired)
            {
                RegularCallback p = new RegularCallback(logger_LogChange);
                Invoke(p, new object[] { sender, data });
            }
            else
            {
                RefreshLogs();
            }
        }

        void logger_LogAdd(object sender, EventArgs data)
        {
            if (InvokeRequired)
            {
                RegularCallback p = new RegularCallback(logger_LogAdd);
                Invoke(p, new object[] { sender, data });
            }
            else
            {
                if (IsVisible(logger.LastLog))
                {
                    AppendLog(logger.LastLog);
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.Logs.Clear();
            RefreshLogs();
        }

        private void LogRTB_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                verboseToolStripMenuItem.Checked = prefs.ShowLogs;
                infoToolStripMenuItem.Checked = prefs.ShowInfos;
                warningToolStripMenuItem.Checked = prefs.ShowWarnings;
                warningToolStripMenuItem.Checked = prefs.ShowErrors;

                contextMenuStrip1.Show(PointToScreen(e.Location));
            }
        }
    }
}
