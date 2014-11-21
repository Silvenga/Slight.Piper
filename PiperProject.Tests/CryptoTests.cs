using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PiperProject.Common.Actors;

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
    }
}
