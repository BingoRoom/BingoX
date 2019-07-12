using BingoX.Security;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.SecurityExtension.Test
{
    [Author("huangbin")]
    [TestFixture]
    public class SecurityExtensionTest
    {
        [OneTimeSetUp]
        public void Setup()
        {

        }

        [Test]
        [TestCase("How are you", TestName = "TestPKCS8_TO_PKCS1")]
        [TestCase("How are you,你是谁！你在哪？", TestName = "TestPKCS8_TO_PKCS1_Chinese")]
        public void TestPKCS8_TO_XML(string content)
        {
            string privateKey_PKCS8 = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDnOLYCXPBSpieIE9FGfzBdmPdte+m6WVBQkflcEsNAKESvhTX0UoQ/zaPp1RBm/8fwZ5HAZ3mjCLcvyV+Tfa9WDcvIeeg20Z6YD1crV4zNQvbn9OEzgGsPvUgSFLQ0t8LJ1N7ZIATeHbwr899acqjsgv8thr4WYfeqdMYp2fUyULinEazaS/AYyZF6vELk7Ilan4WSFXivBURaiJjqM1iD+KcwGEAT9goGoMLLIiXE5DF6+UNXRDNjeQnis3SMnTzxnPwoyPBjj+pnD2OQ8TJporXvKicGNrOeDfLm/2iGi57oqYI3tgUTI4F3RKDt760GvEPxNm4yNlKN5uLEl9ExAgMBAAECggEAccDSumLi0KZxuF6XjKkyMDn7MF/P4x/hhWHrXcLVUBD/iaJtot+dtI4E4Bs0UJfx03IAoLnHe8/j35ygar1gr2NmKCDhf2atMX4KkoYl/S2+rusoh9/bzAiBFnQdG/pz2DvjzsoT2vqiArWd4q+aL5Fa3Rj3F6fm4nNEsn5KRQ/F+e5ujHSdn2vapNhdCwIj0kXaM4+U6nOwV/J+icYNYu8OzM/vuv3TPL8j8y9/Nbe0N2cGqHP5TDzMAcTRuB3HBxbH/NIatadkxWyXIsXUsUpwv4+M8tqL7y3jwqJMQvnHFPX/7MqtNLPgWpXF26t07bx7q2f1qk/AXQ8xA8oc4QKBgQD32vjcowOT3RnL9qWaIkVnL23ISbfkN/t5Rx2QTdi9j8omCxJ0SHxOQJbtA35GpEiRRB2AzKslO3qeJ7xDJaFEWaDK4thjXSwuP1ukpIJS46Iy4aE9bWIDijkiKtKeGwpFuQx0xf380V3uzk3Q3oPrSXeqX60PnL9aECX3OEmWZQKBgQDu0c954r6iGr1tgQQEYGT8sFmUDBZOn/jLonEYjVOsJNmc0wdhN6cqAeME4IdAu0Vd4Ah6LkhvWv/VCpjGbYPQiaWngJVbNivhyLKdJu1QLrePL2NujmbXGLlFq6jOQLVm+tHxA00UwmPzYYDmoUiHyLjYdC12xEyZA8iW6o9M3QKBgGDKYW8ANfBzYpSVMFx9z66ZBf02VJrKBel5jSECVYbJdT7gCgfhGrIQZSAPwitiEniwKflXc+ppTNwgVlO+iTjA65rvdWNwSkWRNn+YQtCN1pSaKjZr1d/eBavDH1bg5mUN+8BKZYJqXI1agWb0zn1+xj2BeXrkUpGgRsNBdvNFAoGBAIkTA/MBokmI75EarCOW8F/ZGJFRHryiNTssUZ22AICd5gmNVn8GYnxJ+POjq/4LfxUSscrvJcREhvLQ0j+SPEZFuz8ZHqDrxuQhPePVpACRz+nvWTLrqTtshWrnzEwV+AjTjgy3yFZR/OyE2meFywukufQDOtUgdpadBVUZ7IZBAoGAK7RU7yGj6+GZdZ4deKHXdAiYxT6N6EGsLkhKu7ccourM3HpuSGWXhUZ6+m1cisonUxfM1ZS29vPJfkvaGY5alea2tcikaYjs7Bhni4ziHJT1ms8JJaWOPCtqB3UuwKujVRk70FhY/4QvcPYPaLPGOlvnCibl+yjW4esD2grD3e0=";
            string publicKey_PKCS8 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA5zi2AlzwUqYniBPRRn8wXZj3bXvpullQUJH5XBLDQChEr4U19FKEP82j6dUQZv/H8GeRwGd5owi3L8lfk32vVg3LyHnoNtGemA9XK1eMzUL25/ThM4BrD71IEhS0NLfCydTe2SAE3h28K/PfWnKo7IL/LYa+FmH3qnTGKdn1MlC4pxGs2kvwGMmRerxC5OyJWp+FkhV4rwVEWoiY6jNYg/inMBhAE/YKBqDCyyIlxOQxevlDV0QzY3kJ4rN0jJ088Zz8KMjwY4/qZw9jkPEyaaK17yonBjazng3y5v9ohoue6KmCN7YFEyOBd0Sg7e+tBrxD8TZuMjZSjebixJfRMQIDAQAB";
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = "SHA256" });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = RSAConvert.BouncyCastle.RSA_PrivateKey_PKCS8_TO_XML(privateKey_PKCS8);
            config.PublicKey = RSAConvert.BouncyCastle.RSA_PublicKey_PKCS8_TO_XML(publicKey_PKCS8);
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "签名失败");
        }

        [Test]
        [TestCase("How are you", TestName = "TestXML_PKCS1")]
        [TestCase("How are you,你是谁！你在哪？", TestName = "TestXML_PKCS1_Chinese")]
        public void TestPKCS1_XML(string content)
        {
            string privateKey_PKCS1 = "MIIEpAIBAAKCAQEAqKtl81km5mp8IzD+yzeU9ih83gWU/PnKIAnkDWmac+/r0yrsEVWt2ZgZkUgm7zyE+xrqDdfD3VQ0LrZYPvkD44liDooS8p8TNz26sS63Im3iQwg7ebB0iYijl+vazfVgypycoxImjQWI7FYBlU/6t+mt0Yx9wAJHKlIQ1Qxnhdzq0pdsurQXTqLRLSBNHYDIK50MiYVMREFqyQxxeAXkM4C/cTzpLP/w3fLm16LyEwH1vrZu0IlKmozroIL5lBddo5Q+z0t0HWurHXqEi/vLtIBWFhHjdW0KlqnQB3b4rr8/zuJw48E1d3dPvdJu6Sx8ehf3anMmC1pLfsOGKV9gEQIDAQABAoIBACUEUMCibT4eWpYil5ij3yZS5myXjwYiD/EOGqWy6nnNStUyU1LOmfPqRJo02o6gmpK9amaVVuAJLdaeoClZ3zednA6S0Ou8a6ww+PsPxJNiyEhr0xm21yyj9ztNQNz4oEB3NI05W4qRj2ZGzdwmDGE2gLY/fg2YsmOgVR9ctI+U/A933CHCo1tJurSa9wf3X2pYFxpCvMKEwPV0qpeBYChSchUCrMpqkBHs4uiiqGKrif4Onby7Ea41yzyA0JOE3Y+ZQKvNx7ZlFTvHF9qZU3cRoGbadXUx+ewjKEguu+kVxZFbA9LYZWd7w8fk7uT8YEWZ+asH/n0XTMjx9gTQ440CgYEA2GY0vn7JJfAVXZPvSK81KPC2FIa90jTY7kPPCbM6w6gy0ah4Gwq8teNjO0SHss6JglXQJhqRz4FlIj6sKPExxyV/y4Ij2a7TcqJ9NZ31OV/TDQETL3UY68OQ8mCmP817wsltwPxac+Qp1NcOZlvY+FwQCIWPbUrdm+KG3Nt0WIsCgYEAx4kq43WN6A/YQo6JLmJBmC/Shx/rLklDS9ZI0EMqW02oDgJ+nD4BMC8aoNm40cgBQiHRcf9xjjD/UnIEbo8dUH/YZHlXUAeq+Ch37kUhNxrdYyGCjRBN3vXkcOCN52QyepkxdTlKKrcKJH6PFfS7ffWWdgpKcnH2agM+OnCOYVMCgYB/nQZNsIb+kG6JGMxrmKdI1WsK/8rBQ9kJ6YMWvVNSPsZ5TVhG2jYfLVE/ilJb5+C/s8tkLRb/v9bcMwlju5kXT83lPYHWxXp8h8Y+8D/E7fWQHaoqumtP3ayEwupHhe4OGKtYu3i8Dzl8ArbDNjblf6UU4W6LjTLS0uulIuWhsQKBgQCRYYmULOR1/oVtCPk+iGyOe9mChmkOUmbr7Hck9qr31z7o5jxljm/DMa7PH5MflgAEtSsrShjspxqAcX51J2UOb/e0TQymzM9u+91WB5xJ0BMZ1SKPsR9rofENpS7/Nuvok5GLfXBiavbC3EtYjfXFspgr7ReNByeo2jAO2E+E7QKBgQDJ0x3BLUBDNuIzgu11iyNJDyIK6hiDvCGCdySOXZAibTK+uhZXU5mvXLqAUUl8Y6+vSF88a6zGDyfHoZj24MwFC+TOW7c5aXNL6zQEJTjmweSmT3fumIHGKU7tB6Dghnr8v+KzmykQ5ztHkqRm2hYslkI2f/p4vsXwSHjkKMrqww==";
            string publicKey_PKCS1 = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqKtl81km5mp8IzD+yzeU9ih83gWU/PnKIAnkDWmac+/r0yrsEVWt2ZgZkUgm7zyE+xrqDdfD3VQ0LrZYPvkD44liDooS8p8TNz26sS63Im3iQwg7ebB0iYijl+vazfVgypycoxImjQWI7FYBlU/6t+mt0Yx9wAJHKlIQ1Qxnhdzq0pdsurQXTqLRLSBNHYDIK50MiYVMREFqyQxxeAXkM4C/cTzpLP/w3fLm16LyEwH1vrZu0IlKmozroIL5lBddo5Q+z0t0HWurHXqEi/vLtIBWFhHjdW0KlqnQB3b4rr8/zuJw48E1d3dPvdJu6Sx8ehf3anMmC1pLfsOGKV9gEQIDAQAB";
            var rsa = SecurityFactory.CreateSecurityRSA(new AsymmetricAlgorithmConfig() { Halg = "SHA256" });
            var config = (AsymmetricAlgorithmConfig)rsa.GetConfig();
            config.PrivateKey = RSAConvert.BouncyCastle.RSA_PrivateKey_PKCS1_TO_XML(privateKey_PKCS1);
            config.PublicKey = RSAConvert.BouncyCastle.RSA_PublicKey_PKCS1_TO_XML(publicKey_PKCS1);
            var ciphertext = rsa.Encrypt(content);
            var planttext = rsa.Decrypt(ciphertext);
            StringAssert.AreEqualIgnoringCase(planttext, content);

            var signature = rsa.SignData(content);
            var sign = rsa.VerifyData(content, signature);
            Assert.IsTrue(sign, "签名失败");
        }
    }
}
