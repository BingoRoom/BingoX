using BingoX.Helper;
using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using BingoX.Validation;

namespace BingoX.Utility
{
    /// <summary>
    /// 提供一个针对 String的操作工具
    /// </summary> 
    public class StringUtility
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="targetType"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentNullException"></exception>
        ///// <exception cref="NotSupportedException"></exception>
        //public static object Cast(string str, Type targetType)
        //{
        //    if (targetType == null) throw new ArgumentNullException("targetType");
        //    if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("str");
        //    var tmpType = targetType.RemoveNullabl();

        //    if (typeof(Guid) == tmpType && str.Length == 36) return Guid.Parse(str);

        //    if (tmpType.IsEnum) return Enum.Parse(tmpType, str);
        //    if (targetType == typeof(bool))
        //    {
        //        if (str == "1" ||
        //            string.Equals(str, "on", StringComparison.OrdinalIgnoreCase) ||
        //            string.Equals(str, "true", StringComparison.OrdinalIgnoreCase))
        //            return true;
        //    }
        //    //if (targetType == typeof(DateTime))
        //    //{
        //    //    if (str.Length == 10) return DateTime.ParseExact(str, "dd/MM/yyyy", null);
        //    //    if (str.Length == 19) return DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", null);
        //    //}
        //    if (typeof(IConvertible).IsAssignableFrom(tmpType)) return Convert.ChangeType(str, tmpType);

        //    var targetConvter = TypeDescriptor.GetConverter(tmpType);
        //    if (targetConvter == null || !targetConvter.CanConvertFrom(typeof(string))) throw new NotSupportedException();

        //    return targetConvter.ConvertFrom(str);
        //}

