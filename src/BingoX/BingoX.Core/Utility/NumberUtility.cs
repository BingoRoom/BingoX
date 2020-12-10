using System;
using System.Text.RegularExpressions;

namespace BingoX.Utility
{
    public static class NumberUtility
    {

        public static string ToChineseNumber(decimal value)
        {
            var s = value.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
        public static string ToChineseNumber(double value)
        {

            var s = value.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;

        }
        /// <summary>
        /// 获取decimal的值
        /// </summary>
        /// <param name="d">当前decimal</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static decimal Round(decimal d, int digits)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return Math.Round(d, digits, MidpointRounding.AwayFromZero);
        }



        /// <summary>
        /// 获取double的值
        /// </summary>
        /// <param name="d">当前double</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static double Round(double d, int digits)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return Math.Round(d, digits, MidpointRounding.AwayFromZero);
        }


    }
}
