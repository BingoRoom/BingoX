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
    public static class StringUtility
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
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
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
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Base64ToByte(string str)
        {
            return Convert.FromBase64String(str);
        }
        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FromBase64(string str, System.Text.Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(str));
        }
        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(string[] str)
        {
            return ToBase64(str, System.Text.Encoding.UTF8, " ");
        }

        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(string[] str, System.Text.Encoding encoding, string separator = " ")
        {
            var line = string.Join(separator, str);
            var buffer = encoding.GetBytes(line);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 移除StringBuilder最后一个字符
        /// </summary>
        /// <param name="builder"></param>
        public static void ReomveLastChar(this StringBuilder builder)
        {
            if (builder == null) return;
            builder.Remove(builder.Length - 1, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValues"></param>
        /// <param name="newValue"></param>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HasSpace(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (Char.IsWhiteSpace(value[0])) return true;
            if (Char.IsWhiteSpace(value[value.Length - 1])) return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSpace(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (HasSpace(value)) return value.Trim();
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveSpaceAll(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            return new string(value.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static string RemoveStartsWith(string str, char ch)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            var newstr = str.Trim();
            var index = newstr.TakeWhile(t => t == ch).Count();
            return index > 0 ? newstr.Substring(index) : newstr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
            if (String.IsNullOrWhiteSpace(str))
                return String.Empty;
            string lastStr = String.IsNullOrEmpty(replaceString) ? "..." : replaceString;
            return str.Length > index ? str.Substring(0, index) + lastStr : str;
        }


        /// <summary>
        /// 字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Array Cast(IEnumerable<string> value, Type type)
        {
            if (type == null || !value.HasAny())
                return null;
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
        /// 字符串转换类型
        /// </summary>
        /// <param name="value"></param> 
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
        ///  字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> Cast<T>(string value)
        {
            return Cast(value, default(T));
        }

        /// <summary>
        ///  字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typestring"></param>
        /// <returns></returns>
        public static TryResult<object> Cast(string value, string typestring)
        {
            return Cast(value, typestring, null);
        }

        /// <summary>
        ///  字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typestring"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TryResult<object> Cast(string value, string typestring, object defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            if (string.IsNullOrWhiteSpace(typestring))
                return defaultValue;
            var type = ReflectionUtility.GetType(typestring);
            if (type == null) return defaultValue;
            return Cast(value, type);
        }

        /// <summary>
        /// 字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TryResult<T> Cast<T>(string value, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            var obj = Cast(value, typeof(T));
            if (obj.Value is T)
                return (T)obj;
            return new TryResult<T>(obj.Error, defaultValue);
        }

        /// <summary>
        ///  字符串转换类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TryResult<object> Cast(string str, Type type, object defaultValue)
        {
            if (type == null)
                return new TryResult<object>(new ArgumentNullException("type"), defaultValue);
            if (type == typeof(string)) return str;
            bool isNullableType = type.IsNullable();
            var castType = type.RemoveNullable();
            object setvalue = defaultValue;
            if (castType.IsValueType && !isNullableType && setvalue == null)
                setvalue = Activator.CreateInstance(castType);
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
            if (StringValidator.IsNumeric(str) && TypeUtility.IsNumeric(castType)) return Convert.ChangeType(str, castType, null);
            try
            {
                switch (castType.FullName)
                {
                    case "System.Int32":
                        {
                            Int32 tmp;

                            if (Int32.TryParse(value, out tmp)) return tmp;

                            break;
                        }
                    case "System.Int64":
                        {
                            Int64 tmp;
                            if (Int64.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Int16":
                        {
                            Int16 tmp;
                            if (Int16.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt32":
                        {
                            UInt32 tmp;
                            if (UInt32.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt64":
                        {
                            UInt64 tmp;
                            if (UInt64.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.UInt16":
                        {
                            UInt16 tmp;
                            if (UInt16.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Decimal":
                        {
                            Decimal tmp;
                            if (Decimal.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Byte":
                        {
                            Byte tmp;
                            if (Byte.TryParse(value, out tmp)) return tmp;
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
                            Double tmp;
                            if (Double.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.String":
                        {
                            setvalue = value;
                            break;
                        }
                    case "System.Single":
                        {
                            Single tmp;
                            if (Single.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.Boolean":
                        {
                            if (value.Length == 1 && Char.IsNumber(value[0]))
                                setvalue = Char.GetNumericValue(value[0]) > 0;
                            else
                            {
                                Boolean tmp;
                                if (Boolean.TryParse(value, out tmp)) return tmp;
                            }
                            break;
                        }
                    case "System.Char":
                        {
                            Char tmp;
                            if (Char.TryParse(value, out tmp)) return tmp;
                            break;
                        }
                    case "System.SByte":
                        {
                            SByte tmp;
                            if (SByte.TryParse(value, out tmp)) return tmp;
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
                            if (converter.CanConvertFrom(typeof(string)))
                                return converter.ConvertFrom(value);
                            converter = TypeDescriptor.GetConverter(typeof(string));
                            if (converter.CanConvertTo(castType))
                                return converter.ConvertTo(value, castType);
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
        /// 转换失败后不调用TypeConverter的类
        /// </summary>
        /// <param name="type"></param>
        public static void TryAddIgnoreType(Type type)
        {
            if (type == null || IgnoreTypes.Contains(type)) return;
            IgnoreTypes.Add(type);
        }

        /// <summary>
        ///  字符串转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TryResult<object> Cast(string value, Type type)
        {
            return Cast(value, type, null);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Cast<T>(string[] text)
        {

            T[] arr = new T[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                arr[i] = Cast<T>(text[i]);

            }
            return arr;
        }
    }
}
