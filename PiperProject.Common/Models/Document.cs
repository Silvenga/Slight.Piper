using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace PiperProject.Common.Models {

    public class Document {

        public string Id {
            get;
            set;
        }

        public string CryptoHeader {
            get;
            set;
        }

        public string Body {
            get;
            set;
        }
    }
}
