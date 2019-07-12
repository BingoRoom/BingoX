using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    /// <summary>
    /// 非对称加密配置器
    /// </summary>
    public class AsymmetricAlgorithmConfig : SecurityConfig
    {
        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// 密钥类型
        /// </summary>
        public KeyTypeEnum KeyType { get; set; }

#if Standard

        public AsymmetricAlgorithmConfig()
        {
            Halg = HashAlgorithmName.SHA1;
            RsaEncryptionPadding = RSAEncryptionPadding.Pkcs1;
            RsaSignaturePadding = RSASignaturePadding.Pkcs1;
        }
        /// <summary>
        /// RSA加解密填充方式。默认PKCS1
        /// </summary>
        public RSAEncryptionPadding RsaEncryptionPadding { get; set; }
        /// <summary>
        /// RSA签名填充方式。默认PKCS1
        /// </summary>
        public RSASignaturePadding RsaSignaturePadding { get; set; }
        /// <summary>
        /// 用于创建数据哈希值的哈希算法的名称。默认：SHA1
        /// 如果为：SHA1，密钥长度不限制。
        /// 如果为：SHA256，密钥长度必须大于等于2048位。
        /// </summary>
        public HashAlgorithmName Halg { get; set; }

#else

        public AsymmetricAlgorithmConfig()
        {
            Halg = "SHA1";
        }
        /// <summary>
        /// 如果为 true，则使用 OAEP 填充，否则，如果为 false 则使用 PKCS#1 v1.5 填充。
        /// </summary>
        public bool FOAEP { get; set; }
        /// <summary>
        /// 用于创建数据哈希值的哈希算法的名称。默认SHA1
        /// 可选：MD5、SHA1、SHA256、SHA384、SHA512
        /// </summary>
        public string Halg { get; set; }

#endif

    }

    public enum KeyTypeEnum
    {
        /// <summary>
        /// 使用私钥加解密
        /// </summary>
        pri = 0,
        /// <summary>
        /// 使用公钥加解密
        /// </summary>
        pub = 1
    }
}
