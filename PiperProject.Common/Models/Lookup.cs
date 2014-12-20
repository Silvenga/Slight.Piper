#region Usings

using PiperProject.Common.Actors;

#endregion

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

        public string PostResource {
            get {
                return string.Format("/api/document/");
            }
        }

        public string HeadResource {
            get {
                return string.Format("/api/document/{0}", Hash);
            }
        }

        public string GetResource {
            get {
                return string.Format("/api/document/{0}", Hash);
            }
        }

    }

}