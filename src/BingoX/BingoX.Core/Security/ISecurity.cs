namespace BingoX.Security
{
    /// <summary>
    /// 加密/解密接口
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content">明文</param>
        /// <returns></returns>
        string Encrypt(string content);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        string Decrypt(string ciphertext);
        /// <summary>
        /// 获取配置信息（根据实际情况转型为对应的配置信息类）
        /// </summary>
        /// <returns></returns>
        SecurityConfig GetConfig();
    }
}
