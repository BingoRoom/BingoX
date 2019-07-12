using System.Security.Cryptography;

namespace BingoX.Security
{
    /// <summary>
    /// 非对称加密密钥结构
    /// </summary>
    public struct AsymmetricKey
    {
        /// <summary>
        /// 公钥(XML格式)
        /// </summary>
        public string Public { get; set; }
        /// <summary>
        /// 私钥(XML格式)
        /// </summary>
        public string Private { get; set; }
        /// <summary>
        /// 获取支持的密钥长度信息。单位：bit
        /// </summary>
        public KeySizes[] LegalKeySizes { get; set; }
    }
}
