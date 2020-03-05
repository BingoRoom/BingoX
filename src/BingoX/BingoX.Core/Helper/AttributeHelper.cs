using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Helper
{
    public static class AttributeHelper
    {

        /// <summary>
        /// 返回应用于此方法的指定自定义特性的集合
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T">特性类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            var ef = member.GetCustomAttributes(inherit);
            return ef.OfType<T>();
        }
        /// <summary>
        /// 返回应用于此方法的指定自定义特性
        /// </summary>
        /// <param name="member"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberInfo member, bool inherit = true) where T : Attribute
        {
            var ef = member.GetCustomAttributes<T>(inherit);
            if (ef != null && ef.Any()) return ef.FirstOrDefault();
            return null;
        }
        /// <summary>
        /// 返回应用于此程序集的指定自定义特性
        /// </summary>
        /// <param name="ass"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Assembly ass, bool inherit = true) where T : Attribute
        {
            var ef = ass.GetCustomAttributes<T>(inherit);
            if (ef != null && ef.Any()) return ef.FirstOrDefault();
            return null;
        }
        /// <summary>
        /// 返回应用于此程序集的指定自定义特性的集合
        /// </summary>
        /// <param name="ass"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Assembly ass, bool inherit = true) where T : Attribute
        {
            var ef = ass.GetCustomAttributes(inherit);
            return ef.OfType<T>();
        }
        /// <summary>
        /// 返回应用于此类型的指定自定义特性的集合
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type, bool inherit = true) where T : Attribute
        {
            var ef = type.GetCustomAttributes(inherit);
            return ef.OfType<T>();
        }
        /// <summary>
        /// 返回应用于此类型的指定自定义特性的集合,包含基类
        /// </summary>
        /// <param name="type">被检查的类型的实例</param>
        /// <param name="inherit">是否递归检查type的所有派生类</param>
        /// <typeparam name="T">指定自定义特性的类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributesIncludeBaseType<T>(this Type type, bool inherit = true) where T : Attribute
        {
            if (type == typeof(object)) return Utility.EmptyUtility<T>.EmptyArray;
          
            List<Type> alltype = new List<Type>() { };
            while (type != typeof(object))
            {
                alltype.Add(type);
                type = type.BaseType;
            }
            alltype.Reverse();
            return alltype.SelectMany(n => n.GetCustomAttributes<T>()).Distinct().ToArray();
        }

        /// <summary>
        /// 返回应用于此类型的指定自定义特性
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            var ef = type.GetCustomAttributes<T>(inherit);
            if (ef != null && ef.Any()) return ef.FirstOrDefault();
            return null;
        }
    } 
     
}
