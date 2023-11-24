using System;
using System.IO;
using RSA;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace RSAEncryptionApp
{
    internal class Program
    {
        private static void StartEncryption()
        {
            // Read a message from the input
            Console.WriteLine("Enter your message: ");
            string message = Console.ReadLine();

            Console.WriteLine("Start encrypting your message...");

            RSAEncryptor encryptor = new RSAEncryptor();

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            byte[] bytes = encryptor.Encrypt(message);

            watch.Stop();

            Console.WriteLine($"The encryption process took: {watch.Elapsed}");

            Console.WriteLine("Enter a name for the output files");
            string filename = Console.ReadLine();

            using (var stream = new StreamWriter(filename + ".txt"))
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    stream.WriteLine(bytes[i]);
                }
            }

            KeySerializer publicKeySerializer = new KeySerializer(encryptor.GetPublicKey());
            string publicKey = publicKeySerializer.Serialize();

            using (var stream = new StreamWriter(filename + "_publicKey.xml"))
            {
                stream.Write(publicKey);
            }

            KeySerializer privateKeySerializer = new KeySerializer(encryptor.GetPrivateKey());
            string privateKey = privateKeySerializer.Serialize();

            using (var stream = new StreamWriter(filename + "_privateKey.xml"))
            {
                stream.Write(privateKey);
            }

            Console.WriteLine("The process is finished");
            Console.ReadKey();
        }

        private static void StartDecryption()
        {
            Console.WriteLine("Enter a filename to decrypt: ");
            string filename = Console.ReadLine();

            List<byte> bytesList = new List<byte>();
            using (var stream = new StreamReader(filename + ".txt"))
            {
                string data;

                while ((data = stream.ReadLine()) != null)
                {
                    bytesList.Add(Convert.ToByte(data));
                }
            }

            byte[] bytes = bytesList.ToArray();

            RSAParameters privateKey;
            using (var stream = new StreamReader(filename + "_privateKey.xml"))
            {
                KeySerializer serializer = new KeySerializer();
                privateKey = serializer.Deserialize(stream);
            }

            RSAParameters publicKey;
            using (var stream = new StreamReader(filename + "_publicKey.xml"))
            {
                KeySerializer serializer = new KeySerializer();
                publicKey = serializer.Deserialize(stream);
            }

            RSAEncryptor encryptor = new RSAEncryptor(false);
            encryptor.SetPrivateKey(privateKey);
            //encryptor.SetPublicKey(publicKey);

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            string message = encryptor.Decrypt(bytes);

            watch.Stop();

            Console.WriteLine(message);
            Console.WriteLine($"The decryption process took: {watch.Elapsed}");

            Console.ReadKey();
        }

        static void Main(string[] args)
        {

            try
            {
                int choice;
                do
                {
                    Console.WriteLine("Select a task to do (0 - Decryption, 1 - Encryption, -1 - Exit): ");
                    choice = Convert.ToInt32(Console.ReadLine());

                    if (choice == 0)
                    {
                        StartDecryption();
                    }
                    else if (choice == 1)
                    {
                        StartEncryption();
                    }

                    Console.Clear();
                } while (choice != -1);
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong");
                Console.ReadKey();
            }
            
        }
    }
}
