using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenWaves.ImageTransformations;

namespace RestImageResize
{
    /// <summary>
    /// Extends the <see cref="ImageTransformationParser"/> to provide parsing of any type of <see cref="IImageTransformation"/> instead predefined only.
    /// </summary>
    public class UniversalImageTransformationParser : ImageTransformationParser
    {
        /// <summary>
        /// Tries to parse transformation instance by name and properties string. 
        /// </summary>
        /// <param name="name">The transformation name.</param>
        /// <param name="serializedProperties">The serialized transformation properties string.</param>
        /// <param name="transformation">The transformation instance.</param>
        /// <returns>
        /// <c>true</c> if transformation is successfully parsed, otherwise - <c>False</c>
        /// </returns>
        protected override bool TryParse(string name, string serializedProperties, out IImageTransformation transformation)
        {
            if (base.TryParse(name, serializedProperties, out transformation))
            {
                return true;
            }

            try
            {
                Type type = Type.GetType(name);
                if (type != null && typeof(IImageTransformation).IsAssignableFrom(type))
                {
                    var ctor = type.GetConstructor(new[] { typeof(string) });
                    if (ctor != null)
                    {
                        object instance = ctor.Invoke(new object[] { serializedProperties });
                        transformation = (IImageTransformation)instance;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
