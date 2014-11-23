using System;

using PiperProject.Common.Models;

namespace PiperProject.Common.Actors {

    public static class DocumentHelper {

        public static string NextId(int size = 6) {

            return Guid.NewGuid().ToString("N").Substring(0, size);
        }

        public static Document Encrypt(this Document document, out Lookup lookup) {

            if(document.IsEncrypted())
                throw new ArgumentException("Document must be plain text.");

            lookup = new Lookup {
                Key = NextId()
            };

            var id = lookup.Hash;
            var header = Crypto.Encrypt(lookup.Key, lookup.Key);
            var body = Crypto.Encrypt(document.Body, lookup.Key);

            document.Id = id;
            document.CryptoHeader = header;
            document.Body = body;

            return document;
        }

        public static Document Decrypt(this Document document, Lookup lookup) {

            if(!document.IsEncrypted())
                throw new ArgumentException("Document must be crypto text.");

            if(!Equals(lookup.Hash, document.Id))
                throw new ArgumentException("Document hash must match with the lookup signature.");

            var header = Crypto.Decrypt(document.CryptoHeader, lookup.Key);

            if(!Equals(header, lookup.Key))
                throw new ArgumentException("Document cannot be decrypted with the given lookup key. ");

            var body = Crypto.Decrypt(document.Body, lookup.Key);
            document.Body = body;
            document.CryptoHeader = null;

            return document;
        }

        public static bool IsEncrypted(this Document document) {

            return !string.IsNullOrWhiteSpace(document.CryptoHeader);
        }

        public static Document CreateComplete(string message, out Lookup lookup) {

            var documnet = new Document {
                Body = message
            };

            return documnet.Encrypt(out lookup);
        }
    }
}
