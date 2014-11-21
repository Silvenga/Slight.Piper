using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver.Builders;

using PiperProject.Models;

namespace PiperProject.Actors {
    class Helper {

        public static bool TryFind(string hash, out Document document) {

            var query = Query<Document>.EQ(e => e.Id, hash);

            var context = Config.Collection<Document>();

            document = context.FindOne(query);

            return document != null;
        }
    }
}
