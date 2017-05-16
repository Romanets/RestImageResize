using System;
using System.Configuration;
using System.Web.Hosting;
using OpenWaves;

namespace RestImageResize.Utils
{
    public class LoggingConfigurationSection : ConfigurationSection, IFileLoggingServiceConfiguration
    {
        private Type _logServiceType;

        [ConfigurationProperty("logSeverityThreshold", DefaultValue = LogEntrySeverity.Info, IsRequired = false)]
        public virtual LogEntrySeverity LogSeverityThreshold
        {
            get
            {
                return (LogEntrySeverity)this["logSeverityThreshold"];
            }
            set
            {
                this["logSeverityThreshold"] = (object)value;
            }
        }

        protected virtual string GetDirectory(string physicalOrVirtualPath)
        {
            var directory = physicalOrVirtualPath;

            if (directory.StartsWith("~/") || directory.StartsWith("/"))
            {
                directory = HostingEnvironment.MapPath(directory);
            }

            return directory;
        }

        public virtual Type LogServiceType
        {
            get
            {
                if (_logServiceType == null)
                {
                    _logServiceType = Type.GetType(LogServiceTypeName);
                }

                return _logServiceType;
            }
            set { _logServiceType = value; }
        }

        [ConfigurationProperty("logServiceType", 
            DefaultValue = "RestImageResize.Utils.DebugOutputLoggingService, RestImageResize", IsRequired = false)]
        public virtual string LogServiceTypeName
        {
            get
            {
                var result = GetDirectory((string)this["logServiceType"]);
                return result;
            }
            set
            {
                this["logServiceType"] = (object)value;
            }
        }

        [ConfigurationProperty("logDirectory", DefaultValue = "~/App_Data/logs/", IsRequired = false)]
        public virtual string LogDirectory
        {
            get
            {
                var result = GetDirectory((string)this["logDirectory"]);
                return result;
            }
            set
            {
                this["logDirectory"] = (object)value;
            }
        }
    }
}