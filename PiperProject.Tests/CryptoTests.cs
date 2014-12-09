using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MongoDB.Driver.Builders;

using PiperProject.Common.Actors;
using PiperProject.Common.Models;
using PiperProject.Models;

using Document = PiperProject.Common.Models.Document;

namespace PiperProject.Tests {
    [TestClass]
    public class CryptoTests {

        [TestMethod]
        public void CryptoTest() {

            const string test = "Hello, World!";
            const string password = "SilverLight#";

            var cryptoText = Crypto.Encrypt(test, password);
            var result = Crypto.Decrypt(cryptoText, password);

            Assert.AreEqual(test, result);
        }

        [TestMethod]
        public void HashTest() {

            const string test = "Hello, World!";
            const string expected = "dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f";

            var result = Crypto.Hash(test);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void MongoTest() {

            var database = Config.Database;

            var collection = Config.Collection<Document>();

            var query = Query<Document>.EQ(e => e.Id, "test");

            var context = Config.Collection<Document>();

            var document = context.FindOne(query);


        }

        [TestMethod]
        public void Salt() {

            const string passPhrase = "TestHash";

            var salt = Crypto.GenerateSalt(passPhrase);
            var str = System.Text.Encoding.UTF8.GetString(salt);

            using(var password = new Rfc2898DeriveBytes(passPhrase, salt, 1000)) {

                var keyBytes = password.GetBytes(256 / 8);

                var key = Encoding.ASCII.GetString(keyBytes);

                var a = ByteArrayToString(keyBytes);
            }
        }

        public static string ByteArrayToString(byte[] ba) {
            var hex = new StringBuilder(ba.Length * 2);
            foreach(var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string Hash1(string value) {

            var sb = new StringBuilder();

            using(var hash = new SHA1Managed()) {

                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach(var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
