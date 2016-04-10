using System;
using System.Collections.Generic;
using System.Linq;

namespace Greenleaf
{
    public static class DictionaryExtensions
    {
         public static string ToRequestString(this IDictionary<string, object> dict)
         {
             return "?{0}".FormatWith(ToParametersString(dict));
         }

         public static string ToParametersString(this IDictionary<string, object> dict)
         {
             return string.Join("&", dict
                 .Where(x => x.Value != null)
                 .Select(x => "{0}={1}".FormatWith(x.Key, x.Value is string ? Uri.EscapeDataString((string)x.Value) : x.Value)));
         }

        public static T GetValue<T>(this IDictionary<string, string> source, string key)
        {
            string value;

            if (!source.TryGetValue(key, out value))
            {
                throw new KeyNotFoundException("No key found in dictionary");
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)bool.Parse(value);
            }
            
            if (typeof(T) == typeof(string))
            {
                return (T)(object)value;
            }
            
            if (typeof(T) == typeof (int))
            {
                return (T)(object)int.Parse(value);
            }

            throw new InvalidOperationException("No type to convert to supported");
        }
    }
}