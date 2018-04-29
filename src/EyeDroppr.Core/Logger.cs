using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Core
{
    public static class Logger
    {
        private static readonly Stopwatch AppTimer;
        private static LoggerMode _mode = LoggerMode.Total;
        private static TimeSpan _lastLog;

        static Logger()
        {
            AppTimer = new Stopwatch();
            AppTimer.Start();
        }

        public static void SetMode(LoggerMode mode)
        {
            _mode = mode;
        }

        public static void Log(string messge, [CallerMemberName]string callerMemberName = null)
        {
            var logTime = AppTimer.Elapsed;
            TimeSpan reportLogTime;
            if (_mode == LoggerMode.Difference)
            {
                reportLogTime = logTime.Subtract(_lastLog);
            }
            else if (_mode == LoggerMode.Total)
            {
                reportLogTime = logTime;
            }
            _lastLog = logTime;
            Debug.WriteLine("{3}{0}: {1} ({2})", reportLogTime, messge, callerMemberName, (_mode == LoggerMode.Difference ? "+" : string.Empty));
        }
    }
}