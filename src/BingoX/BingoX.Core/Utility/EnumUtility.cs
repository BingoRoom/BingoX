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
    public static class EnumUtility
    {
        //class EnumEntry
        //{

        //}
        //private IDictionary<Type,>
        public static string GetDescription(Enum value)
        {

            return GetDescription(value.GetType(), value.ToString());
        }
        public static string GetDescription<T>(int value) where T : Enum
        {

            var obj = Enum.ToObject(typeof(T), value);
            var str = Enum.GetName(typeof(T), obj);
            if (str == null) throw new EnumConverterException("转换失败");


            var display = GetDescription<T>(str);

            return display;
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

        public static string GetDescription<T>(string value) where T : Enum
        {
            return GetDescription(typeof(T), value);


        }
    }
}
