using BingoX.Utility;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public class SecurityMD5 :  ISecurity
    {
        public SecurityConfig Config { get; protected set; }

        public SecurityMD5(SecurityConfig config)
        {
            Config = config;
        }
        public SecurityMD5(Encoding encoding):this(new SecurityConfig { Encoding= encoding, ByteTo= ByteTo.Hex })
        {

        }
        string ISecurity.Decrypt(string content)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string Encrypt(string content)
        {
            if (StringUtility.HasChinese(content))
            {
                switch (Config.Encoding.EncodingName)
                {
                    case "US-ASCII":
                    case "Unicode": throw new SecurityException(string.Format("加密码信息包含中文，不能使用【{0}】编码进行加密码", Config.Encoding.EncodingName));
                    default:
                        break;
                }
            }
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes= md5Hasher.ComputeHash(Config.Encoding.GetBytes(content));
            return Config.EncryptByteToFunc(hashedDataBytes);
        }

        public SecurityConfig GetConfig()
        {
            return Config;
        }
    }
}
