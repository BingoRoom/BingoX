using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BingoX.Validation
{
    public class StringValidator
    {
        /// <summary>    
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true    
        /// </summary>    
        /// <param name="input">输入字符串</param>    
        /// <param name="pattern">模式字符串</param>            
        public static bool IsMatch(string input, string pattern)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;
            Regex reg1 = new Regex(pattern);
            return reg1.IsMatch(input);
        }

        /// <summary>
        /// 是否日期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDate(string str)
        {
            return IsMatch(str, ConstRegex.DateRegex);
        }

        /// <summary>
        /// 是否Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsGuid(string str)
        {
            return IsMatch(str, ConstRegex.GuidRegex);
        }

        /// <summary>
        /// 判断一个字符串是否为数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            return IsMatch(str, @"^[-]?\d+[.]?\d*$");
        }
        /// <summary>
        /// 是否IP地址
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ipAddress)
        {
            return IsMatch(ipAddress, ConstRegex.IPRegex);
        }

        /// <summary>
        /// 是否网络地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrl(string url)
        {
            return IsMatch(url, ConstRegex.UrlRegex);
        }
        /// <summary>
        /// 是否电子邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            return IsMatch(email, ConstRegex.EmailRegex);
        }

        /// <summary>
        /// 是否复杂电子邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsComplexEmail(string email)
        {
            Regex regex = new Regex(ConstRegex.ComplexEmailRegex, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }
    }
}
