using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX
{
    /// <summary>
    /// 常量值
    /// </summary>
    public struct ConstValue
    {
        /// <summary>
        /// 常用分割符
        /// </summary>
        public static readonly char[] SplitChars = { ',', '|' };
        /// <summary>
        /// 是否Mono环境
        /// </summary>
        public static readonly bool IsMono = Type.GetType("Mono.Runtime") != null;
        /// <summary>
        /// 汉字星期几
        /// </summary>
        public static readonly string[] ChinesseWeekDay = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        /// <summary>
        /// 最大缓存字节长度
        /// </summary>
        public const int MaxBuffterLength = 1024 * 1024;
        /// <summary>
        /// 失败代码
        /// </summary>
        public const int FailCode = -1;
        /// <summary>
        /// 
        /// </summary>
        public const BindingFlags DefaulBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
    }
}
