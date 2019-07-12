using BingoX.Security;
using BingoX.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Test.Security
{
    [Author("Dason")]
    [TestFixture]
    public class SecurityTest
    {
        [OneTimeSetUp]
        public void Setup()
        {

        }
        [Test]
        [TestCase("123", "202cb962ac59075b964b07152d234b70", "202cb962ac59075b964b07152d234b70", "202cb962ac59075b964b07152d234b70", TestName = "TestMD5")]
        [TestCase("让出", "0ab6aeba88de5a4a0a896c03be3434ae", "", "4e0a8d105c552bb07b99f7d6ed72be11", TestName = "TestMD5Chinese")]
        public void TestMD5Security(string content, string md5result1, string md5result2, string md5result3)
        {
            var md5 = SecurityExtension.MD5.Encrypt(content);
            StringAssert.AreEqualIgnoringCase(md5, md5result1);

            if (!string.IsNullOrEmpty(md5result2))
            {
                md5 = new SecurityMD5(Encoding.ASCII).Encrypt(content);
                StringAssert.AreEqualIgnoringCase(md5, md5result2);
            }

            md5 = SecurityFactory.CreateSecurityMD5(new SecurityConfig() { Encoding = Encoding.GetEncoding("gb2312"), ByteTo = ByteTo.Hex }).Encrypt(content);
            StringAssert.AreEqualIgnoringCase(md5, md5result3);
        }

        [Test]
        public void TestMD5Chinese_Throw()
        {
            var md5 = new SecurityMD5(Encoding.ASCII);
            Assert.That(() =>
            {
                md5.Encrypt("让出");
            }, Throws.Exception.TypeOf<BingoX.Security.SecurityException>());
        }

        [Test]
        [TestCase("123", "A98DABA075424446BD2CE1F9", "a5eChWjWKrg=", "a5eChWjWKrg=", "a5eChWjWKrg=", TestName = "TestDes")]
        [TestCase("让出", "A98DABA075424446BD2CE1F9", "rIshpnqJYQM=", "", "2XhR3Asow5k=", TestName = "TestDesChinese")]
        public void TestDESSecurity(string content, string key, string result1, string result2, string result3)
        {
            var securityDES = SecurityExtension.DES;
            var config = (SymmetricAlgorithmConfig)securityDES.GetConfig();
            config.Key = key;
            var descontent = securityDES.Encrypt(content);
            StringAssert.AreEqualIgnoringCase(descontent, result1);


            if (!string.IsNullOrEmpty(result2))
            {
                descontent = new Security3DES(new SymmetricAlgorithmConfig { Key = key, Encoding = Encoding.ASCII }).Encrypt(content);
                StringAssert.AreEqualIgnoringCase(descontent, result2);

            }


            descontent = SecurityFactory.CreateSecurity3DES(new SymmetricAlgorithmConfig { Key = key, Encoding = Encoding.GetEncoding("gb2312") }).Encrypt(content);

            StringAssert.AreEqualIgnoringCase(descontent, result3);
        }
        [Test]

        [TestCase("让出", "A98DABA075424446BD2CE1F9", TestName = "TestDesChinese_Throw")]
        public void TestDESDecryptThrow(string content, string key)
        {
            var securityDES = SecurityFactory.CreateSecurity3DES(new SymmetricAlgorithmConfig { Key = key, Encoding = Encoding.ASCII });

            Assert.That(() =>
            {
                securityDES.Encrypt(content);
            },
            Throws.Exception.TypeOf<BingoX.Security.SecurityException>());
        }


        [Test]
        [TestCase("123", "A98DABA075424446BD2CE1F9", "a5eChWjWKrg=", "a5eChWjWKrg=", "a5eChWjWKrg=", TestName = "TestDesDecrypt")]
        [TestCase("让出", "A98DABA075424446BD2CE1F9", "rIshpnqJYQM=", "", "2XhR3Asow5k=", TestName = "TestDesChineseDecrypt")]
        public void TestDESDecrypt(string content, string key, string result1, string result2, string result3)
        {
            var securityDES = SecurityExtension.DES;
            var config = (SymmetricAlgorithmConfig)securityDES.GetConfig();
            config.Key = key;
            var descontent = securityDES.Decrypt(result1);
            StringAssert.AreEqualIgnoringCase(descontent, content);

            if (!string.IsNullOrEmpty(result2))
            {
                securityDES = new Security3DES(new SymmetricAlgorithmConfig { Key = key, Encoding = Encoding.ASCII });
                descontent = securityDES.Decrypt(result2);

                StringAssert.AreEqualIgnoringCase(descontent, content);
            }
            securityDES = SecurityFactory.CreateSecurity3DES(new SymmetricAlgorithmConfig { Key = key, Encoding = Encoding.GetEncoding("gb2312") });
            descontent = securityDES.Decrypt(result3);

            StringAssert.AreEqualIgnoringCase(descontent, content);
        }

        [Test]
        [TestCase("123", "12345678901234567890123456789012", "", 2, "srwfzn5PzJQ9Gzx8a0HFgA==", "srwfzn5PzJQ9Gzx8a0HFgA==", "srwfzn5PzJQ9Gzx8a0HFgA==", TestName = "TestAES")]
        [TestCase("123", "12345678901234567890123456789012", "1234567890123456", 1, "GI/BxeCs2v3Edz/BTuWENA==", "GI/BxeCs2v3Edz/BTuWENA==", "GI/BxeCs2v3Edz/BTuWENA==", TestName = "TestAES_VI")]
        [TestCase("让出", "12345678901234567890123456789012", "", 2, "d0rgp0ccr33e10KYvB5Oyg==", "", "FuBcWgGjA1LKqiLKfXhtfQ==", TestName = "TestAESChinese")]
        [TestCase("让出", "12345678901234567890123456789012", "1234567890123456", 1, "/gHtVC25YxTd04Ca2bEJYQ==", "", "6OC4DvO1fEgECMKhOb18Lg==", TestName = "TestAESChinese_VI")]
        public void TestAESSecurity(string content, string key, string iv, int cipherMode, string result1, string result2, string result3)
        {

            var securityAES = BingoX.Security.SecurityExtension.AES;
            var config = (SymmetricAlgorithmConfig)securityAES.GetConfig();
            config.Key = key;
            config.IV = iv;
            config.CipherMode = (CipherMode)cipherMode;
            var descontent = securityAES.Encrypt(content);
            StringAssert.AreEqualIgnoringCase(descontent, result1);


            if (!string.IsNullOrEmpty(result2))
            {
                securityAES = new SecurityAES(new SymmetricAlgorithmConfig { Key = key, IV = iv, Encoding = Encoding.ASCII, CipherMode = (CipherMode)cipherMode });
                descontent = securityAES.Encrypt(content);
                StringAssert.AreEqualIgnoringCase(descontent, result2);
            }

            securityAES = SecurityFactory.CreateSecurityAES(new SymmetricAlgorithmConfig { Key = key, IV = iv, Encoding = Encoding.GetEncoding("gb2312"), CipherMode = (CipherMode)cipherMode });
            descontent = securityAES.Encrypt(content);

            StringAssert.AreEqualIgnoringCase(descontent, result3);
        }
        [Test]
        [TestCase("让出", "12345678901234567890123456789012", "", 2, TestName = "TestAESChinese_Throw")]
        [TestCase("让出", "12345678901234567890123456789012", "1234567890123456", 1, TestName = "TestAESChinese_Throw_VI")]
        public void TestAESSecurityThrow(string content, string key, string iv, int cipherMode)
        {
            SecurityAES securityAES = new SecurityAES(new SymmetricAlgorithmConfig { Key = key, IV = iv, Encoding = Encoding.ASCII, CipherMode = (CipherMode)cipherMode });
            Assert.That(() =>
            {
                securityAES.Encrypt(content);
            }, Throws.Exception.TypeOf<BingoX.Security.SecurityException>());

        }
        [Test]
        [TestCase("123", "12345678901234567890123456789012", "", 2, "srwfzn5PzJQ9Gzx8a0HFgA==", "srwfzn5PzJQ9Gzx8a0HFgA==", "srwfzn5PzJQ9Gzx8a0HFgA==", TestName = "TestAES_Decrypt")]
        [TestCase("123", "12345678901234567890123456789012", "1234567890123456", 1, "GI/BxeCs2v3Edz/BTuWENA==", "GI/BxeCs2v3Edz/BTuWENA==", "GI/BxeCs2v3Edz/BTuWENA==", TestName = "TestAES_Decrypt_VI")]
        [TestCase("让出", "12345678901234567890123456789012", "", 2, "d0rgp0ccr33e10KYvB5Oyg==", "", "FuBcWgGjA1LKqiLKfXhtfQ==", TestName = "TestAESChinese_Decrypt")]
        [TestCase("让出", "12345678901234567890123456789012", "1234567890123456", 1, "/gHtVC25YxTd04Ca2bEJYQ==", "", "6OC4DvO1fEgECMKhOb18Lg==", TestName = "TestAESChinese_Decrypt_VI")]
        public void TestAESDecrypt(string content, string key, string iv, int cipherMode, string result1, string result2, string result3)
        {
            var securityAES = BingoX.Security.SecurityExtension.AES;
            var config = (SymmetricAlgorithmConfig)securityAES.GetConfig();
            config.Key = key;
            config.IV = iv;
            config.CipherMode = (CipherMode)cipherMode;
            var descontent = securityAES.Decrypt(result1);
            StringAssert.AreEqualIgnoringCase(descontent, content);


            if (!string.IsNullOrEmpty(result2))
            {
                securityAES = new SecurityAES(new SymmetricAlgorithmConfig { Key = key, IV = iv, Encoding = Encoding.ASCII, CipherMode = (CipherMode)cipherMode });
                descontent = securityAES.Decrypt(result2);
                StringAssert.AreEqualIgnoringCase(descontent, content);
            }

            securityAES = SecurityFactory.CreateSecurityAES(new SymmetricAlgorithmConfig { Key = key, IV = iv, Encoding = Encoding.GetEncoding("gb2312"), CipherMode = (CipherMode)cipherMode });
            descontent = securityAES.Decrypt(result3);
            StringAssert.AreEqualIgnoringCase(descontent, content);
        }

        [Test]
        [TestCase(128, TestName = "TestGenerateKey_128")]
        [TestCase(192, TestName = "TestGenerateKey_192")]
        [TestCase(256, TestName = "TestGenerateKey_256")]
        [TestCase(13, TestName = "TestGenerateKey_13")]
        public void TestGenerateSymmetricAlgorithmKey(int keysize)
        {
            //3DES minsize:128;maxsize:192;skipSize:64
            //AES minsize:128;maxsize:256;skipSize:64
            switch (keysize)
            {
                case 128:
                    var DesKey128 = SecurityExtension.Generate3DESKey(keysize);
                    Assert.AreEqual(24, DesKey128.Key.Length);
                    Assert.AreEqual(12, DesKey128.IV.Length);
                    var AesKey128 = SecurityExtension.GenerateAESKey(keysize);
                    Assert.AreEqual(24, DesKey128.Key.Length);
                    Assert.AreEqual(12, DesKey128.IV.Length);
                    break;
                case 192:
                    var DesKey192 = SecurityExtension.Generate3DESKey(keysize);
                    Assert.AreEqual(32, DesKey192.Key.Length);
                    Assert.AreEqual(12, DesKey192.IV.Length);
                    var AesKey192 = SecurityExtension.GenerateAESKey(keysize);
                    Assert.AreEqual(32, AesKey192.Key.Length);
                    Assert.AreEqual(24, AesKey192.IV.Length);
                    break;
                case 256:
                    Assert.That(() =>
                    {
                        var Key = SecurityExtension.Generate3DESKey(keysize);
                    }, Throws.Exception.TypeOf<CryptographicException>());
                    var AesKey256 = SecurityExtension.GenerateAESKey(keysize);
                    Assert.AreEqual(44, AesKey256.Key.Length);
                    Assert.AreEqual(24, AesKey256.IV.Length);
                    break;
                default:
                    Assert.That(() =>
                    {
                        var Key = SecurityExtension.Generate3DESKey(keysize);
                    }, Throws.Exception.TypeOf<CryptographicException>());
                    Assert.That(() =>
                    {
                        var Key = SecurityExtension.GenerateAESKey(keysize);
                    }, Throws.Exception.TypeOf<CryptographicException>());
                    break;
            }
        }

        [Test]
        [TestCase(384, TestName = "TestGenerateAsKey_384")]
        [TestCase(392, TestName = "TestGenerateAsKey_392")]
        [TestCase(1024, TestName = "TestGenerateAsKey_1024")]
        [TestCase(2048, TestName = "TestGenerateAsKey_2048")]
        [TestCase(16384, TestName = "TestGenerateAsKey_16384")]
        [TestCase(168, TestName = "TestGenerateAsKey_168")]
        [TestCase(16884, TestName = "TestGenerateAsKey_16884")]
        public void TestGenerateAsymmetricAlgorithmKey(int keysize)
        {
            //PlatformNotSupportedException 
            //AES minsize:384;maxsize:16384;skipSize:8
            if (keysize == 168 || keysize == 16884)
            {
                Assert.That(() =>
                {
                    var Key = SecurityExtension.GenerateRSAKey(keysize);
                }, Throws.Exception.TypeOf<CryptographicException>());
            }
            else
            {
                var Key = SecurityExtension.GenerateRSAKey(keysize);
            }

        }

        [Test]
        [TestCase("How ary you", "TestRSA_PKCS1_1024", TestName = "TestRSA_PKCS1_1024")]
        [TestCase("How ary you 你好啊！你是谁。", "TestRSA_PKCS1_1024_Chinese", TestName = "TestRSA_PKCS1_1024_Chinese")]
        [TestCase("How ary you", "TestRSA_PKCS1_2048", TestName = "TestRSA_PKCS1_2048")]
        [TestCase("How ary you 你好啊！你是谁。", "TestRSA_PKCS1_2048_Chinese", TestName = "TestRSA_PKCS1_2048_Chinese")]
        public void TestRSA(string content, string tag)
        {
            var rsa = SecurityExtension.RSA;
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            AsymmetricKey key;
            switch (tag)
            {
                case "TestRSA_PKCS1_1024":
                case "TestRSA_PKCS1_1024_Chinese":
                    key = KeyGenerator.CreateAsymmetricAlgorithmKey<RSACryptoServiceProvider>(1024);
                    break;
                case "TestRSA_PKCS1_2048":
                case "TestRSA_PKCS1_2048_Chinese":
                    key = KeyGenerator.CreateAsymmetricAlgorithmKey<RSACryptoServiceProvider>(2048);
                    break;
                default:
                    break;
            }
            config.PrivateKey = key.Private;
            config.PublicKey = key.Public;
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "签名失败");
        }

        [Test]
        [TestCase("How ary you", "TestRSA2_PKCS1_1024", TestName = "TestRSA2_PKCS1_1024")]
        [TestCase("How ary you 你好啊！你是谁。", "TestRSA2_PKCS1_1024_Chinese", TestName = "TestRSA2_PKCS1_1024_Chinese")]
        [TestCase("How ary you", "TestRSA2_PKCS1_2048", TestName = "TestRSA2_PKCS1_2048")]
        [TestCase("How ary you 你好啊！你是谁。", "TestRSA2_PKCS1_2048_Chinese", TestName = "TestRSA2_PKCS1_2048_Chinese")]
        public void TestRSA2(string content, string tag)
        {
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = "SHA256" });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            AsymmetricKey key;
            switch (tag)
            {
                case "TestRSA2_PKCS1_1024":
                case "TestRSA2_PKCS1_1024_Chinese":
                    key = KeyGenerator.CreateAsymmetricAlgorithmKey<RSACryptoServiceProvider>(1024);
                    break;
                case "TestRSA2_PKCS1_2048":
                case "TestRSA2_PKCS1_2048_Chinese":
                    key = KeyGenerator.CreateAsymmetricAlgorithmKey<RSACryptoServiceProvider>(2048);
                    break;
                default:
                    break;
            }
            config.PrivateKey = key.Private;
            config.PublicKey = key.Public;
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "签名失败");
        }
    }
}
