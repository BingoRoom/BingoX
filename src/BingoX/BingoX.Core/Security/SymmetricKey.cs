using System.Security.Cryptography;

namespace BingoX.Security
{
    /// <summary>
    /// 对称加密密钥结构
    /// </summary>
    public struct SymmetricKey
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public string IV { get; set; }
        /// <summary>
        /// 获取支持的密钥长度信息。单位：bit
        /// </summary>
        public KeySizes[] LegalKeySizes { get; set; }
        /// <summary>
        /// 获取支持的加密块大小信息。单位：bit
        /// </summary>
        public KeySizes[] LegalBlockSizes { get; set; }
    }
}
