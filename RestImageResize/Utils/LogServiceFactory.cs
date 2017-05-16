using System;
using System.Configuration;
using JetBrains.Annotations;
using OpenWaves;

namespace RestImageResize.Utils
{
    public class LogServiceFactory : ILogServiceFactory
    {
        public virtual ILoggingService CreateLogger()
        {
            var config = ReadConfiguration("restImageResize.logging");
            var service = (ILoggingService) Activator.CreateInstance(config.LogServiceType, (IFileLoggingServiceConfiguration) config);
            return service;
        }

        [NotNull]
        protected virtual LoggingConfigurationSection ReadConfiguration(string sectionName)
        {
            var result = (LoggingConfigurationSection) ConfigurationManager.GetSection(sectionName) ?? GetDefaultConfiguration();
            return result;
        }

        protected virtual LoggingConfigurationSection GetDefaultConfiguration()
        {
            return new LoggingConfigurationSection
            {
                LogSeverityThreshold = LogEntrySeverity.Info,
                LogServiceType = typeof(DebugOutputLoggingService)
            };
        }
    }
}