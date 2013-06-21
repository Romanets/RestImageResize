using System.Reflection;
using OpenWaves;

namespace RestImageResize.Utils
{
    /// <summary>
    /// Contains utility methods that help to work with <see cref="ServiceLocator"/> class.
    /// </summary>
    public static class ServiceLocatorUtils
    {
        /// <summary>
        /// Gets the currently used resolver.
        /// </summary>
        /// <returns>
        /// The resolver instance.
        /// </returns>
        public static IResolver GetCurrentResolver()
        {
            var currentResolverField = typeof(ServiceLocator).GetField("currentResolver", BindingFlags.Static | BindingFlags.NonPublic);
            if (currentResolverField != null)
            {
                return currentResolverField.GetValue(null) as IResolver;
            }

            return null;
        }
    }
}
