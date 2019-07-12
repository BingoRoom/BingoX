using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace BingoX.Utility
{
    public static class ObjectUtility
    {

        public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            string value = collection.Get(key);
            if (value == null) return defaultValue;

            return Cast<T>(value, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Cast<T>(object value)
        {

            return (T)Cast(value, typeof(T), default(T));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Cast<T>(object value, T defaultValue)
        {

            return (T)Cast(value, typeof(T), defaultValue);

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static object Cast(object value, Type targetType, object defaultValue = null)
        {
            if (value == null) return defaultValue;
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (value == null) throw new ArgumentNullException("value");
            var tmpType = targetType.RemoveNullabl();

            if (tmpType.IsInstanceOfType(value)) return value;
            if (tmpType == typeof(string)) return value.ToString();

            var convertible = value as IConvertible;

            var code = Convert.GetTypeCode(value);
            switch (code)
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    throw new ArgumentNullException("value");

                case TypeCode.String:
                    return StringUtility.Cast((string)value, tmpType);

                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    {
                        if (tmpType.IsEnum)
                        {
                            return Enum.ToObject(tmpType, value);
                        }
                        if (tmpType == typeof(bool))
                        {
                            return (decimal)value > 0;
                        }
                        if (convertible != null) return Convert.ChangeType(value, tmpType);
                        break;
                    }
            }
            var valueType = value.GetType();
            var targetConvter = TypeDescriptor.GetConverter(tmpType);
            if (targetConvter.CanConvertFrom(valueType)) return targetConvter.ConvertFrom(value);
            var toConvter = TypeDescriptor.GetConverter(valueType);
            if (toConvter.CanConvertTo(tmpType)) return toConvter.ConvertTo(value, tmpType);

            throw new NotSupportedException();
        }
    }
}
