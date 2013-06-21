using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenWaves;

namespace RestImageResize.Utils
{
    /// <summary>
    /// Simple <see cref="IResolver"/> that allow to wrap other <see cref="IResolver"/> instance and add new of override existing services.
    /// </summary>
    public class WrapResolver : IResolver
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly IResolver _wrappedResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapResolver" /> class.
        /// </summary>
        /// <param name="resolverToWrap">The resolver to wrap.</param>
        /// <exception cref="System.ArgumentNullException">resolverToWrap</exception>
        public WrapResolver([NotNull] IResolver resolverToWrap)
        {
            if (resolverToWrap == null)
            {
                throw new ArgumentNullException("resolverToWrap");
            }

            _wrappedResolver = resolverToWrap;
        }

        /// <summary>
        /// Tries to resolve service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public bool TryResolve<TService>(out TService service) where TService : class
        {
            object instance;
            if (_services.TryGetValue(typeof(TService), out instance))
            {
                service = (TService)instance;
                return true;
            }

            return _wrappedResolver.TryResolve(out service);
        }

        /// <summary>
        /// Registers the service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instance">The service instance.</param>
        public void Register<TService>(TService instance)
        {
            _services[typeof(TService)] = instance;
        }
    }
}
