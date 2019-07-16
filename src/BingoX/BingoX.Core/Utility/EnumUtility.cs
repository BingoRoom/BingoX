using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    [Serializable]
    public class EnumConverterException : Exception
    {
        public EnumConverterException() { }
        public EnumConverterException(string message) : base(message) { }
        public EnumConverterException(string message, Exception inner) : base(message, inner) { }
        protected EnumConverterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// 提供一个针对枚举类的操作工具
    /// </summary>
    public class EnumUtility
    {
        /// <summary>
        /// 获取枚举类型的对象的项的注释
        /// </summary>
        /// <param name="value">枚举对象</param>
        /// <returns>注释</returns>
        public static string GetDescription(Enum value)
        {
            return GetDescription(value.GetType(), value.ToString());
        }
        /// <summary>
        /// 获取枚举类型的对象的项的注释
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举的值</param>
        /// <returns>注释</returns>
        public static string GetDescription<T>(int value)
        {
            if (typeof(T) != typeof(Enum)) throw new EnumConverterException("泛型必须为枚举类型");
            var obj = Enum.ToObject(typeof(T), value);
            var str = Enum.GetName(typeof(T), obj);
            if (str == null) throw new EnumConverterException("转换失败");

            var display = GetDescription<T>(str);
            return display;
        }
        /// <summary>
        /// 获取枚举类型的对象的项的注释
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">枚举的名称</param>
        /// <returns>注释</returns>
        public static string GetDescription<T>(string value)
        {
            if (typeof(T) != typeof(Enum)) throw new EnumConverterException("泛型必须为枚举类型");
            return GetDescription(typeof(T), value);
        }

        private static string GetDescription(Type enumtype, string value)
        {
            var fields = enumtype.GetFields();
            var field = fields.FirstOrDefault(n => n.Name == value);
            if (field == null) return value;

#if Standard
            var att = field.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
             
#else
            var att = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true).OfType<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();

#endif
            return att == null ? value : att.Description;
        }
    }
}
