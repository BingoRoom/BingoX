using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.Helper
{
    /// <summary>
    /// 提供对ISystemParameter接口的实现类的辅助
    /// </summary>
    public static class SystemParameterHelper
    {
        /// <summary>
        /// 把ISystemParameter的值转为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">ISystemParameter实例</param>
        /// <returns>指定类型的实例或指定类型的默认实例</returns>
        public static T Cast<T>(this ISystemParameter parameter) where T : IConvertible
        {
            return Cast<T>(parameter, default(T));

        }
        /// <summary>
        /// 把ISystemParameter的值转型为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">ISystemParameter实例</param>
        /// <param name="defaultValue">默认类型实例</param>
        /// <returns>指定类型的实例或指定类型的默认实例</returns>
        public static T Cast<T>(this ISystemParameter parameter, T defaultValue) where T : IConvertible
        {
            if (parameter == null || string.IsNullOrEmpty(parameter.ParameterValue)) return defaultValue;
            return ObjectUtility.Cast<T>(parameter.GetValue(), defaultValue);
        }
        /// <summary>
        /// 把ISystemParameter集合中指定的KEY的值转型为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">ISystemParameter集合</param>
        /// <param name="key">指定key</param>
        /// <param name="defaultValue">默认类型的实例</param>
        /// <returns></returns>
        public static T GetValue<T>(this IEnumerable<ISystemParameter> parameters, string key, T defaultValue) where T : IConvertible
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            var item = parameters.FirstOrDefault(n => n.ParameterKey == key);
            return Cast<T>(item, defaultValue);
        }
        /// <summary>
        /// 把ISystemParameter集合转为NameValueCollection集合
        /// </summary>
        /// <param name="parameters">ISystemParameter集合</param>
        /// <returns>NameValueCollection集合</returns>
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
        /// 把值为某分隔符拼接的字符串的ISystemParameter的值转型为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">ISystemParameter实例</param>
        /// <param name="split">分隔符</param>
        /// <returns>数组</returns>
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
    }
}
