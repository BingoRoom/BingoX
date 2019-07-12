using BingoX.Utility;
using System;
using System.Security.Cryptography;

namespace BingoX.Security
{
    public class SecurityAES : AbstractSymmetricAlgorithm
    {
        public SecurityAES(SymmetricAlgorithmConfig config)
        {
            Config = config;
        }

        protected override void CheckConfig(SymmetricAlgorithmConfig config)
        {
            base.CheckConfig(config);
            if (string.IsNullOrEmpty(Config.Key))
            {
                throw new SecurityException("密钥不能为空");
            }
            if (Config.CipherMode != CipherMode.ECB)
            {
                if (string.IsNullOrEmpty(Config.IV)) throw new SecurityException("偏移量不能为空");
                if(Config.IV.Length < Config.AESBlockSize / 8) throw new SecurityException(string.Format("偏移量最小不能低于{0}位", Config.AESBlockSize / 8));
            }
        }

        /// <summary>  
        /// 创建一个统一配置的加密算法。  
        /// </summary>   
        /// <returns>RijndaelManaged</returns>  
        protected override SymmetricAlgorithm Create()
        {
            CheckConfig(Config);
            RijndaelManaged rm = new RijndaelManaged();
            if(Config.CipherMode != CipherMode.ECB) rm.IV = Config.Encoding.GetBytes(Config.IV);
            rm.Key = Config.Encoding.GetBytes(Config.Key);
            rm.Mode = Config.CipherMode;
            rm.Padding = Config.PaddingMode;
            rm.BlockSize = Config.AESBlockSize;
            return rm;
        }
    }
}
