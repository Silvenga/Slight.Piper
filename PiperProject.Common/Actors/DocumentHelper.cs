using System;

using PiperProject.Common.Models;

namespace PiperProject.Common.Actors {

    public static class DocumentHelper {

        public static string Next(int size = 6) {

            return Guid.NewGuid().ToString("N").Substring(0, size);
        }

        public static Document Encrypt(this Document document, out string lookup) {

            if(document.IsEncrypted())
                throw new ArgumentException("Document must be plain text.");

            lookup = Next();

            var id = Crypto.Hash(lookup);
            var header = Crypto.Encrypt(lookup, lookup);
            var body = Crypto.Encrypt(document.Body, lookup);

            document.Id = id;
            document.CryptoHeader = header;
            document.Body = body;

            return document;
        }

        public static Document Encrypt(this Document document, string lookup) {

            if(!document.IsEncrypted())
                throw new ArgumentException("Document must be crypto text.");

            var header = Crypto.Decrypt(document.CryptoHeader, lookup);

            if(!Equals(header, lookup))
                throw new ArgumentException("Document cannot be decrypted with the given lookup key. ");

            var body = Crypto.Decrypt(document.Body, lookup);
            document.Body = body;
            document.CryptoHeader = null;

            return document;
        }

        public static bool IsEncrypted(this Document document) {

            return !string.IsNullOrWhiteSpace(document.CryptoHeader);
        }
    }
}
