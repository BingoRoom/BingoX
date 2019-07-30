using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Security
{

    public class SecurityRSA : AbstractASymmetricAlgorithm
    {
#if Standard

        private string _oldPrivateKey;
        private string _oldPublicKey;

        private RSA _privateKeyRsaProvider;
        private RSA _publicKeyRsaProvider;

        public SecurityRSA(AsymmetricAlgorithmConfig config)
        {
            Config = config;
        }

        public override string Decrypt(string content)
        {
            Create();
            return Config.KeyType == KeyTypeEnum.pri ?
                Config.Encoding.GetString(_privateKeyRsaProvider.Decrypt(Config.DecryptByteToFunc(content), Config.RsaEncryptionPadding)) :
                Config.Encoding.GetString(_publicKeyRsaProvider.Decrypt(Config.DecryptByteToFunc(content), Config.RsaEncryptionPadding));
        }

        public override string Encrypt(string content)
        {
            Create();
            return Config.KeyType == KeyTypeEnum.pri ?
                Config.EncryptByteToFunc(_privateKeyRsaProvider.Encrypt(Config.Encoding.GetBytes(content), Config.RsaEncryptionPadding)) :
                Config.EncryptByteToFunc(_publicKeyRsaProvider.Encrypt(Config.Encoding.GetBytes(content), Config.RsaEncryptionPadding));
        }

        public override string SignData(string content)
        {
            Create();
            if(Config.KeyType == KeyTypeEnum.pri)
            {
                byte[] dataBytes = Config.Encoding.GetBytes(content);
                var signatureBytes = _privateKeyRsaProvider.SignData(dataBytes, 0, dataBytes.Length, Config.Halg, Config.RsaSignaturePadding);
                return Config.EncryptByteToFunc(signatureBytes);
            }
            else
            {
                byte[] dataBytes = Config.Encoding.GetBytes(content);
                var signatureBytes = _publicKeyRsaProvider.SignData(dataBytes, 0, dataBytes.Length, Config.Halg, Config.RsaSignaturePadding);
                return Config.EncryptByteToFunc(signatureBytes);
            }
        }

        public override bool VerifyData(string content, string signature)
        {
            Create();
            if (Config.KeyType == KeyTypeEnum.pri)
            {
                byte[] dataBytes = Config.Encoding.GetBytes(content);
                byte[] signBytes = Config.DecryptByteToFunc(signature);
                return _privateKeyRsaProvider.VerifyData(dataBytes, signBytes, Config.Halg, Config.RsaSignaturePadding);
            }
            else
            {
                byte[] dataBytes = Config.Encoding.GetBytes(content);
                byte[] signBytes = Config.DecryptByteToFunc(signature);
                return _publicKeyRsaProvider.VerifyData(dataBytes, signBytes, Config.Halg, Config.RsaSignaturePadding);

            }
        }

        private void Create()
        {
            CheckConfig(Config);
            if (Config.KeyType == KeyTypeEnum.pri && (_privateKeyRsaProvider == null || _oldPrivateKey != Config.PrivateKey))
            {
                _privateKeyRsaProvider = RSACreator.CreateRsaProviderFromPrivateKey(Config.PrivateKey);
                _oldPrivateKey = Config.PrivateKey;
            }
            if (Config.KeyType == KeyTypeEnum.pub && (_publicKeyRsaProvider == null || _oldPublicKey != Config.PublicKey))
            {
                _publicKeyRsaProvider = RSACreator.CreateRsaProviderFromPublicKey(Config.PublicKey);
                _oldPublicKey = Config.PublicKey;
            }
        }
#else


        private RSACryptoServiceProvider _provide;
        private string _oldPrivateKey;
        private string _oldPublicKey;
        public SecurityRSA(AsymmetricAlgorithmConfig config)
        {
            Config = config;

        }

        public override string Decrypt(string content)
        {
            Create();
            return Config.Encoding.GetString(_provide.Decrypt(Config.DecryptByteToFunc(content), Config.FOAEP));
        }

        public override string Encrypt(string content)
        {
            Create();
            return Config.EncryptByteToFunc(_provide.Encrypt(Config.Encoding.GetBytes(content), Config.FOAEP));
        }

        public override string SignData(string content)
        {
            Create();
            return Config.EncryptByteToFunc(_provide.SignData(Config.Encoding.GetBytes(content), Config.Halg));
        }

        public override bool VerifyData(string content, string signature)
        {
            Create();
            return _provide.VerifyData(Config.Encoding.GetBytes(content), Config.Halg, Config.DecryptByteToFunc(signature));
        }

        private void Create()
        {
            CheckConfig(Config);
            if (_provide == null) _provide = new RSACryptoServiceProvider();
            if (Config.KeyType == KeyTypeEnum.pri && _oldPrivateKey != Config.PrivateKey)
            {
                _provide.FromXmlString(Config.PrivateKey);
                _oldPrivateKey = Config.PrivateKey;
            }
            if (Config.KeyType == KeyTypeEnum.pub && _oldPublicKey != Config.PublicKey)
            {
                _provide.FromXmlString(Config.PublicKey);
                _oldPublicKey = Config.PublicKey;
            }
        }

#endif
    }
}
