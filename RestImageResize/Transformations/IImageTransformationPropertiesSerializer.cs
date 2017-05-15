using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using JetBrains.Annotations;
using OpenWaves;

namespace RestImageResize.Transformations
{
    [DefaultImplementation(typeof(ImageTransformationPropertiesSerializer))]
    public interface IImageTransformationPropertiesSerializer
    {
        string Serialize([NotNull] IDictionary<string, string> properties);
        IDictionary<string, string> Deserialize([NotNull] string serializedProperties);
        IDictionary<string, string> TryDeserialize([NotNull] string serializedProperties);
    }

    public class ImageTransformationPropertiesSerializer : IImageTransformationPropertiesSerializer
    {
        public virtual string Serialize(IDictionary<string, string> properties)
        {
            var keyValueArray = properties.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}").ToArray();
            var result = string.Join("&", keyValueArray);
            return result;
        }

        public virtual IDictionary<string, string> TryDeserialize(string serializedProperties)
        {
            if (serializedProperties == null) throw new ArgumentNullException(nameof(serializedProperties));

            serializedProperties = HttpUtility.UrlDecode(serializedProperties);
            
            var keyValueArray = serializedProperties.Split(new [] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var keyValueString in keyValueArray)
            {
                var pair = keyValueString.Split(new [] {'='}, 2, StringSplitOptions.None);
                
                if (pair.Length == 2)
                {
                    result[pair[0]] = pair[1];
                }
            }

            if (!result.ContainsKey("transform") || result["transform"].IsNullOrWhiteSpace())
                return null;

            return result;
        }

        public virtual IDictionary<string, string> Deserialize(string serializedProperties)
        {
            var result = TryDeserialize(serializedProperties);
            if (result == null)
                throw new FormatException($"Unable to parse serialized properties: '{serializedProperties}'");
            return result;
        }
    }

    public static class DictionaryUtil
    {
        public static TValue TryGetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key, TValue fallback = default(TValue))
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            if (key == null) throw new ArgumentNullException(nameof(key));

            TValue result;
            return dictionary.TryGetValue(key, out result) ? result : fallback;
        }
        
        public static IDictionary<string, string> ToPropertiesDictionary([NotNull] this RouteValueDictionary properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            var result = properties.ToDictionary(x => x.Key, p => p.Value?.ToString(), StringComparer.InvariantCultureIgnoreCase);
            return result;
        }

        public static IDictionary<string, string> ToPropertiesDictionary([NotNull] this object properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            return new RouteValueDictionary(properties).ToPropertiesDictionary();
        }
    }
}