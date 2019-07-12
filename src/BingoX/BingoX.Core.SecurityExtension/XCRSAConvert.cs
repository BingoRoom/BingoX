using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if Standard
using XC.RSAUtil;
#endif

namespace BingoX.Security
{
#if Standard
    /// <summary>
    /// XC.RSAUtil提供的密钥转换实现
    /// </summary>
    public class XCRSAConvert : IRSAConvet
    {
        public string RSA_PrivateKey_PKCS1_TO_PKCS8(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyPkcs1ToPkcs8(privateKey).Replace("-----BEGIN RSA PRIVATE KEY-----\r\n", "").Replace("-----END RSA PRIVATE KEY-----\r\n", "");
        }

        public string RSA_PrivateKey_PKCS1_TO_XML(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyPkcs1ToXml(privateKey);
        }

        public string RSA_PrivateKey_PKCS8_TO_PKCS1(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyPkcs8ToPkcs1(privateKey).Replace("-----BEGIN RSA PRIVATE KEY-----\r\n", "").Replace("-----END RSA PRIVATE KEY-----\r\n", "");
        }

        public string RSA_PrivateKey_PKCS8_TO_XML(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyPkcs8ToXml(privateKey);
        }

        public string RSA_PrivateKey_XML_TO_PKCS1(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyXmlToPkcs1(privateKey).Replace("-----BEGIN RSA PRIVATE KEY-----\r\n", "").Replace("-----END RSA PRIVATE KEY-----\r\n", "");
        }

        public string RSA_PrivateKey_XML_TO_PKCS8(string privateKey)
        {
            return RsaKeyConvert.PrivateKeyXmlToPkcs8(privateKey).Replace("-----BEGIN RSA PRIVATE KEY-----\r\n", "").Replace("-----END RSA PRIVATE KEY-----\r\n", "");
        }

        public string RSA_PublicKey_PKCS1_TO_XML(string publicKey)
        {
            return RsaKeyConvert.PublicKeyPemToXml(publicKey);
        }

        public string RSA_PublicKey_PKCS8_TO_XML(string publicKey)
        {
            return RsaKeyConvert.PublicKeyPemToXml(publicKey);
        }

        public string RSA_PublicKey_XML_TO_PKCS1(string publicKey)
        {
            return RsaKeyConvert.PublicKeyXmlToPem(publicKey).Replace("-----BEGIN PUBLIC KEY-----\r\n", "").Replace("-----END PUBLIC KEY-----\r\n", "");
        }

        public string RSA_PublicKey_XML_TO_PKCS8(string publicKey)
        {
            return RsaKeyConvert.PublicKeyXmlToPem(publicKey).Replace("-----BEGIN PUBLIC KEY-----\r\n", "").Replace("-----END PUBLIC KEY-----\r\n", "");
        }
    }
#endif
}
