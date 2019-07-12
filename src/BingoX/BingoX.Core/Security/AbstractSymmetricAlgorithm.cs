using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public abstract class AbstractSymmetricAlgorithm :  ISecurity
    {
        public SymmetricAlgorithmConfig Config { get; protected set; }

        protected abstract SymmetricAlgorithm Create();

        protected virtual void CheckConfig(SymmetricAlgorithmConfig config)
        {
            if (config.Encoding == null) config.Encoding = Encoding.UTF8;
            if (config.CipherMode == 0) config.CipherMode = CipherMode.ECB;
            if (config.PaddingMode == 0) config.PaddingMode = PaddingMode.PKCS7;
        }

        public virtual string Decrypt(string content)
        {
            byte[] bs = Config.DecryptByteToFunc(content);
            ICryptoTransform transform = Create().CreateDecryptor();
            var result = transform.TransformFinalBlock(bs, 0, bs.Length);
            return Config.Encoding.GetString(result);
        }

        public virtual string Encrypt(string content)
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
            byte[] bs = Config.Encoding.GetBytes(content);
            ICryptoTransform transform = Create().CreateEncryptor();
            var result = transform.TransformFinalBlock(bs, 0, bs.Length);
            return Config.EncryptByteToFunc(result);
        }

        public SecurityConfig GetConfig()
        {
            return Config;
        }
    }
}
