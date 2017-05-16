using System;
using OpenWaves;

namespace RestImageResize.Utils
{
    public class DebugOutputLoggingService : ILoggingService
    {
        public DebugOutputLoggingService(IFileLoggingServiceConfiguration configuration)
            : this(configuration.LogSeverityThreshold)
        {
        }

        public DebugOutputLoggingService(LogEntrySeverity filter)
        {
            Filter = filter;
        }

        /// <summary>
        /// 
        /// </summary>
        protected LogEntrySeverity Filter { get; }

        protected virtual bool ShouldLog(LogEntrySeverity severity)
        {
            return severity >= Filter;
        }

        public virtual void Log(LogEntrySeverity severity, Exception exception, string message)
        {
            if (ShouldLog(severity))
            {
                if (exception != null)
                {
                    System.Diagnostics.Trace.WriteLine($"[{severity}] {message} - {exception}");
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine($"[{severity}] {message}");
                }
            }
        }

        public virtual void Log(LogEntrySeverity severity, string message)
        {
            Log(severity, null, message);
        }
    }

    public class DefaultLoggingService
    {
        public static Func<ILoggingService> InstanceAccessor { get; set; }  = () => DefaultInstance;

        public static ILoggingService Instance => InstanceAccessor();

        private static readonly ILoggingService DefaultInstance = new DebugOutputLoggingService(LogEntrySeverity.Info);
    }
}