using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Aop.Api.Util
{
    public class AlipaySignature
    {
        /** 默认编码字符集 */
        private static string DEFAULT_CHARSET = "GBK";

        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, string signType)
        {
            string signContent = GetSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, signType);
        }

        public static string RSASign(string data, string privateKeyPem, string charset, string signType)
        {
            return RSASignCharSet(data, privateKeyPem, charset, signType);
        }
        ///*
        public static string RSASign(IDictionary<string, string> parameters, string privateKeyPem, string charset, bool keyFromFile, string signType)
        {
            string signContent = GetSignContent(parameters);

            return RSASignCharSet(signContent, privateKeyPem, charset, keyFromFile, signType);
        }

        public static string RSASign(string data, string privateKeyPem, string charset, string signType, bool keyFromFile)
        {
            return RSASignCharSet(data, privateKeyPem, charset, keyFromFile, signType);
        }
        //*/
        public static string RSASignCharSet(string data, string privateKeyPem, string charset, string signType)
        {
            RSAHelper helper = new RSAHelper("RSA2".Equals(signType) ? RSAType.RSA2 : RSAType.RSA, Encoding.GetEncoding(charset), privateKeyPem);
            return helper.Sign(data);
            //RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);
            //byte[] dataBytes = null;
            //if (string.IsNullOrEmpty(charset))
            //{
            //    dataBytes = Encoding.UTF8.GetBytes(data);
            //}
            //else
            //{
            //    dataBytes = Encoding.GetEncoding(charset).GetBytes(data);
            //}


            //if ("RSA2".Equals(signType))
            //{

            //    byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA256");

            //    return Convert.ToBase64String(signatureBytes);

            //}
            //else
            //{
            //    byte[] signatureBytes = rsaCsp.SignData(dataBytes, "SHA1");

            //    return Convert.ToBase64String(signatureBytes);
            //}
        }


        public static string RSASignCharSet(string data, string privateKeyPem, string charset, bool keyFromFile, string signType)
        {
            try
            {
                string sprivateKeyPem;
                if (keyFromFile)
                {
                    sprivateKeyPem = File.ReadAllText(privateKeyPem);
                    sprivateKeyPem = sprivateKeyPem.Replace("-----BEGIN PRIVE KEY-----\r\n", string.Empty);
                    sprivateKeyPem = sprivateKeyPem.Replace("-----END PRIVE KEY-----\r\n\r\n", string.Empty);
                }
                else
                {

                    sprivateKeyPem = privateKeyPem;
                }
                RSAHelper helper = new RSAHelper("RSA2".Equals(signType) ? RSAType.RSA2 : RSAType.RSA, Encoding.GetEncoding(charset), privateKeyPem);
                return helper.Sign(data);


            }
            catch (Exception ex)
            {
                throw new AopException("您使用的私钥格式错误，请检查RSA私钥配置" + ",charset = " + charset, ex);
            }
            //       return Convert.ToBase64String(signatureBytes);
        }


        public static bool RSACheckV1(IDictionary<string, string> parameters, string publicKeyPem, string charset)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            parameters.Remove("sign_type");
            string signContent = GetSignContent(parameters);
            return RSACheckContent(signContent, sign, publicKeyPem, charset, "RSA");
        }

        public static bool RSACheckV1(IDictionary<string, string> parameters, string publicKeyPem)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            parameters.Remove("sign_type");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, DEFAULT_CHARSET, "RSA");
        }

        public static bool RSACheckV1(IDictionary<string, string> parameters, string publicKeyPem, string charset, string signType, bool keyFromFile)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            parameters.Remove("sign_type");
            string signContent = GetSignContent(parameters);
            return RSACheckContent(signContent, sign, publicKeyPem, charset, signType, keyFromFile);
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, DEFAULT_CHARSET, "RSA");
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem, string charset)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, charset, "RSA");
        }

        public static bool RSACheckV2(IDictionary<string, string> parameters, string publicKeyPem, string charset, string signType, bool keyFromFile)
        {
            string sign = parameters["sign"];

            parameters.Remove("sign");
            string signContent = GetSignContent(parameters);

            return RSACheckContent(signContent, sign, publicKeyPem, charset, signType, keyFromFile);
        }

        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType)
        {

            try
            {
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                RSAHelper helper = new RSAHelper("RSA2".Equals(signType) ? RSAType.RSA2 : RSAType.RSA, Encoding.GetEncoding(charset), null, publicKeyPem);
                return helper.Verify(signContent, sign);
                //if ("RSA2".Equals(signType))
                //{
                //    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);

                //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //    rsa.PersistKeyInCsp = false;
                //    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                //    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));
                //    return bVerifyResultOriginal;

                //}
                //else
                //{
                //    string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //    rsa.PersistKeyInCsp = false;
                //    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                //    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                //    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));
                //    return bVerifyResultOriginal;
                //}
            }
            catch
            {
                return false;
            }

        }
        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, string signType, bool keyFromFile)
        {

            try
            {
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }

                string sPublicKeyPEM;
                if (keyFromFile)
                {
                    sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                    sPublicKeyPEM = sPublicKeyPEM.Replace("-----BEGIN PUBLIC KEY-----\r\n", string.Empty);
                    sPublicKeyPEM = sPublicKeyPEM.Replace("-----END PUBLIC KEY-----\r\n\r\n", string.Empty);
                }
                else
                {

                    sPublicKeyPEM = publicKeyPem;
                }

                RSAHelper helper = new RSAHelper("RSA2".Equals(signType) ? RSAType.RSA2 : RSAType.RSA, Encoding.GetEncoding(charset), null, sPublicKeyPEM);
                return helper.Verify(signContent, sign);
                //if (keyFromFile)
                //{
                //    sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                //}
                //else
                //{
                //    sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
                //    sPublicKeyPEM += publicKeyPem;
                //    sPublicKeyPEM += "-----END PUBLIC KEY-----\r\n\r\n";
                //}
                //if ("RSA2".Equals(signType))
                //{
                //        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //        rsa.PersistKeyInCsp = false;
                //        RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                //        bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));
                //        return bVerifyResultOriginal;
                //}
                //else
                //{
                //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //    rsa.PersistKeyInCsp = false;
                //    RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                //    SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                //    bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));
                //    return bVerifyResultOriginal;
                //}
            }
            catch
            {
                return false;
            }

        }
        public static bool RSACheckContent(string signContent, string sign, string publicKeyPem, string charset, bool keyFromFile)
        {
            try
            {
                string sPublicKeyPEM;
                if (keyFromFile)
                {
                    sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                }
                else
                {
                    sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
                    sPublicKeyPEM = sPublicKeyPEM + publicKeyPem;
                    sPublicKeyPEM = sPublicKeyPEM + "-----END PUBLIC KEY-----\r\n\r\n";
                }
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), sha1, Convert.FromBase64String(sign));
                return bVerifyResultOriginal;
            }
            catch (Exception ex)
            {
                string s = ex.Message.ToString();
                return false;
            }

        }

        public static string CheckSignAndDecrypt(IDictionary<string, string> parameters, string alipayPublicKey,
                                             string cusPrivateKey, bool isCheckSign,
                                             bool isDecrypt)
        {
            string charset = parameters["charset"];
            string bizContent = parameters["biz_content"];
            if (isCheckSign)
            {
                if (!RSACheckV2(parameters, alipayPublicKey, charset))
                {
                    throw new AopException("rsaCheck failure:rsaParams=" + parameters);
                }
            }

            if (isDecrypt)
            {
                return RSADecrypt(bizContent, cusPrivateKey, charset, "RSA");
            }

            return bizContent;
        }

        public static string CheckSignAndDecrypt(IDictionary<string, string> parameters, string alipayPublicKey,
                                             string cusPrivateKey, bool isCheckSign,
                                             bool isDecrypt, string signType, bool keyFromFile)
        {
            string charset = parameters["charset"];
            string bizContent = parameters["biz_content"];
            if (isCheckSign)
            {
                if (!RSACheckV2(parameters, alipayPublicKey, charset, signType, keyFromFile))
                {
                    throw new AopException("rsaCheck failure:rsaParams=" + parameters);
                }
            }

            if (isDecrypt)
            {
                return RSADecrypt(bizContent, cusPrivateKey, charset, signType, keyFromFile);
            }

            return bizContent;
        }

        public static string encryptAndSign(string bizContent, string alipayPublicKey,
                                        string cusPrivateKey, string charset, bool isEncrypt,
                                        bool isSign, string signType, bool keyFromFile)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(charset))
            {
                charset = DEFAULT_CHARSET;
            }
            sb.Append("<?xml version=\"1.0\" encoding=\"" + charset + "\"?>");
            if (isEncrypt)
            {// 加密
                sb.Append("<alipay>");
                String encrypted = RSAEncrypt(bizContent, alipayPublicKey, charset, keyFromFile);
                sb.Append("<response>" + encrypted + "</response>");
                sb.Append("<encryption_type>" + signType + "</encryption_type>");
                if (isSign)
                {
                    String sign = RSASign(encrypted, cusPrivateKey, charset, signType, keyFromFile);
                    sb.Append("<sign>" + sign + "</sign>");
                    sb.Append("<sign_type>" + signType + "</sign_type>");
                }
                sb.Append("</alipay>");
            }
            else if (isSign)
            {// 不加密，但需要签名
                sb.Append("<alipay>");
                sb.Append("<response>" + bizContent + "</response>");
                String sign = RSASign(bizContent, cusPrivateKey, charset, signType, keyFromFile);
                sb.Append("<sign>" + sign + "</sign>");
                sb.Append("<sign_type>" + signType + "</sign_type>");
                sb.Append("</alipay>");
            }
            else
            {// 不加密，不加签
                sb.Append(bizContent);
            }
            return sb.ToString();
        }

        public static string encryptAndSign(string bizContent, string alipayPublicKey,
                                        string cusPrivateKey, string charset, bool isEncrypt,
                                        bool isSign)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(charset))
            {
                charset = DEFAULT_CHARSET;
            }
            sb.Append("<?xml version=\"1.0\" encoding=\"" + charset + "\"?>");
            if (isEncrypt)
            {// 加密
                sb.Append("<alipay>");
                String encrypted = RSAEncrypt(bizContent, alipayPublicKey, charset);
                sb.Append("<response>" + encrypted + "</response>");
                sb.Append("<encryption_type>RSA</encryption_type>");
                if (isSign)
                {
                    String sign = RSASign(encrypted, cusPrivateKey, charset, "RSA");
                    sb.Append("<sign>" + sign + "</sign>");
                    sb.Append("<sign_type>RSA</sign_type>");
                }
                sb.Append("</alipay>");
            }
            else if (isSign)
            {// 不加密，但需要签名
                sb.Append("<alipay>");
                sb.Append("<response>" + bizContent + "</response>");
                String sign = RSASign(bizContent, cusPrivateKey, charset, "RSA");
                sb.Append("<sign>" + sign + "</sign>");
                sb.Append("<sign_type>RSA</sign_type>");
                sb.Append("</alipay>");
            }
            else
            {// 不加密，不加签
                sb.Append(bizContent);
            }
            return sb.ToString();
        }

        public static string RSAEncrypt(string content, string publicKeyPem, string charset)
        {
            try
            {
                string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
                int maxBlockSize = rsa.KeySize / 8 - 11; //加密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsa.Encrypt(data, false);
                    return Convert.ToBase64String(cipherbytes);
                }
                MemoryStream plaiStream = new MemoryStream(data);
                MemoryStream crypStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toEncrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
                    Byte[] cryptograph = rsa.Encrypt(toEncrypt, false);
                    crypStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                }

                return Convert.ToBase64String(crypStream.ToArray(), Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                throw new AopException("EncryptContent = " + content + ",charset = " + charset, ex);
            }
        }
        public static string RSAEncrypt(string content, string publicKeyPem, string charset, bool keyFromFile)
        {
            try
            {
                string sPublicKeyPEM;
                if (keyFromFile)
                {
                    sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                }
                else
                {
                    sPublicKeyPEM = "-----BEGIN PUBLIC KEY-----\r\n";
                    sPublicKeyPEM += publicKeyPem;
                    sPublicKeyPEM += "-----END PUBLIC KEY-----\r\n\r\n";
                }
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Encoding.GetEncoding(charset).GetBytes(content);
                int maxBlockSize = rsa.KeySize / 8 - 11; //加密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsa.Encrypt(data, false);
                    return Convert.ToBase64String(cipherbytes);
                }
                MemoryStream plaiStream = new MemoryStream(data);
                MemoryStream crypStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toEncrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
                    Byte[] cryptograph = rsa.Encrypt(toEncrypt, false);
                    crypStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = plaiStream.Read(buffer, 0, maxBlockSize);
                }

                return Convert.ToBase64String(crypStream.ToArray(), Base64FormattingOptions.None);
            }
            catch (Exception ex)
            {
                throw new AopException("EncryptContent = " + content + ",charset = " + charset, ex);
            }
        }

        public static string RSADecrypt(string content, string privateKeyPem, string charset, string signType)
        {
            try
            {
                RSACryptoServiceProvider rsaCsp = LoadCertificateFile(privateKeyPem, signType);
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Convert.FromBase64String(content);
                int maxBlockSize = rsaCsp.KeySize / 8; //解密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsaCsp.Decrypt(data, false);
                    return Encoding.GetEncoding(charset).GetString(cipherbytes);
                }
                MemoryStream crypStream = new MemoryStream(data);
                MemoryStream plaiStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toDecrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toDecrypt, 0, blockSize);
                    Byte[] cryptograph = rsaCsp.Decrypt(toDecrypt, false);
                    plaiStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                }

                return Encoding.GetEncoding(charset).GetString(plaiStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new AopException("DecryptContent = " + content + ",charset = " + charset, ex);
            }
        }

        public static string RSADecrypt(string content, string privateKeyPem, string charset, string signType, bool keyFromFile)
        {
            try
            {
                RSACryptoServiceProvider rsaCsp = null;
                if (keyFromFile)
                {
                    //文件读取
                    rsaCsp = LoadCertificateFile(privateKeyPem, signType);
                }
                else
                {
                    //字符串获取
                    rsaCsp = LoadCertificateString(privateKeyPem, signType);
                }
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }
                byte[] data = Convert.FromBase64String(content);
                int maxBlockSize = rsaCsp.KeySize / 8; //解密块最大长度限制
                if (data.Length <= maxBlockSize)
                {
                    byte[] cipherbytes = rsaCsp.Decrypt(data, false);
                    return Encoding.GetEncoding(charset).GetString(cipherbytes);
                }
                MemoryStream crypStream = new MemoryStream(data);
                MemoryStream plaiStream = new MemoryStream();
                Byte[] buffer = new Byte[maxBlockSize];
                int blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                while (blockSize > 0)
                {
                    Byte[] toDecrypt = new Byte[blockSize];
                    Array.Copy(buffer, 0, toDecrypt, 0, blockSize);
                    Byte[] cryptograph = rsaCsp.Decrypt(toDecrypt, false);
                    plaiStream.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = crypStream.Read(buffer, 0, maxBlockSize);
                }

                return Encoding.GetEncoding(charset).GetString(plaiStream.ToArray());
            }
            catch (Exception ex)
            {
                throw new AopException("DecryptContent = " + content + ",charset = " + charset, ex);
            }
        }

        private static byte[] GetPem(string type, byte[] data)
        {
            string pem = Encoding.UTF8.GetString(data);
            string header = String.Format("-----BEGIN {0}-----\\n", type);
            string footer = String.Format("-----END {0}-----", type);
            int start = pem.IndexOf(header) + header.Length;
            int end = pem.IndexOf(footer, start);
            string base64 = pem.Substring(start, (end - start));

            return Convert.FromBase64String(base64);
        }

        private static RSACryptoServiceProvider LoadCertificateFile(string filename, string signType)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(filename))
            {
                byte[] data = new byte[fs.Length];
                byte[] res = null;
                fs.Read(data, 0, data.Length);
                if (data[0] != 0x30)
                {
                    res = GetPem("RSA PRIVATE KEY", data);
                }
                try
                {
                    RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(res, signType);
                    return rsa;
                }
                catch (Exception )
                {
                }
                return null;
            }
        }
        private static RSACryptoServiceProvider LoadCertificateString(string strKey, string signType)
        {
            byte[] data = null;
            //读取带
            //ata = Encoding.Default.GetBytes(strKey);
            data = Convert.FromBase64String(strKey);
            //data = GetPem("RSA PRIVATE KEY", data);
            try
            {
                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
                return rsa;
            }
            catch (Exception ex)
            {
                throw new AopException("EncryptContent = woshihaoren,zheshiyigeceshi,wanerde", ex);
            }
           
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception )
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }

    public class RSAHelper
    {
        private readonly RSA _privateKeyRsaProvider;
        private readonly RSA _publicKeyRsaProvider;
        private readonly HashAlgorithmName _hashAlgorithmName;
        private readonly Encoding _encoding;

        /// <summary>
        /// 实例化RSAHelper
        /// </summary>
        /// <param name="rsaType">加密算法类型 RSA SHA1;RSA2 SHA256 密钥长度至少为2048</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="publicKey">公钥</param>
        public RSAHelper(RSAType rsaType, Encoding encoding, string privateKey, string publicKey = null)
        {
            _encoding = encoding;
            if (!string.IsNullOrEmpty(privateKey))
            {
                _privateKeyRsaProvider = CreateRsaProviderFromPrivateKey(privateKey);
            }

            if (!string.IsNullOrEmpty(publicKey))
            {
                _publicKeyRsaProvider = CreateRsaProviderFromPublicKey(publicKey);
            }

            _hashAlgorithmName = rsaType == RSAType.RSA ? HashAlgorithmName.SHA1 : HashAlgorithmName.SHA256;
        }

        #region 使用私钥签名

        /// <summary>
        /// 使用私钥签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <returns></returns>
        public string Sign(string data)
        {
            byte[] dataBytes = _encoding.GetBytes(data);

            var signatureBytes = _privateKeyRsaProvider.SignData(dataBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        #endregion

        #region 使用公钥验证签名

        /// <summary>
        /// 使用公钥验证签名
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public bool Verify(string data, string sign)
        {
            byte[] dataBytes = _encoding.GetBytes(data);
            byte[] signBytes = Convert.FromBase64String(sign);

            var verify = _publicKeyRsaProvider.VerifyData(dataBytes, signBytes, _hashAlgorithmName, RSASignaturePadding.Pkcs1);

            return verify;
        }

        #endregion

        #region 解密

        public string Decrypt(string cipherText)
        {
            if (_privateKeyRsaProvider == null)
            {
                throw new Exception("_privateKeyRsaProvider is null");
            }
            return Encoding.UTF8.GetString(_privateKeyRsaProvider.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1));
        }

        #endregion

        #region 加密

        public string Encrypt(string text)
        {
            if (_publicKeyRsaProvider == null)
            {
                throw new Exception("_publicKeyRsaProvider is null");
            }
            return Convert.ToBase64String(_publicKeyRsaProvider.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
        }

        #endregion

        #region 使用私钥创建RSA实例

        public RSA CreateRsaProviderFromPrivateKey(string privateKey)
        {
            var privateKeyBits = Convert.FromBase64String(privateKey);

            var rsa = RSA.Create();
            var rsaParameters = new RSAParameters();

            using (BinaryReader binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                rsaParameters.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.D = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.P = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.Q = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DP = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.DQ = binr.ReadBytes(GetIntegerSize(binr));
                rsaParameters.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(rsaParameters);
            return rsa;
        }

        #endregion

        #region 使用公钥创建RSA实例

        public RSA CreateRsaProviderFromPublicKey(string publicKeyString)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            var x509Key = Convert.FromBase64String(publicKeyString);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsa = RSA.Create();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }
        }

        #endregion

        #region 导入密钥算法

        private int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
            if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        #endregion

    }

    /// <summary>
    /// RSA算法类型
    /// </summary>
    public enum RSAType
    {
        /// <summary>
        /// SHA1
        /// </summary>
        RSA = 0,
        /// <summary>
        /// RSA2 密钥长度至少为2048
        /// SHA256
        /// </summary>
        RSA2
    }
}
