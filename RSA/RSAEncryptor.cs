using System;
using System.Security.Cryptography;
using System.Text;

namespace RSA
{
    public class RSAEncryptor
    {
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        private RSACryptoServiceProvider _defaultProvider;

        public RSAEncryptor(bool initKeys = true)
        {
            _defaultProvider = new RSACryptoServiceProvider();

            if (initKeys)
            {
                _privateKey = _defaultProvider.ExportParameters(true);
                _publicKey = _defaultProvider.ExportParameters(false);
            }
        }

        public void SetPublicKey(RSAParameters publicKey)
        {
            _publicKey = publicKey;
            _defaultProvider.ImportParameters(publicKey);
        }

        public void SetPrivateKey(RSAParameters privateKey)
        {
            _privateKey = privateKey;
            _defaultProvider.ImportParameters(privateKey);
        }

        public RSAParameters GetPublicKey()
        {
            return _publicKey;
        }

        public RSAParameters GetPrivateKey()
        {
            return _privateKey;
        }

        public byte[] Encrypt(string text)
        {
            return _defaultProvider.Encrypt(Encoding.UTF8.GetBytes(text), false);
        }

        public string Decrypt(byte[] bytes)
        {
            return Encoding.UTF8.GetString(_defaultProvider.Decrypt(bytes, false));
        }
    }
}
