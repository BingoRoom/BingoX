using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Utility
{
    public static class ByteUtility
    {
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHex(byte[] bytes)
        {
            StringBuilder returnStr = new StringBuilder();
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr.Append(bytes[i].ToString("x2"));
                }
            }
            return returnStr.ToString();
        }

        /// <summary>
        /// 字节数组转Base64字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
