using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace BingoX.Utility
{
    /// <summary>
    /// 表示一个针对Object的操作工具
    /// </summary>
    public class ObjectUtility
    {
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
        /// <summary>
        /// 执行对象的数值自增
        /// </summary>
        /// <param name="number">可自增的对象</param>
        /// <returns>自增后的对象</returns>
        public static object MunberIncrement(object number)
        {
            TypeCode code = Convert.GetTypeCode(number);
            switch (code)
            {
                default:
                    throw new Exception("类型不能自增");
                case TypeCode.Single:
                    {
                        var a = (Single)number;
                        a++;
                        return a;
                    }
                case TypeCode.Byte:
                    {
                        var a = (int)number;
                        a++;
                        return a;
                    }
                case TypeCode.Char:
                    {
                        var a = (Char)number;
                        a++;
                        return a;
                    }

                case TypeCode.Decimal:
                    {
                        var a = (Decimal)number;
                        a++;
                        return a;
                    }
                case TypeCode.Double:
                    {
                        var a = (Double)number;
                        a++;
                        return a;
                    }
                case TypeCode.Int16:
                    {
                        var a = (Int16)number;
                        a++;
                        return a;
                    }
                case TypeCode.Int32:
                    {
                        var a = (Int32)number;
                        a++;
                        return a;
                    }
                case TypeCode.Int64:
                    {
                        var a = (Int64)number;
                        a++;
                        return a;
                    }
                case TypeCode.SByte:
                    {
                        var a = (SByte)number;
                        a++;
                        return a;
                    }
                case TypeCode.UInt16:
                    {
                        var a = (UInt16)number;
                        a++;
                        return a;
                    }
                case TypeCode.UInt32:
                    {
                        var a = (UInt32)number;
                        a++;
                        return a;
                    }
                case TypeCode.UInt64:
                    {
                        var a = (UInt64)number;
                        a++;
                        return a;
                    }
            }
        }

        /// <summary>
        /// 执行对象的数值自减
        /// </summary>
        /// <param name="number">可自减的对象</param>
        /// <returns>自减后的对象</returns>
        public static object MunberDecrement(object number)
        {

            TypeCode code = Convert.GetTypeCode(number);
            switch (code)
            {
                default:
                    throw new Exception("类型不能自增");
                case TypeCode.Single:
                    {
                        var a = (Single)number;
                        a--;
                        return a;
                    }
                case TypeCode.Byte:
                    {
                        var a = (Byte)number;
                        a--;
                        return a;
                    }
                case TypeCode.Char:
                    {
                        var a = (Char)number;
                        a--;
                        return a;
                    }
                case TypeCode.Decimal:
                    {
                        var a = (Decimal)number;
                        a--;
                        return a;
                    }
                case TypeCode.Double:
                    {
                        var a = (Double)number;
                        a--;
                        return a;
                    }
                case TypeCode.Int16:
                    {
                        var a = (Int16)number;
                        a--;
                        return a;
                    }
                case TypeCode.Int32:
                    {
                        var a = (Int32)number;
                        a--;
                        return a;
                    }
                case TypeCode.Int64:
                    {
                        var a = (Int64)number;
                        a--;
                        return a;
                    }
                case TypeCode.SByte:
                    {
                        var a = (SByte)number;
                        return a--;
                    }
                case TypeCode.UInt16:
                    {
                        var a = (UInt16)number;
                        a--;
                        return a;
                    }
                case TypeCode.UInt32:
                    {
                        var a = (UInt32)number;
                        a--;
                        return a;
                    }
                case TypeCode.UInt64:
                    {
                        var a = (UInt64)number;
                        a--;
                        return a;
                    }
            }
        }
    }
}
