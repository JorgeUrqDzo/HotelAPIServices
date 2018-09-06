using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hotel.Services.Extensions
{
    public static class Extensions
    {
        public static string NullIfWhiteSpace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }

        public static T ToObject<T>(this IDictionary<string, string> dictionary) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (dictionary.ContainsKey(property.Name) == false)
                {
                    continue;
                }

                var value = dictionary[property.Name];

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = property.PropertyType; //t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                object convertedValue = Convert.ChangeType(value, newT);
                property.SetValue(t, convertedValue);
            }

            return t;
        }
    }
}