using System;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public class Security3DES : AbstractSymmetricAlgorithm
    {
        public Security3DES(SymmetricAlgorithmConfig config)  
        {
            Config = config;
        }

        protected override void CheckConfig(SymmetricAlgorithmConfig config)
        {
            base.CheckConfig(config);
            if (string.IsNullOrEmpty(Config.Key)) throw new SecurityException("3DES密匙为空");
            if (Config.CipherMode != CipherMode.ECB)
            {
                if (string.IsNullOrEmpty(Config.IV) || Config.IV.Length < 8) throw new SecurityException("偏移量最少：8字节长度");
            }
        }

        protected override SymmetricAlgorithm Create()
        {
            CheckConfig(Config);
            var des = new TripleDESCryptoServiceProvider();
            if (Config.CipherMode != CipherMode.ECB) des.IV = Config.Encoding.GetBytes(Config.IV);
            des.Key = Config.Encoding.GetBytes(Config.Key);
            des.Mode = Config.CipherMode;
            des.Padding = Config.PaddingMode;
            return des;
        }
    }
}