        /// <summary>
        /// 判断是不是中文
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] HexToByte(string str)
        {
            //todo 这里要写验证content是否为可转换的十六进制字符串
            str = str.Replace(" ", "");
            if ((str.Length % 2) != 0) str += " ";
            byte[] returnBytes = new byte[str.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] Base64ToByte(string str)
        {
            return Convert.FromBase64String(str);
        }
        /// <summary>
        /// 把Base64字符串转成对应编码的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string FromBase64(string str, Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(str));
        }
        /// <summary>
        /// 用空格字符拼接字符串数组，并且以UTF8编码转成Base64字符串
        /// </summary>
        /// <param name="strs">字符串数组</param>
        /// <returns></returns>
        public static string ToBase64(string[] strs)
        {
            return ToBase64(strs, Encoding.UTF8);
        }

        /// <summary>
        /// 用指定字符拼接字符串数组，并且以指定编码转成Base64字符串
        /// </summary>
        /// <param name="strs">字符串数组</param>
        /// <param name="encoding">指定编码</param>
        /// <param name="separator">指定连接符</param>
        /// <returns></returns>
        public static string ToBase64(string[] strs, Encoding encoding, string separator = " ")
        {
            var line = string.Join(separator, strs);
            var buffer = encoding.GetBytes(line);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 检索一个字符串，把指定的字符旧串替换为新字符串
        /// </summary>
        /// <param name="str">待替换字符串</param>
        /// <param name="oldValues">被替换的字符串集合</param>
        /// <param name="newValue">替换字符串</param>
        /// <returns></returns>
        public static string Replace(string str, string[] oldValues, string newValue)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            if (!oldValues.HasAny() || newValue == null) return str;
            var tme = str;
            foreach (var value in oldValues)
            {
                tme = tme.Replace(value, newValue);
            }
            return tme;
        }

        /// <summary>
        /// 判断字符串两头是否有空格
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool HasSpace(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (Char.IsWhiteSpace(value[0])) return true;
            if (Char.IsWhiteSpace(value[value.Length - 1])) return true;
            return false;
        }
        /// <summary>
        /// 移除字符串两头的空格
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static string RemoveSpace(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (HasSpace(value)) return value.Trim();
            return value;
        }
        /// <summary>
        /// 移除字符串中的所有空格字符
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static string RemoveSpaceAll(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return new string(value.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
        /// <summary>
        /// 移除以某字符为标志的前面的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ch">字符标志</param>
        /// <returns>移除后的字符串</returns>
        public static string RemoveStartsWith(string str, char ch)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var newstr = str.Trim();
            var index = newstr.TakeWhile(t => t == ch).Count();
            return index > 0 ? newstr.Substring(index) : newstr;
        }
        /// <summary>
        /// 移除以某字符串为标志的后面的字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="ch">字符标志</param>
        /// <returns>移除后的字符串</returns>
        public static string RemoveEndsWith(string str, char ch)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var newstr = str.Trim();
            var index = 0;
            for (int i = newstr.Length - 1; i >= 0; i--)
            {
                if (newstr[i] != ch) break;
                index++;
            }

            return index > 0 ? newstr.Substring(0, newstr.Length - index) : newstr;
        }

        /// <summary>
        /// 移除字符串的最后一个字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>移除后的字符串</returns>
        public static string RemoveLast(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return str.Remove(str.Length - 1, 1);
        }

        /// <summary>
        /// 获取字符串指定两个字符中间的值,正则
        /// </summary>
        /// <param name="beseString">字符串</param>
        /// <param name="startString">开始字符串</param>
        /// <param name="endString">结束字符串</param>
        /// <returns>项目值</returns>
        public static string GetStringMiddleEx(string beseString, string startString, string endString)
        {
            if (string.IsNullOrEmpty(beseString) || string.IsNullOrEmpty(startString) || string.IsNullOrEmpty(endString)) return string.Empty;
            try
            {
                // \s* 匹配0个或多个空白字符
                // .*? 匹配0个或多个除 "\n" 之外的任何字符(?指尽可能少重复)
                string regexStr = startString.Replace("*", @"\*").Replace("[", @"\[").Replace("]", @"\]") + @"\s*(?<key>.*?)" + endString.Replace("*", @"\*").Replace("[", @"\[").Replace("]", @"\]");
                Regex r = new Regex(regexStr, RegexOptions.IgnoreCase);
                if (r.IsMatch(beseString))
                {
                    Match mc = r.Match(beseString);
                    return mc.Groups["key"].Value;
                }
            }
            catch (Exception)
            {


            }
            return GetStringMiddle(beseString, startString, endString);
        }


        /// <summary>
        /// 返回字符串 指定 两个字符串中间的信息
        /// </summary>
        /// <param name="baseString">字符串</param>
        /// <param name="startString">开始段</param>
        /// <param name="endString">尾段</param>
        /// <returns></returns>
        public static string GetStringMiddle(string baseString, string startString, string endString)
        {
            if (baseString == null | startString == null || endString == null) return string.Empty;
            if (baseString.IndexOf(startString, StringComparison.CurrentCultureIgnoreCase) == -1 || baseString.IndexOf(endString, StringComparison.CurrentCultureIgnoreCase) == -1) return string.Empty;
            string lastPart = baseString.Substring(baseString.IndexOf(startString, StringComparison.CurrentCultureIgnoreCase) + startString.Length);
            var index = lastPart.IndexOf(endString, StringComparison.CurrentCultureIgnoreCase);
            if (index == -1) return string.Empty;
            return lastPart.Substring(0, index);
        }

        /// <summary>
        /// 对指定的长度字符串，超过指定上长替换处理。
        /// 方便显示内容。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <param name="replaceString">默认...</param>
        /// <returns></returns>
        public static string GetBriefnessString(string str, int index, string replaceString = "...")
        {
            if (String.IsNullOrWhiteSpace(str)) return String.Empty;
            string lastStr = String.IsNullOrEmpty(replaceString) ? "..." : replaceString;
            return str.Length > index ? str.Substring(0, index) + lastStr : str;
        }

        /// <summary>
        /// 将字符串枚举转型为对应类型的数组
        /// 把字符枚举里的字符串逐个转型成指定类型之后插入数组
        /// </summary>
        /// <param name="value">字符串枚举</param>
        /// <param name="type">指定类型</param>
        /// <returns>返回对应类型的数组</returns>
        public static Array Cast(IEnumerable<string> value, Type type)
        {
            if (type == null || !value.HasAny()) return null;
            var array = Array.CreateInstance(type, value.Count());
            int index = 0;
            foreach (string s in value)
            {
                var obj = Cast(s, type);
                array.SetValue(obj.Value, index);
                index++;
            }
            return array;
        }
        /// <summary>
        /// 将字符串枚举转型为对应类型的数组
        /// 把字符枚举里的字符串逐个转型成指定类型之后插入数组
        /// </summary>
        /// <param name="value">字符串枚举</param> 
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns></returns>
        public static T[] Cast<T>(IEnumerable<string> value)
        {
            if (!value.HasAny()) return CollectionHelper.EmptyOfArray<T>();
            var type = typeof(T);
            T[] array = new T[value.Count()];
            int index = 0;
            foreach (string s in value)
            {
                var obj = Cast(s, type);
                array.SetValue(obj.Value, index);
                index++;
            }
            return array;
        }
        /// <summary>
        ///  将字符串转换为指定类型的对象
        /// </summary>
        /// <param name="value">字符串</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>指定类型的对象的隐式转换</returns>
        public static TryResult<T> Cast<T>(string value)
        {
            return Cast(value, default(T));
        }

        /// <summary>
        ///  将字符串转换为指定类型的对象
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="typestring">指定类型的类型名称</param>
        /// <returns>指定类型的对象的隐式转换</returns>
        public static TryResult<object> Cast(string value, string typestring)
        {
            return Cast(value, typestring, null);
        }

        /// <summary>
        ///  将字符串转换为指定类型的对象，如转换失败则返回默认对象
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="typestring">指定类型的类型名称</param>
        /// <param name="defaultValue">默认对象</param>
        /// <returns>指定类型的对象或默认对象的隐式转换</returns>
        public static TryResult<object> Cast(string value, string typestring, object defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
            if (string.IsNullOrWhiteSpace(typestring)) return defaultValue;
            var type = ReflectionUtility.GetType(typestring);
            if (type == null) return defaultValue;
            return Cast(value, type);
        }

        /// <summary>
        /// 将字符串转换为指定类型的对象，如转换失败则返回默认对象
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="defaultValue">默认对象</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>指定类型的对象或默认对象的隐式转换</returns>
        public static TryResult<T> Cast<T>(string value, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value)) return defaultValue;
            var obj = Cast(value, typeof(T));
            if (obj.Value is T) return (T)obj;
            return new TryResult<T>(obj.Error, defaultValue);
        }

        /// <summary>
        ///  将字符串转换为指定类型的对象
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="type">指定类型</param>
        /// <returns>指定类型的隐式转换</returns>
        public static TryResult<object> Cast(string value, Type type)
        {
            return Cast(value, type, null);
        }

        /// <summary>
        /// 将字符串数组中的每一项转型为指定类型的数组
        /// </summary>
        /// <param name="text">字符串数组</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>指定类型数组</returns>
        public static T[] Cast<T>(string[] text)
        {
            T[] arr = new T[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                arr[i] = Cast<T>(text[i]);

            }
            return arr;
        }

        /// <summary>
        ///  将字符串转换为指定类型的对象，如转换失败则返回默认对象
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="type">指定类型</param>
        /// <param name="defaultValue">默认对象</param>
        /// <returns>指定类型的对象或默认对象的隐式转换</returns>
        public static TryResult<object> Cast(string str, Type type, object defaultValue)
        {
            if (type == null) return new TryResult<object>(new ArgumentNullException("type"), defaultValue);
            if (type == typeof(string)) return str;
            bool isNullableType = type.IsNullable();
            var castType = type.RemoveNullable();
            object setvalue = defaultValue;
            if (castType.IsValueType && !isNullableType && setvalue == null) setvalue = Activator.CreateInstance(castType);
            if (string.IsNullOrWhiteSpace(str)) return new TryResult<object>(new ArgumentNullException("str"), setvalue);
            string value = str.Trim();
            if (castType.IsEnum)
            {
                if (Enum.IsDefined(castType, str))
                {
                    return Enum.Parse(castType, str, true);
                }
                if (StringValidator.IsNumeric(str))
                {
                    var valueobj = Convert.ChangeType(str, Enum.GetUnderlyingType(castType), null);
                    if (Enum.IsDefined(castType, valueobj)) return Enum.ToObject(castType, valueobj);
                }
                var names = Enum.GetNames(castType);
                var neme = names.FirstOrDefault(n => string.Equals(n, str, StringComparison.OrdinalIgnoreCase));
                return Enum.Parse(castType, neme ?? str, true);
            }
            if (StringValidator.IsNumeric(str) && castType.IsNumberType()) return Convert.ChangeType(str, castType, null);
            try
            {
                switch (castType.FullName)
                {
                    case "System.Int32":
                        {
                            int tmp;
                            if (int.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Int64":
                        {
                            long tmp;
                            if (long.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Int16":
                        {
                            short tmp;
                            if (short.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt32":
                        {
                            uint tmp;
                            if (uint.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt64":
                        {
                            ulong tmp;
                            if (ulong.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt16":
                        {
                            ushort tmp;
                            if (ushort.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Decimal":
                        {
                            decimal tmp;
                            if (decimal.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Byte":
                        {
                            byte tmp;
                            if (byte.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Guid":
                        {
                            Guid tmp;
                            if (Guid.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Double":
                        {
                            double tmp;
                            if (double.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.String":
                        {
                            setvalue = value;
                            break;
                        }
                    case "System.Single":
                        {
                            float tmp;
                            if (float.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Boolean":
                        {
                            if (value.Length == 1 && char.IsNumber(value[0])) setvalue = char.GetNumericValue(value[0]) > 0;
                            else
                            {
                                bool tmp;
                                if (bool.TryParse(value, out tmp)) return tmp;
                            }
                            break;
                        }
                    case "System.Char":
                        {
                            char tmp;
                            if (char.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.SByte":
                        {
                            sbyte tmp;
                            if (sbyte.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.DateTime":
                        {
                            DateTime tmp;
                            if (DateTime.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.TimeSpan":
                        {
                            TimeSpan tmp;
                            if (TimeSpan.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Version":
                        {
                            Version tmp;
                            if (Version.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    default:
                        {
                            var tryf = type.InovkeType(ConvertConstructor, value);
                            if (tryf.Error == null)
                            {
                                return tryf.Value;
                            }
                            if (IgnoreTypes.Contains(castType)) return tryf.Error;
                            var converter = TypeDescriptor.GetConverter(castType);
                            if (converter.CanConvertFrom(typeof(string))) return converter.ConvertFrom(value);
                            converter = TypeDescriptor.GetConverter(typeof(string));
                            if (converter.CanConvertTo(castType)) return converter.ConvertTo(value, castType);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                return new TryResult<object>(e, setvalue);
            }
            return new TryResult<object>(new NotSupportedException(type.FullName), setvalue);
        }
        private static readonly Type[] ConvertConstructor = { typeof(string) };
        /// <summary>
        /// 转换失败后不调用TypeConverter的类列表
        /// </summary>
        private static readonly IList<Type> IgnoreTypes = new List<Type> { typeof(Uri) };
        /// <summary>
        /// 把指定类型配置到全局设置，使得转换失败后不调用TypeConverter的类
        /// </summary>
        /// <param name="type">指定类型</param>
        public static void TryAddIgnoreType(Type type)
        {
            if (type == null || IgnoreTypes.Contains(type)) return;
            IgnoreTypes.Add(type);
        }
    }
}
