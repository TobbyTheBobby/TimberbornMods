using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace DynamicSpecifications
{
    public static class ExpandoObjectExtensions
    {
        public static object GetPropertyValue(this IDictionary<string, object> expando, string propertyName)
        {
            if (expando.TryGetValue(propertyName, out var value))
            {
                return value;
            }
            return null;
        }

        public static ExpandoObject ToExpandoObject(this JObject jObject)
        {
            ExpandoObject expando = new ExpandoObject();
            IDictionary<string, object> dictionary = expando;

            foreach (JProperty property in jObject.Properties())
            {
                if (property.Value is JObject valueObject)
                {
                    dictionary[property.Name] = valueObject.ToExpandoObject();
                }
                else if (property.Value is JArray valueArray)
                {
                    List<object> list = new List<object>();
                    foreach (JToken item in valueArray)
                    {
                        if (item is JObject itemObject)
                        {
                            list.Add(itemObject.ToExpandoObject());
                        }
                        else
                        {
                            list.Add(item.Value<object>());
                        }
                    }
                    dictionary[property.Name] = list;
                }
                else
                {
                    dictionary[property.Name] = property.Value.Value<object>();
                }
            }

            return expando;
        }
        
        public static IEnumerable<ExpandoObject> GetExpandoObjectList(this ExpandoObject expando, string propertyName)
        {
            IDictionary<string, object> dictionary = expando;
            if (dictionary.TryGetValue(propertyName, out object value))
            {
                if (value is IEnumerable<object> list)
                {
                    foreach (object item in list)
                    {
                        if (item is ExpandoObject expandoObject)
                        {
                            yield return expandoObject;
                        }
                    }
                }
            }
        }
        
        public static IEnumerable<object> GetValueList(this ExpandoObject expando, string propertyName)
        {
            IDictionary<string, object> dictionary = expando;
            if (dictionary.TryGetValue(propertyName, out object value))
            {
                if (value is IEnumerable<object> list)
                {
                    foreach (object item in list)
                    {
                        if (item is ExpandoObject) 
                            continue;
                        var jValue = (JValue)item;
                        yield return jValue.Value;
                    }
                }
            }
        }
    }
}