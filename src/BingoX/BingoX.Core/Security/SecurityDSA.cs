using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{
#if Framework

    public class SecurityDSA : AbstractASymmetricAlgorithm
    {
        private readonly DSACryptoServiceProvider _provide;

        public SecurityDSA(AsymmetricAlgorithmConfig config)
        {
            Config = config;
            _provide = new DSACryptoServiceProvider();
        }
        public override string Decrypt(string content)
        {
            throw new NotImplementedException();
        }

        public override string Encrypt(string content)
        {
            throw new NotImplementedException();
        }

        public override string SignData(string content)
        {
            return Config.EncryptByteToFunc(_provide.SignData(Config.Encoding.GetBytes(content)));
        }

        public override bool VerifyData(string content, string signature)
        {
            return _provide.VerifyData(Config.Encoding.GetBytes(content), Config.DecryptByteToFunc(signature));
        }
    }

#endif

}
