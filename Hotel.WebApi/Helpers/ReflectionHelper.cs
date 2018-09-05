using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Hotel.Entities;

namespace Hotel.WebApi.Helpers
{
    public static class ReflectionHelper
    {
        // reflection namespace
        public static void getFriendlyNames()
        {
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Static);
            // sort properties by name
            Array.Sort(propertyInfos,
                delegate(PropertyInfo propertyInfo1, PropertyInfo propertyInfo2)
                {
                    return propertyInfo1.Name.CompareTo(propertyInfo2.Name);
                });

            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Console.WriteLine(propertyInfo.Name);
            }
        }

        public static string GetFriendlyNames<T>()
        {
            var propertyInfos = typeof(T).GetProperties().Where(s =>
                s.CustomAttributes.Any(d => d.AttributeType == typeof(DisplayNameAttribute)));

            // write property names
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                var atribute =
                    propertyInfo.CustomAttributes.FirstOrDefault(s => s.AttributeType == typeof(DisplayNameAttribute));

                var displayName = atribute.ConstructorArguments.FirstOrDefault().Value;

                Console.WriteLine(propertyInfo.Name);
            }

            return null;
        }
    }
}