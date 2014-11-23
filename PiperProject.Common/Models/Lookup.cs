using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PiperProject.Common.Actors;

namespace PiperProject.Common.Models {

    public class Lookup {

        public string Key {
            get;
            set;
        }

        public string Hash {
            get {
                return Crypto.Hash(Key);
            }
        }
    }
}
