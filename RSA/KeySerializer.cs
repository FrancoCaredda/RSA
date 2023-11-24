using System;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace RSA
{
    public class KeySerializer
    {
        private RSAParameters _key;
        private XmlSerializer _serializer;
        private StringWriter _writer;

        private bool _keyInitialized;

        public KeySerializer()
        {
            _serializer = new XmlSerializer(typeof(RSAParameters));
            _writer = new StringWriter();
            _keyInitialized = false;
        }

        public KeySerializer(RSAParameters key)
        {
            _key = key;
            _serializer = new XmlSerializer(typeof(RSAParameters));
            _writer = new StringWriter();
            _keyInitialized = true;
        }

        public string Serialize()
        {
            if (!_keyInitialized)
            {
                return null;
            }

            _serializer.Serialize(_writer, _key);
            return _writer.ToString();
        }

        public RSAParameters Deserialize(TextReader reader)
        {
            return (RSAParameters)_serializer.Deserialize(reader);
        }
    }
}
