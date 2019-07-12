using BingoX.Security;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Core.SecurityExtension.Test
{
    [Author("huangbin")]
    [TestFixture]
    public class SecurityExtensionTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Test]
        [TestCase("How are you", TestName = "TestPKCS8_TO_PKCS1")]
        [TestCase("How are you,ÄãÊÇË­£¡ÄãÔÚÄÄ£¿", TestName = "TestPKCS8_TO_PKCS1_Chinese")]
        public void TestPKCS8_TO_PKCS1(string content)
        {
            string privateKey_PKCS8 = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDnOLYCXPBSpieIE9FGfzBdmPdte+m6WVBQkflcEsNAKESvhTX0UoQ/zaPp1RBm/8fwZ5HAZ3mjCLcvyV+Tfa9WDcvIeeg20Z6YD1crV4zNQvbn9OEzgGsPvUgSFLQ0t8LJ1N7ZIATeHbwr899acqjsgv8thr4WYfeqdMYp2fUyULinEazaS/AYyZF6vELk7Ilan4WSFXivBURaiJjqM1iD+KcwGEAT9goGoMLLIiXE5DF6+UNXRDNjeQnis3SMnTzxnPwoyPBjj+pnD2OQ8TJporXvKicGNrOeDfLm/2iGi57oqYI3tgUTI4F3RKDt760GvEPxNm4yNlKN5uLEl9ExAgMBAAECggEAccDSumLi0KZxuF6XjKkyMDn7MF/P4x/hhWHrXcLVUBD/iaJtot+dtI4E4Bs0UJfx03IAoLnHe8/j35ygar1gr2NmKCDhf2atMX4KkoYl/S2+rusoh9/bzAiBFnQdG/pz2DvjzsoT2vqiArWd4q+aL5Fa3Rj3F6fm4nNEsn5KRQ/F+e5ujHSdn2vapNhdCwIj0kXaM4+U6nOwV/J+icYNYu8OzM/vuv3TPL8j8y9/Nbe0N2cGqHP5TDzMAcTRuB3HBxbH/NIatadkxWyXIsXUsUpwv4+M8tqL7y3jwqJMQvnHFPX/7MqtNLPgWpXF26t07bx7q2f1qk/AXQ8xA8oc4QKBgQD32vjcowOT3RnL9qWaIkVnL23ISbfkN/t5Rx2QTdi9j8omCxJ0SHxOQJbtA35GpEiRRB2AzKslO3qeJ7xDJaFEWaDK4thjXSwuP1ukpIJS46Iy4aE9bWIDijkiKtKeGwpFuQx0xf380V3uzk3Q3oPrSXeqX60PnL9aECX3OEmWZQKBgQDu0c954r6iGr1tgQQEYGT8sFmUDBZOn/jLonEYjVOsJNmc0wdhN6cqAeME4IdAu0Vd4Ah6LkhvWv/VCpjGbYPQiaWngJVbNivhyLKdJu1QLrePL2NujmbXGLlFq6jOQLVm+tHxA00UwmPzYYDmoUiHyLjYdC12xEyZA8iW6o9M3QKBgGDKYW8ANfBzYpSVMFx9z66ZBf02VJrKBel5jSECVYbJdT7gCgfhGrIQZSAPwitiEniwKflXc+ppTNwgVlO+iTjA65rvdWNwSkWRNn+YQtCN1pSaKjZr1d/eBavDH1bg5mUN+8BKZYJqXI1agWb0zn1+xj2BeXrkUpGgRsNBdvNFAoGBAIkTA/MBokmI75EarCOW8F/ZGJFRHryiNTssUZ22AICd5gmNVn8GYnxJ+POjq/4LfxUSscrvJcREhvLQ0j+SPEZFuz8ZHqDrxuQhPePVpACRz+nvWTLrqTtshWrnzEwV+AjTjgy3yFZR/OyE2meFywukufQDOtUgdpadBVUZ7IZBAoGAK7RU7yGj6+GZdZ4deKHXdAiYxT6N6EGsLkhKu7ccourM3HpuSGWXhUZ6+m1cisonUxfM1ZS29vPJfkvaGY5alea2tcikaYjs7Bhni4ziHJT1ms8JJaWOPCtqB3UuwKujVRk70FhY/4QvcPYPaLPGOlvnCibl+yjW4esD2grD3e0=";
            string publicKey_PKCS8 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5zi2AlzwUqYniBPRRn8wXZj3bXvpullQUJH5XBLDQChEr4U19FKEP82j6dUQZv/H8GeRwGd5owi3L8lfk32vVg3LyHnoNtGemA9XK1eMzUL25/ThM4BrD71IEhS0NLfCydTe2SAE3h28K/PfWnKo7IL/LYa+FmH3qnTGKdn1MlC4pxGs2kvwGMmRerxC5OyJWp+FkhV4rwVEWoiY6jNYg/inMBhAE/YKBqDCyyIlxOQxevlDV0QzY3kJ4rN0jJ088Zz8KMjwY4/qZw9jkPEyaaK17yonBjazng3y5v9ohoue6KmCN7YFEyOBd0Sg7e+tBrxD8TZuMjZSjebixJfRMQIDAQAB";
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = HashAlgorithmName.SHA256 });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = RSAConvert.XC.RSA_PrivateKey_PKCS8_TO_PKCS1(privateKey_PKCS8);
            config.PublicKey = publicKey_PKCS8;
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "Ç©ÃûÊ§°Ü");
        }

        [Test]
        [TestCase("How are you", TestName = "TestXML_PKCS1")]
        [TestCase("How are you,ÄãÊÇË­£¡ÄãÔÚÄÄ£¿", TestName = "TestXML_PKCS1_Chinese")]
        public void TestXML_PKCS1(string content)
        {
            string privateKey_XML = "<RSAKeyValue><Modulus>vJiSFcaHuHxDa4rggx3KZA6ll3QZD+iJ6kyuKYgzUgPhurKxRxKynU+IJYDBs445y2dwptMjoSYOWr86Q+jWFkoEiFqHaN6VoD8i9M4yHwznFynrXEIKXExXDc+NfGAYFAv+xFb8UhHokoZ2R4HAs7GilwGdjWIr0qhtOPr+mcU=</Modulus><Exponent>AQAB</Exponent><P>4MCVppI1aFQn73muRr4hhr/F5EmNW1f3Baft5SqwvdZsH5nUHb938/PcfL0hMSIcIs9Asxeg0JXD4BdZ5rMpmw==</P><Q>1tEbD/9t9oPxzE5S4F0svCux1rEGHC8TTqXUiU+C9Ams/2n0I20EzEHWC3o9ShGxE4dAERcLSovOgeO9yDawHw==</Q><DP>4GttAwa7jZF6zwURoFH56DNVxr4rCCqt3lfwlAQst2KEVIml5I5rmIzIfUXc/tKwhZGtaScOzTi+feTTQClmQQ==</DP><DQ>RgD8zbjSr+wNpyO7FEyo4GVo3erwb8zTgOS/n0OfDEH+83kmy0iisKGfzDu4r2OWjiOcODWHQ3LOCDwcQ5u2xw==</DQ><InverseQ>PxxWTYFlRAvkoEPk3ntVmYBtY3kTrHX+C9Ffz9IffCSQUF3AJp+vdPbtK3Sa0gab0dLySMSr0yUZOFW2jQsNyg==</InverseQ><D>cw5pYRuDRUuaHgvVuGf+R1igMWvEtm3ZZbVuMyxCkvCHT80gVgnUKiCRIMASbCaCn9L1aPE6tV9wNnCfbTqpEFff9Cq1D85Eo8GTRT5Urv/qoHOgNf9GakV19iS9DQQsZssYu53a83sqM3j6jFNQtnPWEWNdN9aNLHVNv36Y0ik=</D></RSAKeyValue>";
            string publicKey_XML = "<RSAKeyValue><Modulus>vJiSFcaHuHxDa4rggx3KZA6ll3QZD+iJ6kyuKYgzUgPhurKxRxKynU+IJYDBs445y2dwptMjoSYOWr86Q+jWFkoEiFqHaN6VoD8i9M4yHwznFynrXEIKXExXDc+NfGAYFAv+xFb8UhHokoZ2R4HAs7GilwGdjWIr0qhtOPr+mcU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = HashAlgorithmName.SHA256 });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = RSAConvert.XC.RSA_PrivateKey_XML_TO_PKCS1(privateKey_XML);
            config.PublicKey = RSAConvert.XC.RSA_PublicKey_XML_TO_PKCS1(publicKey_XML);
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "Ç©ÃûÊ§°Ü");
        }
    }
}