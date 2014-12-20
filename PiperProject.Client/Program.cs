#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PiperProject.Client.Actors;

#endregion

namespace PiperProject.Client {

    internal static class Program {

        public static void Main(string[] args) {

            Task.Run(
                async () => { await AsyncContext(args); }).Wait();
        }

        public static async Task AsyncContext(IEnumerable<string> args) {

            var arg = args.FirstOrDefault();

            if(arg != null) {

                var path = arg.Split('/');

                var apiHost = path.FirstOrDefault();
                var lookupKey = path.LastOrDefault();

                try {

                    Console.Error.WriteLine();

                    if(path.Length == 1) {

                        Console.WriteLine("Reading Stdin.");
                        var documentBody = await ClientHelper.ReadStdInAsync();
                        var lookup = await ClientHelper.SendAsync(apiHost, documentBody);

                        Console.WriteLine("\n{0}/{1}", apiHost, lookup.Key);

                    } else if(path.Length == 2) {

                        var document = await ClientHelper.ReadAsync(apiHost, lookupKey);

                        Console.Error.WriteLine();
                        Console.WriteLine(document.Body);

                    } else {

                        BadArguments();
                    }

                } catch(Exception e) {

                    Console.Error.WriteLine("{0}", e.Message);
                }
            } else {

                BadArguments();
            }
        }

        public static void BadArguments() {

            Console.Error.WriteLine("Arguments invalid, syntax: piper <api host>[/<lookup>]");
        }

    }

}