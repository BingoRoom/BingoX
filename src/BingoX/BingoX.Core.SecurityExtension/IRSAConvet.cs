using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Security
{
    /// <summary>
    /// RSA密钥转换接口
    /// </summary>
    public interface IRSAConvet
    {
        /// <summary>
        /// PKCS8私钥转XML私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_PKCS8_TO_XML(string privateKey);
        /// <summary>
        /// PKCS8公钥转XML公钥
        /// </summary>
        /// <returns></returns>
        string RSA_PublicKey_PKCS8_TO_XML(string publicKey);
        /// <summary>
        /// XML私钥转PKCS8私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_XML_TO_PKCS8(string privateKey);
        /// <summary>
        /// XML公钥转PKCS8公钥
        /// </summary>
        /// <returns></returns>
        string RSA_PublicKey_XML_TO_PKCS8(string publicKey);
        /// <summary>
        /// PKCS1私钥转XML私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_PKCS1_TO_XML(string privateKey);
        /// <summary>
        /// PKCS1公钥转XML公钥
        /// </summary>
        /// <returns></returns>
        string RSA_PublicKey_PKCS1_TO_XML(string publicKey);
        /// <summary>
        /// XML私钥转PKCS1私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_XML_TO_PKCS1(string privateKey);
        /// <summary>
        /// XML公钥转PKCS1公钥
        /// </summary>
        /// <returns></returns>
        string RSA_PublicKey_XML_TO_PKCS1(string publicKey);
        /// <summary>
        /// PKCS1私钥转PKCS8私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_PKCS1_TO_PKCS8(string privateKey);
        /// <summary>
        /// PKCS8私钥转PKCS1私钥
        /// </summary>
        /// <returns></returns>
        string RSA_PrivateKey_PKCS8_TO_PKCS1(string privateKey);
    }
}
