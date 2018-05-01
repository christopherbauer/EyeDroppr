using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Core.Logger
{
    public static class Logger
    {
        private static readonly Stopwatch AppTimer;
        private static LoggerMode _mode = LoggerMode.Total;
        private static LoggerVerbosity _verbosity = LoggerVerbosity.Minimal;
        private static TimeSpan _lastLog;

        static Logger()
        {
            AppTimer = new Stopwatch();
            AppTimer.Start();
        }

        public static void SetMode(LoggerMode mode, LoggerVerbosity verbosity = LoggerVerbosity.Minimal)
        {
            _mode = mode;
            _verbosity = verbosity;
        }

        public static void Log(string messge, [CallerFilePath] string callerFilePath = null, [CallerMemberName]string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
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
            var time = _mode == LoggerMode.Difference ? "+" : string.Empty;
            var logContext = $"{callerMemberName}: ";
            if (_verbosity == LoggerVerbosity.Verbose)
            {
                logContext = $"{GetContextFilePath(callerFilePath)} {callerMemberName} line {callerLineNumber}: ";
            }
            Debug.WriteLine($"{logContext}{time}{reportLogTime}: {messge}");
        }

        private static string GetContextFilePath(string callerFilePath)
        {
            var sourceFolder = @"\src";
            return "~"+callerFilePath.Remove(0, callerFilePath.IndexOf(sourceFolder) + sourceFolder.Length);
        }
    }
}