#region Usings

using System;
using System.Net;
using System.Threading.Tasks;

using PiperProject.Common.Actors;
using PiperProject.Common.Models;

using RestSharp;

#endregion

namespace PiperProject.Client.Actors {

    public static class ClientHelper {

        public static async Task<string> ReadStdInAsync() {

            return await Console.In.ReadToEndAsync();
        }

        public static async Task<Lookup> SendAsync(string apiHost, string documentBody) {

            if(string.IsNullOrWhiteSpace(apiHost)) {
                throw new ArgumentException("ApiHost must have value.");
            }

            var document = new Document {
                Body = documentBody
            };

            Console.WriteLine("Encrypting data.");

            Lookup lookup;
            document = document.Encrypt(out lookup);

            Console.WriteLine("Found as {0}, sending data to {1}.", lookup.Hash.Substring(0, 16), apiHost);

            var client = CreateClient(apiHost);

            var requestPost = new RestRequest(lookup.PostResource, Method.POST) {
                RequestFormat = DataFormat.Json,
            };

            requestPost.AddBody(document);

            var response = await client.ExecuteTaskAsync<Document>(requestPost);
            response.EnsureSuccess();

            return lookup;
        }

        public static async Task<Document> ReadAsync(string apiHost, string lookupKey) {

            if(string.IsNullOrWhiteSpace(apiHost)) {
                throw new ArgumentException("ApiHost must have value.");
            }

            var lookup = new Lookup {
                Key = lookupKey
            };

            var client = CreateClient(apiHost);

            Console.Error.WriteLine("Looking up {0} from {1}.", lookup.Key, apiHost);

            var requestHead = new RestRequest(lookup.HeadResource, Method.OPTIONS) {
                RequestFormat = DataFormat.Json
            };

            var headResponse = await client.ExecuteTaskAsync(requestHead);
            headResponse.EnsureSuccess();
            var header = headResponse.Content.Replace("\"", "");

            if(!DocumentHelper.IsValidHeader(header, lookup)) {
                throw new ArgumentException("Possible hash collision detected. Failing.");
            }

            Console.Error.WriteLine("Found {0}, downloading data.", lookup.Hash.Substring(0, 16));

            var requestGet = new RestRequest(lookup.GetResource, Method.GET) {
                RequestFormat = DataFormat.Json
            };

            var getResponse = await client.ExecuteTaskAsync<Document>(requestGet);
            getResponse.EnsureSuccess();

            var document = getResponse.Data;

            Console.Error.WriteLine("Decrypting {0}.", lookup.Key);
            return document.Decrypt(lookup);
        }

        public static RestClient CreateClient(string apiHost) {

            var client = new RestClient("http://" + apiHost);
            return client;
        }

        public static Task<IRestResponse<T>> ExecuteTaskAsync<T>(this RestClient client, RestRequest request)
            where T : new() {

            var task = new TaskCompletionSource<IRestResponse<T>>();

            client.ExecuteAsync<T>(
                request,
                response => {
                    if(response.ErrorException != null) {
                        task.TrySetException(response.ErrorException);
                    } else {
                        task.TrySetResult(response);
                    }
                });

            return task.Task;
        }

        public static void EnsureSuccess(this IRestResponse client) {

            if(client.StatusCode != HttpStatusCode.OK) {

                throw new Exception(
                    string.Format("Invalid response: {0} {1}.", (int) client.StatusCode, client.StatusDescription));
            }
        }

    }

}