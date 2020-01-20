using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BingoX.Utility
{
    /// <summary>
    /// 表示一个针对Object的操作工具
    /// </summary>
    public class ObjectUtility
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="ignoreName"></param>
        public static void CopeyTo(object source, object destination, params string[] ignoreName)
        {
            if (source == null) throw new System.ArgumentNullException(nameof(source));
            if (destination == null) throw new System.ArgumentNullException(nameof(destination));
            var soucreProperties = source.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();
            foreach (var item in soucreProperties)
            {
                if (ignoreName != null && ignoreName.Contains(item.Name)) continue;
                var desProperty = destinationProperties.FirstOrDefault(n => n.Name == item.Name);
                if (desProperty == null) continue;
                var value = FastReflectionExtensions.FastGetValue(item, source);
                if (item.PropertyType != desProperty.PropertyType && value != null) value = ObjectUtility.Cast(value, desProperty.PropertyType, null);
                FastReflectionExtensions.FastSetValue(desProperty, destination, value);
            }
        }

        /// <summary>
        /// 把对象转型为指定的类型，如果转型失败则返回默认类型。
        /// 默认类型默认为null
        /// </summary>
        /// <param name="value">待转型的对象</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>目标类型对象</returns>
        public static T Cast<T>(object value)
        {
            return (T)Cast(value, typeof(T), default(T));
        }
        /// <summary>
        /// 把对象转型为指定的类型，如果转型失败则返回默认类型。
        /// 默认类型默认为null
        /// </summary>
        /// <param name="value">待转型的对象</param>
        /// <param name="defaultValue">默认类型</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>目标类型对象</returns>
        public static T Cast<T>(object value, T defaultValue)
        {
            return (T)Cast(value, typeof(T), defaultValue);
        }

        /// <summary>
        /// 把对象转型为指定的类型，如果转型失败则返回默认类型。
        /// 默认类型默认为null
        /// </summary>
        /// <param name="value">待转型的对象</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="defaultValue">默认类型</param>
        /// <returns>目标类型对象</returns>
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
