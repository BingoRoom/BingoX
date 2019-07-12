using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BingoX.Security
{
    /// <summary>
    /// 安全器
    /// </summary>
    public static class SecurityExtension
    {
        static SecurityExtension()
        {
            DES = new Security3DES(new SymmetricAlgorithmConfig());
            MD5 = new SecurityMD5(new SecurityConfig() { ByteTo = ByteTo.Hex });
            AES = new SecurityAES(new SymmetricAlgorithmConfig());
            RSA = new SecurityRSA(new AsymmetricAlgorithmConfig());

#if Framework

            DSA = new SecurityDSA(new AsymmetricAlgorithmConfig());

#endif
        }

        /// <summary>
        /// 获取使用默认配置的3DES实例
        /// </summary>
        public static ISecurity DES { get; private set; }
        /// <summary>
        /// 获取使用默认配置的MD5实例
        /// </summary>
        public static ISecurity MD5 { get; private set; }
        /// <summary>
        /// 获取使用默认配置的AES实例
        /// </summary>
        public static ISecurity AES { get; private set; }

        /// <summary>
        /// 获取使用默认配置的RSA实例
        /// </summary>
        public static IRSASecurity RSA { get; private set; }

#if Framework

        /// <summary>
        /// 获取使用默认配置的DSA实例
        /// </summary>
        public static IRSASecurity DSA { get; private set; }

#endif

        /// <summary>
        /// 生成3DES密钥
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        public static SymmetricKey Generate3DESKey(int keySize)
        {
            return KeyGenerator.CreateSymmetricAlgorithmKey<TripleDESCryptoServiceProvider>(keySize);
        }
        /// <summary>
        /// 生成AES密钥
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        public static SymmetricKey GenerateAESKey(int keySize)
        {
            return KeyGenerator.CreateSymmetricAlgorithmKey<RijndaelManaged>(keySize);
        }
        /// <summary>
        /// 生成RSA密钥对
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        public static AsymmetricKey GenerateRSAKey(int keySize)
        {
            return KeyGenerator.CreateAsymmetricAlgorithmKey<RSACryptoServiceProvider>(keySize);
        }
    }
}
