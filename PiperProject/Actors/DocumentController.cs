using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MongoDB.Driver.Builders;

using PiperProject.Common.Models;
using PiperProject.Models;

namespace PiperProject.Actors {

    public class DocumentController : ApiController {

        public string Options(string hash) {

            Console.WriteLine("Head: " + hash);

            Document document;
            if(TryFind(hash, out document)) {

                return document.CryptoHeader;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public Document Get(string hash) {

            Console.WriteLine("Get: " + hash);

            Document document;
            if(TryFind(hash, out document)) {

                return document;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public Document Delete(string hash) {

            Console.WriteLine("Delete: " + hash);

            Document document;
            if(TryDelete(hash, out document)) {

                return document;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public Document Post(Document document) {

            Console.WriteLine("Post: " + document.Id);

            var query = Query<Document>.EQ(e => e.Id, document.Id);
            var context = Config.Collection<Document>();
            var existingDocument = context.FindOne(query);

            if(existingDocument == null) {

                context.Insert(document);
                return document;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }

        private static bool TryFind(string hash, out Document document) {

            var query = Query<Document>.EQ(e => e.Id, hash);

            var context = Config.Collection<Document>();

            document = context.FindOne(query);

            return document != null;
        }

        private static bool TryDelete(string hash, out Document document) {

            var query = Query<Document>.EQ(e => e.Id, hash);

            var context = Config.Collection<Document>();

            document = context.FindOne(query);

            var isFound = document != null;

            if(isFound)
                context.Remove(query);

            return isFound;
        }

    }
}
