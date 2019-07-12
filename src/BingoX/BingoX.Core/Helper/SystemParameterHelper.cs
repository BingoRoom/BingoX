using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.Helper
{
    public static class SystemParameterHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static T Cast<T>(this ISystemParameter parameter) where T : IConvertible
        {
            return Cast<T>(parameter, default(T));

        }

        public static T GetValue<T>(this IEnumerable<ISystemParameter> parameters, string key, T defaultValue) where T : IConvertible
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            var item = parameters.FirstOrDefault(n => n.ParameterKey == key);
            return Cast<T>(item, defaultValue);
        }
        public static NameValueCollection CastNameValueCollection(this IEnumerable<ISystemParameter> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            NameValueCollection valueCollection = new NameValueCollection();
            foreach (var parameter in parameters)
            {
                valueCollection.Add(parameter.ParameterKey, parameter.GetValue());
            }
            return valueCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static T[] CastArray<T>(this ISystemParameter parameter, char split) where T : IConvertible
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.ParameterValue)) return new T[0];
            var arr = parameter.ParameterValue.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            T[] tarry = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                tarry[i] = ObjectUtility.Cast<T>(arr[i]);
            }
            return tarry;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Cast<T>(this ISystemParameter parameter, T defaultValue) where T : IConvertible
        {

            if (parameter == null || string.IsNullOrEmpty(parameter.ParameterValue)) return defaultValue;

            return ObjectUtility.Cast<T>(parameter.GetValue(), defaultValue);
        }
    }
}
