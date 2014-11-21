using System.IO;

using Newtonsoft.Json.Linq;

namespace PiperProject.Models {

    public static class Config {

        private static dynamic _current;

        public static dynamic Current {
            get {
                return _current ?? (_current = ReadJson("config.json"));
            }
        }

        public static string Host {
            get {

                return Current.Host.Interface + ":" + Current.Host.Port;
            }
        }

        private static dynamic ReadJson(string file) {

            string jsonStr;

            using(var sr = new StreamReader(file)) {
                jsonStr = sr.ReadToEnd();
            }

            return JObject.Parse(jsonStr);
        }

        public static string ConnectionString {
            get {
                return string.Format(
                    "metadata=res://*/Models.Magic8.csdl|res://*/Models.Magic8.ssdl|res://*/Models.Magic8.msl;"+
                    "provider=System.Data.SqlClient;" +
                    "provider connection string='data source={0};initial catalog={1};persist security info=True;" +
                    "user id={2};password={3};MultipleActiveResultSets=True;App=EntityFramework';",
                    Current.Backend.Location,
                    Current.Backend.Database,
                    Current.Backend.User,
                    Current.Backend.Password);
            }
        }
    }
}
