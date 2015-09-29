using System.Collections.Generic;
using System.Linq;
using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// Provides configuration options.
    /// </summary>
    internal static class Config
    {
        internal static class AppSettingKeys
        {
            private const string Prefix = "RestImageResize.";
            // ReSharper disable MemberHidesStaticFromOuterClass

            public const string DefaultTransform = Prefix + "DefautTransform";
            public const string PrivateKeys = Prefix + "PrivateKeys";

            // ReSharper restore MemberHidesStaticFromOuterClass
        }

        /// <summary>
        /// Gets the default image transformation type.
        /// </summary>
        public static ImageTransform DefaultTransform
        {
            get { return ConfigUtils.ReadAppSetting(AppSettingKeys.DefaultTransform, ImageTransform.DownFit); }
        }

        /// <summary>
        /// Gets configured private keys as string in format "{pk1:abc123|pk2:456edf}" (keys are pipe-delimited, each containing a name and a private key itself).
        /// </summary>
        public static string PrivateKeys
        {
            get { return ConfigUtils.ReadAppSetting<string>(AppSettingKeys.PrivateKeys); }
        }
    }
}
