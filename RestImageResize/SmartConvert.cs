using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RestImageResize
{
    /// <summary>
    /// The conversion utile methods which do conversion in more smart safe way then standard <see cref="Convert"/> utility.
    /// </summary>
    public static class SmartConvert
    {
        #region Constants

        private static readonly HashSet<String> TrueStringValues = new HashSet<string> { "TRUE", "1", "YES", "+" };
        private static readonly HashSet<String> FalseStringValues = new HashSet<string> { "FALSE", "0", "NO", "-" };

        #endregion

        #region Methods

        /// <summary>
        /// Converts value to the equivalent value of specified type.
        /// </summary>
        /// <typeparam name="TResult">The target conversion type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        public static TResult ChangeType<TResult>(object value)
        {
            return (TResult)ChangeType(value, typeof(TResult));
        }

        /// <summary>
        /// Converts value to the equivalent value of specified type.
        /// </summary>
        /// <typeparam name="TResult">The target conversion type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value of target type that should be used as empty value equivalent.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        public static TResult ChangeType<TResult>(object value, TResult defaultValue)
        {
            return (TResult)ChangeType(value, defaultValue, typeof(TResult));
        }

        /// <summary>
        /// Converts value to the equivalent value of specified type.
        /// </summary>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        public static object ChangeType(object value, Type targetType)
        {
            return ChangeType(value, null, targetType);
        }

        /// <summary>
        /// Converts value to the equivalent value of specified type.
        /// </summary>
        /// <param name="targetType">The target conversion type.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="defaultValue">The default value of target type that should be used as empty value equivalent.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        public static object ChangeType(object value, object defaultValue, Type targetType)
        {
            // ReSharper disable RedundantNameQualifier
            if (object.Equals(value, string.Empty))
            // ReSharper restore RedundantNameQualifier
            {
                if (targetType == typeof(string))
                {
                    return value; //------------------------------------------RETURN---------------------------------------//
                }

                value = null;
            }

            if (value == null)
            {
                if (defaultValue != null)
                {
                    return defaultValue; //------------------------------------------RETURN---------------------------------------//
                }

                return GetNullValue(targetType); //------------------------------------------RETURN---------------------------------------//
            }

            targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            Type sourceType = value.GetType();
            sourceType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;

            if (targetType.IsAssignableFrom(sourceType))
            {
                return value; //------------------------------------------RETURN---------------------------------------//
            }

            if (targetType == typeof(Boolean))
            {
                if (sourceType == typeof(String))
                {
                    return ConvertStringToBool((String)value); //------------------------------------------RETURN---------------------------------------//
                }

                if (sourceType == typeof(int))
                {
                    return ConvertIntToBool((int)value); //------------------------------------------RETURN---------------------------------------//
                }
            }

            if (targetType == typeof(Guid) && sourceType == typeof(string))
            {
                return Guid.Parse((string)value);
            }

            if (targetType.IsEnum)
            {
                string strValue = value as string;
                if (strValue != null)
                {
                    return Enum.Parse(targetType, strValue, true); //------------------------------------------RETURN---------------------------------------//
                }

                return Enum.ToObject(targetType, value); //------------------------------------------RETURN---------------------------------------//
            }

            // ReSharper disable AssignNullToNotNullAttribute
            Type soursEnumerableType =
                (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ?
                sourceType :
                sourceType.GetInterface(typeof(IEnumerable).FullName, false);
            // ReSharper restore AssignNullToNotNullAttribute

            if (soursEnumerableType != null)
            {
                // ReSharper disable AssignNullToNotNullAttribute
                Type targetCollectionType =
                    (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ?
                    targetType :
                    targetType.GetInterface(typeof(IEnumerable<>).FullName, false);
                // ReSharper restore AssignNullToNotNullAttribute
                if (targetCollectionType != null)
                {
                    Type targetItemType = targetCollectionType.GetGenericArguments()[0];
                    if (targetType.IsArray || (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        var itemsList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(new[] { targetItemType }));
                        foreach (object item in (IEnumerable)value)
                        {
                            var convertedItem = ChangeType(targetItemType, item);
                            itemsList.Add(convertedItem);
                        }

                        MethodInfo toArrayMethod = typeof(Enumerable).GetMethod("ToArray", BindingFlags.Static | BindingFlags.Public);
                        toArrayMethod = toArrayMethod.MakeGenericMethod(new[] { targetItemType });

                        return toArrayMethod.Invoke(null, new object[] { itemsList }); //------------------------------------------RETURN---------------------------------------//
                    }

                    var targetCollection = Activator.CreateInstance(targetType);
                    MethodInfo addMethod = targetType.GetMethod("Add", new[] { targetItemType });

                    foreach (object item in (IEnumerable)value)
                    {
                        var convertedItem = ChangeType(targetItemType, item);
                        addMethod.Invoke(targetCollection, new[] { convertedItem });
                    }

                    return targetCollection; //------------------------------------------RETURN---------------------------------------//
                }
            }

            return Convert.ChangeType(value, targetType); //------------------------------------------RETURN---------------------------------------//
        }

        /// <summary>
        /// Converts the string value to bool value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        private static bool ConvertStringToBool(string value)
        {
            if (TrueStringValues.Contains(value, StringComparer.InvariantCultureIgnoreCase))
            {
                return true;
            }

            if (FalseStringValues.Contains(value, StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }

            throw new InvalidCastException(string.Format("Unable to cast {0} value to Boolean.", value));
        }

        /// <summary>
        /// Converts the string value to bool value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The conversion result value.
        /// </returns>
        private static bool ConvertIntToBool(int value)
        {
            if (value == 1)
            {
                return true;
            }

            if (value == 0)
            {
                return false;
            }

            throw new InvalidCastException(string.Format("Unable to cast {0} value to Boolean.", value));
        }

        /// <summary>
        /// Gets the null value of specified type.
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <returns>
        /// The null value for classes and default values for structures.
        /// </returns>
        private static object GetNullValue(Type targetType)
        {
            if (targetType.IsClass || Nullable.GetUnderlyingType(targetType) != null)
            {
                return null; // Default value for nullable type.
            }

            return Activator.CreateInstance(targetType); // Default value for not nullable struct.
        }

        #endregion
    }
}