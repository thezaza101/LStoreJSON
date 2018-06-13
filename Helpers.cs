using System;
using System.Collections.Generic;

namespace LStoreJSON
{
    internal static class Helpers
    {
        internal static object GetKeyValeue<T>(this object input)
        {
            bool keyFound = false;
            dynamic output = "";
            foreach (System.Reflection.PropertyInfo info in typeof(T).GetProperties())
            {
                if (Attribute.IsDefined(info, typeof(System.ComponentModel.DataAnnotations.KeyAttribute)))
                {
                    keyFound = true;
                    output = info.GetValue(input);
                    break;
                }
            }
            if (!keyFound)
            {
                throw new KeyNotFoundException("Key attribute not found in " + typeof(T).ToString());
            }
            return (object)output;

        }
        internal static List<object> ConvertGenericTypeToObjectList<T>(this List<T> inList)
        {
            List<object> outputObject = new List<object>();
            inList.ForEach(a => outputObject.Add((object)a));
            return outputObject;
        }
        internal static List<T> ConvertObjectToGenericType<T>(this List<object> inList)
        {
            List<T> outputObject = new List<T>();
            inList.ForEach(a => outputObject.Add((T)a));
            return outputObject;
        }
        internal static dynamic ToDynamic<T>(this T input)
        {
            return (dynamic)input;
        }
    }
}