using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举的Remarks特性的信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Remarks(this Enum value)
        {
            FieldInfo info = value.GetType().GetField(value.ToString());
            var attributes = info.GetAttribute<System.ComponentModel.DescriptionAttribute>();
            return attributes == null ? value.ToString() : attributes.Description;
        }

        /// <summary>
        /// Removes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flag">Dumped flag</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	　
        /// </remarks>
        public static T ClearFlag<T>(this Enum variable, T flag)
        {
            return ClearFlags(variable, flag);
        }
        /// <summary>
        /// 把当前枚举对象的类型的所有枚举项转为对应的枚举对象，并返回枚举对象数组
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="variable">当前枚举对象</param>
        /// <returns></returns>
        public static TEnum[] GetValues<TEnum>(this Enum variable) where TEnum : struct
        {
            var array = variable.ToString().Split(',');
            if (array.Length == 0) return null;
            TEnum[] arr = new TEnum[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                TEnum t;
                if (Enum.TryParse(array[i], out t))
                {
                    arr[i] = t;
                }

            }
            return arr;
        }

        /// <summary>
        /// Removes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flags">Dumped flags</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 　
        /// </remarks>
        public static T ClearFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            result = flags.Aggregate(result, (current, flag) => current & ~Convert.ToUInt64(flag));
            return (T)Enum.Parse(variable.GetType(), result.ToString(), true);
        }

        /// <summary>
        /// Includes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flag">Established flag</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static T SetFlag<T>(this Enum variable, T flag)
        {
            return SetFlags(variable, flag);
        }

        /// <summary>
        /// Includes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flags">Established flags</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 　
        /// </remarks>
        public static T SetFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            result = flags.Aggregate(result, (current, flag) => current | Convert.ToUInt64(flag));
            return (T)Enum.Parse(variable.GetType(), result.ToString(), true);
        }
        /// <summary>
        /// 值是否在目标中
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="variable"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool InFlag<E>(this Enum variable, E flags) where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(E).IsEnum)  throw new ArgumentException("variable must be an Enum", "variable");
            var comper = Convert.ToInt64(variable);
            long numFlag = Convert.ToInt64(flags);
            if (numFlag == comper) return true;
            if ((comper & numFlag) == numFlag) return true;
            return false;
        }
        /// <summary>
        /// 值是否在目标中
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="variable"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool InFlag<E>(this Enum variable, params E[] flags) where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(E).IsEnum) throw new ArgumentException("variable must be an Enum", "variable");
            var comper = Convert.ToInt64(variable);
            foreach (long numFlag in flags.Select(flag => Convert.ToInt64(flag)))
            {
                if (numFlag == comper) return true;
                if ((comper & numFlag) == numFlag) return true;
            }
            return false;
        }
        /// <summary>
        /// 值是否在目标中
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="variable"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool NotInFlag<E>(this Enum variable, params E[] flags) where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(E).IsEnum) throw new ArgumentException("variable must be an Enum", "variable");
            var comper = Convert.ToInt64(variable);
            foreach (long numFlag in flags.Select(flag => Convert.ToInt64(flag)))
            {
                if (numFlag == comper) return false;
                if ((comper & numFlag) == comper) return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="flags"></param>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool HasFlags<E>(this E variable, params E[] flags) where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(E).IsEnum) throw new ArgumentException("variable must be an Enum", "variable");
            foreach (var flag in flags)
            {
                if (!Enum.IsDefined(typeof(E), flag)) return false;
                long numFlag = Convert.ToInt64(flag);
                if ((Convert.ToInt64(variable) & numFlag) != numFlag) return false;
            }
            return true;
        }
    }
}
