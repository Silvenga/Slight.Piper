using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using MongoDB.Driver.Builders;

using PiperProject.Models;

namespace PiperProject.Actors {

    public class DocumentController : ApiController {


        public string Options(string hash) {

            Document document;
            if(Helper.TryFind(hash, out document)) {

                return document.CryptoHeader;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public Document Get(string hash) {

            Document document;
            if(Helper.TryFind(hash, out document)) {

                return document;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        public Document Post(Document document) {

            var query = Query<Document>.EQ(e => e.Id, document.Id);
            var context = Config.Collection<Document>();
            var existingDocument = context.FindOne(query);

            if(existingDocument == null) {

                context.Insert(document);
                return document;
            }

            throw new HttpResponseException(HttpStatusCode.Conflict);
        }
    }
}
