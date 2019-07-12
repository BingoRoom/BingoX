using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public abstract class AbstractASymmetricAlgorithm : IRSASecurity
    {
        public AsymmetricAlgorithmConfig Config { get; protected set; }

        protected virtual void CheckConfig(AsymmetricAlgorithmConfig config)
        {
            if (config.Encoding == null) config.Encoding = Encoding.UTF8;
            if (config.KeyType == KeyTypeEnum.pub && string.IsNullOrEmpty(config.PublicKey)) throw new SecurityException("公钥不能为空");
            if (config.KeyType == KeyTypeEnum.pri && string.IsNullOrEmpty(config.PrivateKey)) throw new SecurityException("私钥不能为空");
        }

        public abstract string Decrypt(string ciphertext);

        public abstract string Encrypt(string content);

        public SecurityConfig GetConfig()
        {
            return Config;
        }

        public abstract string SignData(string content);

        public abstract bool VerifyData(string content, string signature);
    }
}
