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
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class RemarksAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remarks"></param>
        public RemarksAttribute(string remarks)
        {
            Remarks = remarks;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class ImageAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageAddress"></param>
        /// <param name="remarks"></param>
        public ImageAttribute(string imageAddress, string remarks)
        {
            ImageAddress = imageAddress;
            Remarks = remarks;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string ImageAddress { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; private set; }
    }
}
