using BingoX.Helper;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BingoX.Utility
{
    public static class StringUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static object Cast(string str, Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("str");
            var tmpType = targetType.RemoveNullabl();

            if (typeof(Guid) == tmpType && str.Length == 36) return Guid.Parse(str);

            if (tmpType.IsEnum) return Enum.Parse(tmpType, str);
            if (targetType == typeof(bool))
            {
                if (str == "1" ||
                    string.Equals(str, "on", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(str, "true", StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            //if (targetType == typeof(DateTime))
            //{
            //    if (str.Length == 10) return DateTime.ParseExact(str, "dd/MM/yyyy", null);
            //    if (str.Length == 19) return DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", null);
            //}
            if (typeof(IConvertible).IsAssignableFrom(tmpType)) return Convert.ChangeType(str, tmpType);

            var targetConvter = TypeDescriptor.GetConverter(tmpType);
            if (targetConvter == null || !targetConvter.CanConvertFrom(typeof(string))) throw new NotSupportedException();

            return targetConvter.ConvertFrom(str);
        }
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
    }
}
