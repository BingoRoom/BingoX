namespace BingoX.Security
{
    public class SecurityFactory
    {
        /// <summary>
        /// 使用指定配置创建3DES实例
        /// </summary>
        /// <param name="config">对称加密配置</param>
        /// <returns></returns>
        public static ISecurity CreateSecurity3DES(SymmetricAlgorithmConfig config)
        {
            return new Security3DES(config);
        }
        /// <summary>
        /// 使用指定配置创建MD5实例
        /// </summary>
        /// <param name="config">加密配置</param>
        /// <returns></returns>
        public static ISecurity CreateSecurityMD5(SecurityConfig config)
        {
            return new SecurityMD5(config);
        }
        /// <summary>
        /// 使用指定配置创建RSA实例
        /// </summary>
        /// <param name="config">加密配置</param>
        /// <returns></returns>
        public static IRSASecurity CreateSecurityRSA(AsymmetricAlgorithmConfig config)
        {
            return new SecurityRSA(config);
        }
        /// <summary>
        /// 使用指定配置创建AES实例
        /// </summary>
        /// <param name="config">对称加密配置</param>
        /// <returns></returns>
        public static ISecurity CreateSecurityAES(SymmetricAlgorithmConfig config)
        {
            return new SecurityAES(config);
        }

#if Framework

        public static IRSASecurity CreateSecurityDSA(AsymmetricAlgorithmConfig config)
        {
            return new SecurityDSA(config);
        }

#endif

    }
}
