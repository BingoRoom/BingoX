using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public class SecurityConfig
    {
        /// <summary>
        /// 字符编码方式。默认UTF-8
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 字节数组转码方式。默认：Base64
        /// </summary>
        public ByteTo ByteTo { get; set; }
        /// <summary>
        /// 加密时的字节数据转码处理。转码规则取决于ByteTo的值
        /// </summary>
        public Func<byte[], string> EncryptByteToFunc;
        /// <summary>
        /// 解密时的字节数据转码处理。转码规则取决于ByteTo的值
        /// </summary>
        public Func<string, byte[]> DecryptByteToFunc;

        public SecurityConfig()
        {
            Encoding = Encoding.UTF8;
            EncryptByteToFunc = OnEncryptByteToFunc;
            DecryptByteToFunc = OnDecryptByteToFunc;
        }

        protected virtual string OnEncryptByteToFunc(byte[] x)
        {
            switch (ByteTo)
            {
                case ByteTo.Base64:
                    return ByteToBase64(x);
                case ByteTo.Hex:
                    return ByteToHex(x);
                default:
                    throw new SecurityException(string.Format("未扩展{0}类型的实现", ByteTo));
            }
        }

        protected virtual byte[] OnDecryptByteToFunc(string x)
        {
            switch (ByteTo)
            {
                case ByteTo.Base64:
                    return Base64ToByte(x);
                case ByteTo.Hex:
                    return HexToByte(x);
                default:
                    throw new SecurityException(string.Format("未扩展{0}类型的实现", ByteTo));
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected virtual string ByteToHex(byte[] bytes)
        {
            return ByteUtility.ByteToHex(bytes);
        }

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected virtual byte[] HexToByte(string content)
        {
            return StringUtility.HexToByte(content);
        }
        /// <summary>
        /// 字节数组转Base64字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected virtual string ByteToBase64(byte[] bytes)
        {
            return ByteUtility.ByteToBase64(bytes);
        }

        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        protected virtual byte[] Base64ToByte(string content)
        {
            return StringUtility.Base64ToByte(content);
        }
    }
    /// <summary>
    /// 字节数组转码方式枚举
    /// </summary>
    public enum ByteTo
    {
        Base64 = 0,
        Hex = 1
    }
}
