using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PiperProject.Client.Actors;
using PiperProject.Common.Actors;
using PiperProject.Common.Models;

namespace PiperProject.Client {
    class Program {
        public static void Main(string[] args) {

            if(args.Length == 1) {

                var path = args.First().Split('/');

                var host = path.First();
                var lookupStr = path.Last();


            }

            //var str = ClientHelper.ReadStdIn().Result;

            var str = "Hello world";

            Lookup lookup;

            var cryptoDoc = DocumentHelper.CreateComplete(str, out lookup);

            Console.WriteLine(str);
        }
    }
}
