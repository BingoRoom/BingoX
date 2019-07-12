using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
    /// <summary>
    /// 对称加密配置器
    /// 默认值：
    /// 加密模式（CipherMode）:ECB
    /// 填充模式（PaddingMode）:PKCS7
    /// 字符编码（Encoding）:UTF8
    /// 字节编码：Base64
    /// AES分组块大小：128
    /// </summary>
    public class SymmetricAlgorithmConfig : SecurityConfig
    {
        public SymmetricAlgorithmConfig()
        {
            CipherMode = CipherMode.ECB;
            PaddingMode = PaddingMode.PKCS7;
            AESBlockSize = 128;
        }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public string IV { get; set; }
        /// <summary>
        /// 块密码模式
        /// </summary>
        public CipherMode CipherMode { get; set; }
        /// <summary>
        /// AES块大小。（单位：bit）
        /// </summary>
        public int AESBlockSize { get; set; }
        /// <summary>
        /// 填充模式
        /// </summary>
        public PaddingMode PaddingMode { get; set; }



    }
}
