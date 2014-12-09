using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PiperProject.Common.Actors {
    public static class Crypto {

        public const int Keysize = 256;

        public static byte[] GenerateSalt(string password) {

            var hashed = Hash(password);
            return Encoding.ASCII.GetBytes(hashed.Substring(0, 16));
        }

        public static string Encrypt(string plainText, string passPhrase) {

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var salt = GenerateSalt(passPhrase);

            using(var password = new Rfc2898DeriveBytes(passPhrase, salt)) {

                var keyBytes = password.GetBytes(Keysize / 8);

                using(var symmetricKey = new AesManaged()) {
                    using(var encryptor = symmetricKey.CreateEncryptor(keyBytes, salt)) {
                        using(var memoryStream = new MemoryStream()) {
                            using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {

                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase) {

            var cipherTextBytes = Convert.FromBase64String(cipherText);
            var salt = GenerateSalt(passPhrase);

            using(var password = new Rfc2898DeriveBytes(passPhrase, salt)) {

                var keyBytes = password.GetBytes(Keysize / 8);

                using(var symmetricKey = new AesManaged()) {
                    using(var decryptor = symmetricKey.CreateDecryptor(keyBytes, salt)) {
                        using(var memoryStream = new MemoryStream(cipherTextBytes)) {
                            using(var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {

                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string Hash(string value) {

            var sb = new StringBuilder();

            using(var hash = new SHA256Managed()) {

                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach(var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
