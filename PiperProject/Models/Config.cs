using System.IO;

using MongoDB.Driver;

using Newtonsoft.Json.Linq;

namespace PiperProject.Models {

    public static class Config {

        private static dynamic _current;
        private static MongoClient _database;

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
                    "mongodb://{0}/{1}",
                    Current.Backend.Location,
                    Current.Backend.Database);
            }
        }

        public static MongoDatabase Database {
            get {
                return (_database ?? (_database = new MongoClient(ConnectionString))).GetServer().GetDatabase((string) Current.Backend.Database);
            }
        }

        public static MongoCollection<T> Collection<T>(string pre = "") {

            var type = typeof(T);

            return Database.GetCollection<T>(pre + type.Name);
        }

    }
}
