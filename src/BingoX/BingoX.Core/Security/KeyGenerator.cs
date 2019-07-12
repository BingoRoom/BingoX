using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    public class KeyGenerator
    {
        /// <summary>  
        /// 随机生成秘钥（对称算法）  
        /// </summary>  
        /// <param name="key">秘钥(base64格式)</param>  
        /// <param name="iv">iv向量(base64格式)</param>  
        /// <param name="keySize">要生成的KeySize，每8个byte是一个字节，注意每种算法支持的KeySize均有差异，实际可通过输出LegalKeySizes来得到支持的值</param>  
        public static SymmetricKey CreateSymmetricAlgorithmKey<T>(int keySize) where T : SymmetricAlgorithm, new()
        {
            using (T provider = new T())
            {
                provider.KeySize = keySize;
                provider.GenerateIV();
                provider.GenerateKey();
                return new SymmetricKey() { IV = Convert.ToBase64String(provider.IV), Key = Convert.ToBase64String(provider.Key), LegalKeySizes = provider.LegalKeySizes, LegalBlockSizes = provider.LegalBlockSizes };
            }
        }
        /// <summary>  
        /// 随机生成秘钥（非对称算法）  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="publicKey">公钥（Xml格式）</param>  
        /// <param name="privateKey">私钥（Xml格式）</param>  
        /// <param name="provider">用于生成秘钥的非对称算法实现类，因为非对称算法长度需要在构造函数传入，所以这里只能传递算法类</param>  
        public static AsymmetricKey CreateAsymmetricAlgorithmKey<T>(int keySize) where T : AsymmetricAlgorithm, new()
        {
            using (T provider = new T())
            {
                provider.KeySize = keySize;
                return new AsymmetricKey { Public = provider.ToXmlString(false), Private = provider.ToXmlString(true), LegalKeySizes = provider.LegalKeySizes };
            }
        }
    }
}
