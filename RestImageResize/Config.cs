using System.Collections.Generic;
using System.Linq;
using RestImageResize.Security;
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

        public static IList<PrivateKey> PrivateKeys
        {
            get
            {
                var privateKeysString = ConfigUtils.ReadAppSetting<string>(AppSettingKeys.PrivateKeys);
                var privateKeys = privateKeysString.Split('|')
                    .Select(val => new PrivateKey
                    {
                        Name = val.Split(':').First(),
                        Key = val.Split(':').Last()
                    })
                    .ToList();
                return privateKeys;
            }
        }
    }
}
