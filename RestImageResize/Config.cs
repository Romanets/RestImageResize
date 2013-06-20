using RestImageResize.Utils;

namespace RestImageResize
{
    /// <summary>
    /// Provides configuration options.
    /// </summary>
    internal static class Config
    {
        private static class AppSettingKeys
        {
            private const string Prefix = "RestImageResize.";
            // ReSharper disable MemberHidesStaticFromOuterClass

            public const string DefaultTransform = Prefix + "DefautTransform";

            // ReSharper restore MemberHidesStaticFromOuterClass
        }

        /// <summary>
        /// Gets the default image transformation type.
        /// </summary>
        public static ImageTransform DefaultTransform
        {
            get { return ConfigUtils.ReadAppSetting(AppSettingKeys.DefaultTransform, ImageTransform.DownFit); }
        }
    }
}
