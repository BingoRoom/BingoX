using BingoX.Security;
using BingoX.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Core.Test.Security
{
    [Author("Dason")]
    [TestFixture]
    public class SecurityTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
#if Standard
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
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
            if(keysize == 168 || keysize == 16884)
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
        [TestCase("How ary you",
            "MIICXAIBAAKBgQDHldBH4WGnxmRfNNqZyzczwvYBszmMWEnQjbM+rbSFnZRSks4OMyg1LiHQLVC+g8WBYCS5uJ24Ktt1HJPvxEkkFUvLlG/oGYqwz4HMNOp7qtDTItoEgcv2mXj0ln++J9tNhbLA6DLxQx2euYv0BMN5eP0Zs6HcDlPmFDME3S8qhQIDAQABAoGASIdg0T5ORrIJ9681YX4/6UeILsX6u825xVg5MyXc3FGPfRJsXyyoB+tjzkspdJJeS82sivFUH4EzjzN7bz5ddWKy02CCPjzYYaAy2x9ZDzEEVFC4qYqlCidnULSKBqWfNYfrRSGNQIDVKoz6BN5INYmciBeI2D5wx38FkdaW/gECQQDyyqWPgtCYgFNVC8VrLd/K4MeszVUnC2t0BsvQMfGNVjc4pXVHeB7fw5Tp9UkiTieYL5ziZ3lOEKob9oZkYy+VAkEA0nFuu6+e3cFftTtYBwEpI2duYmPFa0coCPdyV8YnlmpPPMpsxOg0LqnNl9DzX4XgFyC1yWSGRg5e+IacRD8TMQJABuMOrTJ1vk2tj3UFBZRIi9WydElix/e/9YuXznEMPAkdCeNPn1Zd8dT3bWeCypFS+DMpjRaIT1mTqbVlfIV/vQJAF4FD9QKwo+QtZHi0ZFPk3Gd5FNRAALN57UZ2Jwei9FH7d7bEX1nwOiYNKJ/WDx1M4khtBYmM1dZVFT0zxwukgQJBAJGGEj+QYTrBiptUoxInhCN5SfltdZK2vUpqQMDGE9Zz0fdrrbU6h6/8KXk/6JWugdcT3mgEJcWHI4DjoIM9LGU=",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDHldBH4WGnxmRfNNqZyzczwvYBszmMWEnQjbM+rbSFnZRSks4OMyg1LiHQLVC+g8WBYCS5uJ24Ktt1HJPvxEkkFUvLlG/oGYqwz4HMNOp7qtDTItoEgcv2mXj0ln++J9tNhbLA6DLxQx2euYv0BMN5eP0Zs6HcDlPmFDME3S8qhQIDAQAB",
            TestName = "TestRSA_PKCS1_1024")]
        [TestCase("How ary you 你好啊！你是谁。",
            "MIICXgIBAAKBgQDAZDVw7c0xmyv2zb2SYRiERcUwk1PfiGe7VgD9BZdp27zbiQlAa4J5Eq5+VnSY0YT6CiD2xscZaTdKdg4RrO/iJn6l3OKUE7vkIbnE8aIheTbdwx8OkkuJoq2NkCflWC6G5lI4BBlmKRcrm3fIJMCprfAvRy7jeKEiZasJKOAXmwIDAQABAoGBALfXCbn/gjQEst4De0KExYifou3n2h2fmn/Ijuk4jpb5Al5FdDG6idJnp5XRx1i/3PGQ/C7Tgy0k/VCJvqflONKXUFuUft45COKpKZZSm/jKKxEYkoTqdUtVoVsoOXaZVJrEMCkDQo5nD7FH/D7iJBtwaCB+xfRJMi/yC3AOjeABAkEA8xuLNo5gkY8yIruIe3K8mOdzRDkSa5xgfl5ruF4oll1oQiEzoG1fQinnfmysJiCWyyXxdj5Q1mn+mHzsloKrgQJBAMqYIwB7PNRLtyegLyJj7x6xJvtJSCpe4jJijfxUy7lErP6rRdjVKSK3RZYe1X5JOWRHnxzI29Oi05icxTasgRsCQQC5y9g2htPpCx6PCvJBHqxi06j1gATC942Lps/+5P9yA8tVlxhkyJIf9/SzW4ypywhUFBiZfQC9Dd8FXZxXX2CBAkEAnK95MqAddZtxdcYfbhuYDka4fVQ56fskOg+a/HKgdRM2MGhKRt7lR6IywV3rhBYhuvrhhiOaUF34BGoK5LQZzQJATAZaLRfKMARCY3UtvrdOGm+52mHbGFFwtGVx94sB1TWZy20zeaOpjoRkEiw4i+8uRpiSJ+IfKICaz7h/B3J2iw==",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDAZDVw7c0xmyv2zb2SYRiERcUwk1PfiGe7VgD9BZdp27zbiQlAa4J5Eq5+VnSY0YT6CiD2xscZaTdKdg4RrO/iJn6l3OKUE7vkIbnE8aIheTbdwx8OkkuJoq2NkCflWC6G5lI4BBlmKRcrm3fIJMCprfAvRy7jeKEiZasJKOAXmwIDAQAB",
            TestName = "TestRSA_PKCS1_1024_Chinese")]
        [TestCase("How ary you",
            "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAIYJPGM1n/H3480bwn5PvRrhwchgCkMIl77m+QiIoRWr28ZRO/g+Fc4QgRK/KYzIi5HaRbkzXBTxOMt6ZIrLNa4qJUlBDbPdMQBs0PbNT22+22n0y5YnCeRxfMes5FeBIivKU4aygNWVSDTZ+yL8jIsUoRntmlr5WqgU8YIYcu/bAgMBAAECgYBNtIkH0wzZEKdGP7Ov2rZ9IShg7BzG4/JlQC5b3inVEH8nJ+0ma4fkoPjTT3PdJF6Vvf7x8W6OzZHplRk4xbpmWSOP3O4/sf54evkbSVwFWmdXncgkJFBZNhpAG2NWzbDrqZcpaBOCehvubV4EDxwX49ztEoqtOdX6SEDh8awV8QJBAMCLkAndbqBoFA8ChSEqlHlFB7gBghoyFfIOfqyRc6vnZWX5GJXP4gTbIOFReWBBxJEb/PRYeCsfLJTMA4mFL1UCQQCyNXF3j89LpcZzAbRLMKz5ViveU1fsvYS92rk+ZakVvXsxizuYrQ2dj++y/WRBo6e4yMu+u82eQBchlC7wgcJvAkBjR5L9gy+HhiLRmnsKnunvnahdOGndF/Y42UB3uofqG2YtLkN++7GOz24kv93VxBOWvtevJe7QTUKvdoJcZlABAkBRj1gQ6kZrTsGWXQ9Q4kQoESIerHIvLUWVRodFYW2TP5ckQ57Q71Kt6/Q+LO43hFj8nxlwf69JX/e2LIWXGSwNAkEApLg7tfE6N52ddACRkfhzxCv40eoN3/JD/o5Lo6zBXa8rG/iVV9Wp2Cs4iLvodYwdWQhvrrcXvewzFwHEIZstxw==",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCGCTxjNZ/x9+PNG8J+T70a4cHIYApDCJe+5vkIiKEVq9vGUTv4PhXOEIESvymMyIuR2kW5M1wU8TjLemSKyzWuKiVJQQ2z3TEAbND2zU9tvttp9MuWJwnkcXzHrORXgSIrylOGsoDVlUg02fsi/IyLFKEZ7Zpa+VqoFPGCGHLv2wIDAQAB",
            TestName = "TestRSA_PKCS8_1024")]
        [TestCase("How ary you",
            "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",
            TestName = "TestRSA_PKCS1_2048")]
        [TestCase("How ary you 你好啊！你是谁。",
            "MIIEogIBAAKCAQEA8wlWi3oy+9GfNCP0jgiv2JRX3lDzylDYQz9CglzXheZnGRo+CnzCBIBpOP+gwu6pdGxnndG7hWKY4CcuoLx9h5RS547qImDPDxwoV4y8chiboMV3pZuMNEOOepp2yXoj41vAlTUvVOLRlydsX1paR0odSPonsPWGsBu0Jt0LpcfgJrem6/fkWsxbgO3H+CsoWtvyTEBkzG5KzZJBrJs0it8BIckxx3sdRSv4pxFCwja8DUaqLCldNf5/5vbzjxNGh8LyTXvTr63+892qEsHtt6sioovik8jqYheZ0hvkkhipOYYMKLluFx0zwaimSZlAvNolUEhLYs6qLn6L+KmLCwIDAQABAoIBADHsGyDvawWbG47ES/nZAvYEEsCR9GH8lU/K8MiCdasFluvrHg9dbSUrk3Gn4Mfp6lkF44rhiBlwFWMF6WgPD+IHr7fs/V8ub5n0+LxzJkpxj5ZIubkE6Jnbvqqki/363uBW+oorsJtJNDBOYs57asv2B2n0KK8K4C3QowLQqRHWRFR0Zfa6h/uLfR8OJJfRqAoTPuHZfrNSgSvIEa2+bPDSi03kenWpuMww0PaWGzQoglJeQK6iiCs1rbVHRtDlxHmutU3lrlHy/g2dOZRablkZ8m2r96/AN/13hJuIlc1U/misbfrSw5wHAGNpnIYw9lpa8BNbTYM3opVdCvC4kQECgYEA+6k3e5z2d2r20uiDSY62dzDQQo2LyDoz38TovAe5i81+FS3NdS1W7Sd6b9VDUVwQPgQLXpxZJMzP/u1zvSWZiRUnlF4c4xerPISL5JsBf+YkX4WFbiTqmKC6OwDkHSJFbo6l7ZP/SAWbFUo44TRGwhJYAGkMfbR59BefSk+gJcECgYEA9zoN6P7mEPi+b1TE8WLozzckgPtI7gW2IUhOSotb/4uabNap/0VwPsKmXmyUD43cvLxbJZaFqRHI5ndryE2f0q44WSteQEepAGrE6ZttKVR35nQJgnj8rLK7O/9AzjVQtHSWIgSk4UoqcfDp+JVtVfIZlVK6BLRusQzM6zdAW8sCgYBZTH9SjqzqTPtOdZD4jfpzKtALfZyHLRfxJDcNWZReZkmCDGGmXbNFrceFXWBow0lEKD08kjATndWEeQ+jo27qtKX5wLdxYZmyfLoDIXmTv79SYdep8NGJ3SaX/0XyjJGknFuGhQiFru+Ly22Jg/pDrSyq4Ju9E0he8+7i+IQ0QQKBgC103S3kAEVKlyk9OuDvQMMo7mtQyL4LCsvf0ZVfPlEkSAaPCb7H87V2cMSXzjl16gpBz7h/jp/hh0Do4EZ6D1Lbebzbfn7vLiHHf+E8FLwurcUPvNfiiwLt8gM8EZxYrTgg36HtnKtXyaodk8eDIhFVbGLKTS2kR14CboMpK4o1AoGACUUiv4j3/dI6bsKmUpVSFKhBlvKne6VoUswuse2c40T8BAofCeTHZAaVVXqHhKpybvtUkttFsd1GnHiVHpYYTjS6cQ6YdNYZJt/uHjdcqD9e2gyK4RElW2YfQ4ZSgftT8vfzZaun8bL+MdSneYr2eUsZd9LSCGRYvY/zE/ScUHY=",
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8wlWi3oy+9GfNCP0jgiv2JRX3lDzylDYQz9CglzXheZnGRo+CnzCBIBpOP+gwu6pdGxnndG7hWKY4CcuoLx9h5RS547qImDPDxwoV4y8chiboMV3pZuMNEOOepp2yXoj41vAlTUvVOLRlydsX1paR0odSPonsPWGsBu0Jt0LpcfgJrem6/fkWsxbgO3H+CsoWtvyTEBkzG5KzZJBrJs0it8BIckxx3sdRSv4pxFCwja8DUaqLCldNf5/5vbzjxNGh8LyTXvTr63+892qEsHtt6sioovik8jqYheZ0hvkkhipOYYMKLluFx0zwaimSZlAvNolUEhLYs6qLn6L+KmLCwIDAQAB",
            TestName = "TestRSA_PKCS1_2048_Chinese")]
        [TestCase("How ary you",
            "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDnOLYCXPBSpieIE9FGfzBdmPdte+m6WVBQkflcEsNAKESvhTX0UoQ/zaPp1RBm/8fwZ5HAZ3mjCLcvyV+Tfa9WDcvIeeg20Z6YD1crV4zNQvbn9OEzgGsPvUgSFLQ0t8LJ1N7ZIATeHbwr899acqjsgv8thr4WYfeqdMYp2fUyULinEazaS/AYyZF6vELk7Ilan4WSFXivBURaiJjqM1iD+KcwGEAT9goGoMLLIiXE5DF6+UNXRDNjeQnis3SMnTzxnPwoyPBjj+pnD2OQ8TJporXvKicGNrOeDfLm/2iGi57oqYI3tgUTI4F3RKDt760GvEPxNm4yNlKN5uLEl9ExAgMBAAECggEAccDSumLi0KZxuF6XjKkyMDn7MF/P4x/hhWHrXcLVUBD/iaJtot+dtI4E4Bs0UJfx03IAoLnHe8/j35ygar1gr2NmKCDhf2atMX4KkoYl/S2+rusoh9/bzAiBFnQdG/pz2DvjzsoT2vqiArWd4q+aL5Fa3Rj3F6fm4nNEsn5KRQ/F+e5ujHSdn2vapNhdCwIj0kXaM4+U6nOwV/J+icYNYu8OzM/vuv3TPL8j8y9/Nbe0N2cGqHP5TDzMAcTRuB3HBxbH/NIatadkxWyXIsXUsUpwv4+M8tqL7y3jwqJMQvnHFPX/7MqtNLPgWpXF26t07bx7q2f1qk/AXQ8xA8oc4QKBgQD32vjcowOT3RnL9qWaIkVnL23ISbfkN/t5Rx2QTdi9j8omCxJ0SHxOQJbtA35GpEiRRB2AzKslO3qeJ7xDJaFEWaDK4thjXSwuP1ukpIJS46Iy4aE9bWIDijkiKtKeGwpFuQx0xf380V3uzk3Q3oPrSXeqX60PnL9aECX3OEmWZQKBgQDu0c954r6iGr1tgQQEYGT8sFmUDBZOn/jLonEYjVOsJNmc0wdhN6cqAeME4IdAu0Vd4Ah6LkhvWv/VCpjGbYPQiaWngJVbNivhyLKdJu1QLrePL2NujmbXGLlFq6jOQLVm+tHxA00UwmPzYYDmoUiHyLjYdC12xEyZA8iW6o9M3QKBgGDKYW8ANfBzYpSVMFx9z66ZBf02VJrKBel5jSECVYbJdT7gCgfhGrIQZSAPwitiEniwKflXc+ppTNwgVlO+iTjA65rvdWNwSkWRNn+YQtCN1pSaKjZr1d/eBavDH1bg5mUN+8BKZYJqXI1agWb0zn1+xj2BeXrkUpGgRsNBdvNFAoGBAIkTA/MBokmI75EarCOW8F/ZGJFRHryiNTssUZ22AICd5gmNVn8GYnxJ+POjq/4LfxUSscrvJcREhvLQ0j+SPEZFuz8ZHqDrxuQhPePVpACRz+nvWTLrqTtshWrnzEwV+AjTjgy3yFZR/OyE2meFywukufQDOtUgdpadBVUZ7IZBAoGAK7RU7yGj6+GZdZ4deKHXdAiYxT6N6EGsLkhKu7ccourM3HpuSGWXhUZ6+m1cisonUxfM1ZS29vPJfkvaGY5alea2tcikaYjs7Bhni4ziHJT1ms8JJaWOPCtqB3UuwKujVRk70FhY/4QvcPYPaLPGOlvnCibl+yjW4esD2grD3e0=",
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5zi2AlzwUqYniBPRRn8wXZj3bXvpullQUJH5XBLDQChEr4U19FKEP82j6dUQZv/H8GeRwGd5owi3L8lfk32vVg3LyHnoNtGemA9XK1eMzUL25/ThM4BrD71IEhS0NLfCydTe2SAE3h28K/PfWnKo7IL/LYa+FmH3qnTGKdn1MlC4pxGs2kvwGMmRerxC5OyJWp+FkhV4rwVEWoiY6jNYg/inMBhAE/YKBqDCyyIlxOQxevlDV0QzY3kJ4rN0jJ088Zz8KMjwY4/qZw9jkPEyaaK17yonBjazng3y5v9ohoue6KmCN7YFEyOBd0Sg7e+tBrxD8TZuMjZSjebixJfRMQIDAQAB",
            TestName = "TestRSA_PKCS8_2048")]
        public void TestRSA(string content, string privateKey, string publicKey)
        {
            var rsa = SecurityExtension.RSA;
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = privateKey;
            config.PublicKey = publicKey;
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign,"签名失败");
        }

        [Test]
        [TestCase("How ary you",
            "MIICXAIBAAKBgQDHldBH4WGnxmRfNNqZyzczwvYBszmMWEnQjbM+rbSFnZRSks4OMyg1LiHQLVC+g8WBYCS5uJ24Ktt1HJPvxEkkFUvLlG/oGYqwz4HMNOp7qtDTItoEgcv2mXj0ln++J9tNhbLA6DLxQx2euYv0BMN5eP0Zs6HcDlPmFDME3S8qhQIDAQABAoGASIdg0T5ORrIJ9681YX4/6UeILsX6u825xVg5MyXc3FGPfRJsXyyoB+tjzkspdJJeS82sivFUH4EzjzN7bz5ddWKy02CCPjzYYaAy2x9ZDzEEVFC4qYqlCidnULSKBqWfNYfrRSGNQIDVKoz6BN5INYmciBeI2D5wx38FkdaW/gECQQDyyqWPgtCYgFNVC8VrLd/K4MeszVUnC2t0BsvQMfGNVjc4pXVHeB7fw5Tp9UkiTieYL5ziZ3lOEKob9oZkYy+VAkEA0nFuu6+e3cFftTtYBwEpI2duYmPFa0coCPdyV8YnlmpPPMpsxOg0LqnNl9DzX4XgFyC1yWSGRg5e+IacRD8TMQJABuMOrTJ1vk2tj3UFBZRIi9WydElix/e/9YuXznEMPAkdCeNPn1Zd8dT3bWeCypFS+DMpjRaIT1mTqbVlfIV/vQJAF4FD9QKwo+QtZHi0ZFPk3Gd5FNRAALN57UZ2Jwei9FH7d7bEX1nwOiYNKJ/WDx1M4khtBYmM1dZVFT0zxwukgQJBAJGGEj+QYTrBiptUoxInhCN5SfltdZK2vUpqQMDGE9Zz0fdrrbU6h6/8KXk/6JWugdcT3mgEJcWHI4DjoIM9LGU=",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDHldBH4WGnxmRfNNqZyzczwvYBszmMWEnQjbM+rbSFnZRSks4OMyg1LiHQLVC+g8WBYCS5uJ24Ktt1HJPvxEkkFUvLlG/oGYqwz4HMNOp7qtDTItoEgcv2mXj0ln++J9tNhbLA6DLxQx2euYv0BMN5eP0Zs6HcDlPmFDME3S8qhQIDAQAB",
            TestName = "TestRSA2_PKCS1_1024")]
        [TestCase("How ary you 你好啊！你是谁。",
            "MIICXgIBAAKBgQDAZDVw7c0xmyv2zb2SYRiERcUwk1PfiGe7VgD9BZdp27zbiQlAa4J5Eq5+VnSY0YT6CiD2xscZaTdKdg4RrO/iJn6l3OKUE7vkIbnE8aIheTbdwx8OkkuJoq2NkCflWC6G5lI4BBlmKRcrm3fIJMCprfAvRy7jeKEiZasJKOAXmwIDAQABAoGBALfXCbn/gjQEst4De0KExYifou3n2h2fmn/Ijuk4jpb5Al5FdDG6idJnp5XRx1i/3PGQ/C7Tgy0k/VCJvqflONKXUFuUft45COKpKZZSm/jKKxEYkoTqdUtVoVsoOXaZVJrEMCkDQo5nD7FH/D7iJBtwaCB+xfRJMi/yC3AOjeABAkEA8xuLNo5gkY8yIruIe3K8mOdzRDkSa5xgfl5ruF4oll1oQiEzoG1fQinnfmysJiCWyyXxdj5Q1mn+mHzsloKrgQJBAMqYIwB7PNRLtyegLyJj7x6xJvtJSCpe4jJijfxUy7lErP6rRdjVKSK3RZYe1X5JOWRHnxzI29Oi05icxTasgRsCQQC5y9g2htPpCx6PCvJBHqxi06j1gATC942Lps/+5P9yA8tVlxhkyJIf9/SzW4ypywhUFBiZfQC9Dd8FXZxXX2CBAkEAnK95MqAddZtxdcYfbhuYDka4fVQ56fskOg+a/HKgdRM2MGhKRt7lR6IywV3rhBYhuvrhhiOaUF34BGoK5LQZzQJATAZaLRfKMARCY3UtvrdOGm+52mHbGFFwtGVx94sB1TWZy20zeaOpjoRkEiw4i+8uRpiSJ+IfKICaz7h/B3J2iw==",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDAZDVw7c0xmyv2zb2SYRiERcUwk1PfiGe7VgD9BZdp27zbiQlAa4J5Eq5+VnSY0YT6CiD2xscZaTdKdg4RrO/iJn6l3OKUE7vkIbnE8aIheTbdwx8OkkuJoq2NkCflWC6G5lI4BBlmKRcrm3fIJMCprfAvRy7jeKEiZasJKOAXmwIDAQAB",
            TestName = "TestRSA2_PKCS1_1024_Chinese")]
        [TestCase("How ary you",
            "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAIYJPGM1n/H3480bwn5PvRrhwchgCkMIl77m+QiIoRWr28ZRO/g+Fc4QgRK/KYzIi5HaRbkzXBTxOMt6ZIrLNa4qJUlBDbPdMQBs0PbNT22+22n0y5YnCeRxfMes5FeBIivKU4aygNWVSDTZ+yL8jIsUoRntmlr5WqgU8YIYcu/bAgMBAAECgYBNtIkH0wzZEKdGP7Ov2rZ9IShg7BzG4/JlQC5b3inVEH8nJ+0ma4fkoPjTT3PdJF6Vvf7x8W6OzZHplRk4xbpmWSOP3O4/sf54evkbSVwFWmdXncgkJFBZNhpAG2NWzbDrqZcpaBOCehvubV4EDxwX49ztEoqtOdX6SEDh8awV8QJBAMCLkAndbqBoFA8ChSEqlHlFB7gBghoyFfIOfqyRc6vnZWX5GJXP4gTbIOFReWBBxJEb/PRYeCsfLJTMA4mFL1UCQQCyNXF3j89LpcZzAbRLMKz5ViveU1fsvYS92rk+ZakVvXsxizuYrQ2dj++y/WRBo6e4yMu+u82eQBchlC7wgcJvAkBjR5L9gy+HhiLRmnsKnunvnahdOGndF/Y42UB3uofqG2YtLkN++7GOz24kv93VxBOWvtevJe7QTUKvdoJcZlABAkBRj1gQ6kZrTsGWXQ9Q4kQoESIerHIvLUWVRodFYW2TP5ckQ57Q71Kt6/Q+LO43hFj8nxlwf69JX/e2LIWXGSwNAkEApLg7tfE6N52ddACRkfhzxCv40eoN3/JD/o5Lo6zBXa8rG/iVV9Wp2Cs4iLvodYwdWQhvrrcXvewzFwHEIZstxw==",
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCGCTxjNZ/x9+PNG8J+T70a4cHIYApDCJe+5vkIiKEVq9vGUTv4PhXOEIESvymMyIuR2kW5M1wU8TjLemSKyzWuKiVJQQ2z3TEAbND2zU9tvttp9MuWJwnkcXzHrORXgSIrylOGsoDVlUg02fsi/IyLFKEZ7Zpa+VqoFPGCGHLv2wIDAQAB",
            TestName = "TestRSA2_PKCS8_1024")]
        [TestCase("How ary you",
            "MIIEpAIBAAKCAQEAqKtl81km5mp8IzD+yzeU9ih83gWU/PnKIAnkDWmac+/r0yrsEVWt2ZgZkUgm7zyE+xrqDdfD3VQ0LrZYPvkD44liDooS8p8TNz26sS63Im3iQwg7ebB0iYijl+vazfVgypycoxImjQWI7FYBlU/6t+mt0Yx9wAJHKlIQ1Qxnhdzq0pdsurQXTqLRLSBNHYDIK50MiYVMREFqyQxxeAXkM4C/cTzpLP/w3fLm16LyEwH1vrZu0IlKmozroIL5lBddo5Q+z0t0HWurHXqEi/vLtIBWFhHjdW0KlqnQB3b4rr8/zuJw48E1d3dPvdJu6Sx8ehf3anMmC1pLfsOGKV9gEQIDAQABAoIBACUEUMCibT4eWpYil5ij3yZS5myXjwYiD/EOGqWy6nnNStUyU1LOmfPqRJo02o6gmpK9amaVVuAJLdaeoClZ3zednA6S0Ou8a6ww+PsPxJNiyEhr0xm21yyj9ztNQNz4oEB3NI05W4qRj2ZGzdwmDGE2gLY/fg2YsmOgVR9ctI+U/A933CHCo1tJurSa9wf3X2pYFxpCvMKEwPV0qpeBYChSchUCrMpqkBHs4uiiqGKrif4Onby7Ea41yzyA0JOE3Y+ZQKvNx7ZlFTvHF9qZU3cRoGbadXUx+ewjKEguu+kVxZFbA9LYZWd7w8fk7uT8YEWZ+asH/n0XTMjx9gTQ440CgYEA2GY0vn7JJfAVXZPvSK81KPC2FIa90jTY7kPPCbM6w6gy0ah4Gwq8teNjO0SHss6JglXQJhqRz4FlIj6sKPExxyV/y4Ij2a7TcqJ9NZ31OV/TDQETL3UY68OQ8mCmP817wsltwPxac+Qp1NcOZlvY+FwQCIWPbUrdm+KG3Nt0WIsCgYEAx4kq43WN6A/YQo6JLmJBmC/Shx/rLklDS9ZI0EMqW02oDgJ+nD4BMC8aoNm40cgBQiHRcf9xjjD/UnIEbo8dUH/YZHlXUAeq+Ch37kUhNxrdYyGCjRBN3vXkcOCN52QyepkxdTlKKrcKJH6PFfS7ffWWdgpKcnH2agM+OnCOYVMCgYB/nQZNsIb+kG6JGMxrmKdI1WsK/8rBQ9kJ6YMWvVNSPsZ5TVhG2jYfLVE/ilJb5+C/s8tkLRb/v9bcMwlju5kXT83lPYHWxXp8h8Y+8D/E7fWQHaoqumtP3ayEwupHhe4OGKtYu3i8Dzl8ArbDNjblf6UU4W6LjTLS0uulIuWhsQKBgQCRYYmULOR1/oVtCPk+iGyOe9mChmkOUmbr7Hck9qr31z7o5jxljm/DMa7PH5MflgAEtSsrShjspxqAcX51J2UOb/e0TQymzM9u+91WB5xJ0BMZ1SKPsR9rofENpS7/Nuvok5GLfXBiavbC3EtYjfXFspgr7ReNByeo2jAO2E+E7QKBgQDJ0x3BLUBDNuIzgu11iyNJDyIK6hiDvCGCdySOXZAibTK+uhZXU5mvXLqAUUl8Y6+vSF88a6zGDyfHoZj24MwFC+TOW7c5aXNL6zQEJTjmweSmT3fumIHGKU7tB6Dghnr8v+KzmykQ5ztHkqRm2hYslkI2f/p4vsXwSHjkKMrqww==",
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqKtl81km5mp8IzD+yzeU9ih83gWU/PnKIAnkDWmac+/r0yrsEVWt2ZgZkUgm7zyE+xrqDdfD3VQ0LrZYPvkD44liDooS8p8TNz26sS63Im3iQwg7ebB0iYijl+vazfVgypycoxImjQWI7FYBlU/6t+mt0Yx9wAJHKlIQ1Qxnhdzq0pdsurQXTqLRLSBNHYDIK50MiYVMREFqyQxxeAXkM4C/cTzpLP/w3fLm16LyEwH1vrZu0IlKmozroIL5lBddo5Q+z0t0HWurHXqEi/vLtIBWFhHjdW0KlqnQB3b4rr8/zuJw48E1d3dPvdJu6Sx8ehf3anMmC1pLfsOGKV9gEQIDAQAB",
            TestName = "TestRSA2_PKCS1_2048")]
        [TestCase("How ary you 你好啊！你是谁。",
            "MIIEogIBAAKCAQEA8wlWi3oy+9GfNCP0jgiv2JRX3lDzylDYQz9CglzXheZnGRo+CnzCBIBpOP+gwu6pdGxnndG7hWKY4CcuoLx9h5RS547qImDPDxwoV4y8chiboMV3pZuMNEOOepp2yXoj41vAlTUvVOLRlydsX1paR0odSPonsPWGsBu0Jt0LpcfgJrem6/fkWsxbgO3H+CsoWtvyTEBkzG5KzZJBrJs0it8BIckxx3sdRSv4pxFCwja8DUaqLCldNf5/5vbzjxNGh8LyTXvTr63+892qEsHtt6sioovik8jqYheZ0hvkkhipOYYMKLluFx0zwaimSZlAvNolUEhLYs6qLn6L+KmLCwIDAQABAoIBADHsGyDvawWbG47ES/nZAvYEEsCR9GH8lU/K8MiCdasFluvrHg9dbSUrk3Gn4Mfp6lkF44rhiBlwFWMF6WgPD+IHr7fs/V8ub5n0+LxzJkpxj5ZIubkE6Jnbvqqki/363uBW+oorsJtJNDBOYs57asv2B2n0KK8K4C3QowLQqRHWRFR0Zfa6h/uLfR8OJJfRqAoTPuHZfrNSgSvIEa2+bPDSi03kenWpuMww0PaWGzQoglJeQK6iiCs1rbVHRtDlxHmutU3lrlHy/g2dOZRablkZ8m2r96/AN/13hJuIlc1U/misbfrSw5wHAGNpnIYw9lpa8BNbTYM3opVdCvC4kQECgYEA+6k3e5z2d2r20uiDSY62dzDQQo2LyDoz38TovAe5i81+FS3NdS1W7Sd6b9VDUVwQPgQLXpxZJMzP/u1zvSWZiRUnlF4c4xerPISL5JsBf+YkX4WFbiTqmKC6OwDkHSJFbo6l7ZP/SAWbFUo44TRGwhJYAGkMfbR59BefSk+gJcECgYEA9zoN6P7mEPi+b1TE8WLozzckgPtI7gW2IUhOSotb/4uabNap/0VwPsKmXmyUD43cvLxbJZaFqRHI5ndryE2f0q44WSteQEepAGrE6ZttKVR35nQJgnj8rLK7O/9AzjVQtHSWIgSk4UoqcfDp+JVtVfIZlVK6BLRusQzM6zdAW8sCgYBZTH9SjqzqTPtOdZD4jfpzKtALfZyHLRfxJDcNWZReZkmCDGGmXbNFrceFXWBow0lEKD08kjATndWEeQ+jo27qtKX5wLdxYZmyfLoDIXmTv79SYdep8NGJ3SaX/0XyjJGknFuGhQiFru+Ly22Jg/pDrSyq4Ju9E0he8+7i+IQ0QQKBgC103S3kAEVKlyk9OuDvQMMo7mtQyL4LCsvf0ZVfPlEkSAaPCb7H87V2cMSXzjl16gpBz7h/jp/hh0Do4EZ6D1Lbebzbfn7vLiHHf+E8FLwurcUPvNfiiwLt8gM8EZxYrTgg36HtnKtXyaodk8eDIhFVbGLKTS2kR14CboMpK4o1AoGACUUiv4j3/dI6bsKmUpVSFKhBlvKne6VoUswuse2c40T8BAofCeTHZAaVVXqHhKpybvtUkttFsd1GnHiVHpYYTjS6cQ6YdNYZJt/uHjdcqD9e2gyK4RElW2YfQ4ZSgftT8vfzZaun8bL+MdSneYr2eUsZd9LSCGRYvY/zE/ScUHY=",
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8wlWi3oy+9GfNCP0jgiv2JRX3lDzylDYQz9CglzXheZnGRo+CnzCBIBpOP+gwu6pdGxnndG7hWKY4CcuoLx9h5RS547qImDPDxwoV4y8chiboMV3pZuMNEOOepp2yXoj41vAlTUvVOLRlydsX1paR0odSPonsPWGsBu0Jt0LpcfgJrem6/fkWsxbgO3H+CsoWtvyTEBkzG5KzZJBrJs0it8BIckxx3sdRSv4pxFCwja8DUaqLCldNf5/5vbzjxNGh8LyTXvTr63+892qEsHtt6sioovik8jqYheZ0hvkkhipOYYMKLluFx0zwaimSZlAvNolUEhLYs6qLn6L+KmLCwIDAQAB",
            TestName = "TestRSA2_PKCS1_2048_Chinese")]
        [TestCase("How ary you",
            "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",
            TestName = "TestRSA2_PKCS8_2048")]
        public void TestRSA2(string content, string privateKey, string publicKey)
        {
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = HashAlgorithmName.SHA256 });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = privateKey;
            config.PublicKey = publicKey;
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "签名失败");
        }
    }
}
