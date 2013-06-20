using System;
using System.Configuration;

namespace RestImageResize.Utils
{
    /// <summary>
    /// Provides utile methods that help to work with configuration files.
    /// </summary>
    public static class ConfigUtils
    {
        #region Constants

        private const string RequiredSettingHasNotBeenFound = "Required configuration parameter '{0}' has not been found in app settings.";
        private const string ParameterValueDoesNotMatchExpectedFormat = "Configuration parameter's '{0}' value '{1}' does not match expected format.";

        #endregion

        #region Methods

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="settingKey">The setting key.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        public static TValue ReadAppSetting<TValue>(string settingKey)
        {
            return ReadAppSetting(settingKey, (value) => SmartConvert.ChangeType<TValue>(value));
        }

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="isRequired">Indicating whether value is required.</param>
        /// <param name="settingKey">The setting's key.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        public static TValue ReadAppSetting<TValue>(bool isRequired, string settingKey)
        {
            return ReadAppSetting(isRequired, settingKey, (value) => SmartConvert.ChangeType<TValue>(value));
        }

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="isRequired">Indicating whether value is required.</param>
        /// <param name="settingKey">The setting's key.</param>
        /// <param name="valueConverter">The value from string conversion method.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        public static TValue ReadAppSetting<TValue>(bool isRequired, string settingKey, Func<string, TValue> valueConverter)
        {
            return ReadAppSetting(settingKey, valueConverter, null, isRequired);
        }

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="settingKey">The setting's key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        public static TValue ReadAppSetting<TValue>(string settingKey, TValue defaultValue)
        {
            return ReadAppSetting(settingKey, (value) => SmartConvert.ChangeType<TValue>(value), defaultValue);
        }

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="settingKey">The setting's key.</param>
        /// <param name="valueConverter">The value from string conversion method.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        public static TValue ReadAppSetting<TValue>(string settingKey, Func<string, TValue> valueConverter, TValue defaultValue)
        {
            return ReadAppSetting(settingKey, valueConverter, new Tuple<TValue>(defaultValue), false);
        }

        /// <summary>
        /// Reads the setting entry of application settings section.
        /// </summary>
        /// <typeparam name="TValue">The setting's value type.</typeparam>
        /// <param name="settingKey">The setting's key.</param>
        /// <param name="valueConverter">The value from string conversion method.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="isRequired">Indicating whether value is required.</param>
        /// <returns>
        /// The setting value.
        /// </returns>
        private static TValue ReadAppSetting<TValue>(string settingKey, Func<string, TValue> valueConverter, Tuple<TValue> defaultValue = null, bool isRequired = false)
        {
            string stringValue = ConfigurationManager.AppSettings[settingKey];
            if (stringValue == null)
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException(string.Format(RequiredSettingHasNotBeenFound, settingKey));
                }

                return defaultValue != null ? defaultValue.Item1 : default(TValue);
            }

            try
            {
                return valueConverter(stringValue);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format(ParameterValueDoesNotMatchExpectedFormat, settingKey, stringValue), ex);
            }
        }

        #endregion
    }
}
