using OpenWaves;

namespace RestImageResize.Utils
{
    public interface ILogServiceFactory
    {
        ILoggingService CreateLogger();
    }
}