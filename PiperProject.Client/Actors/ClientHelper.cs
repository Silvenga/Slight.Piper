using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PiperProject.Common.Actors;
using PiperProject.Common.Models;

namespace PiperProject.Client.Actors {
    public static class ClientHelper {

        public static async Task<string> ReadStdIn() {

            return await Console.In.ReadToEndAsync();
        }

        public static async Task Send(string apiHost, Document document) {

            if(string.IsNullOrWhiteSpace(apiHost))
                throw new ArgumentException("ApiHost must have value.");

            if(!document.IsEncrypted())
                throw new ArgumentException("Document must be encrypted before sending.");


        }

        public static async Task Read(string apiHost, Lookup lookup) {


        }


    }
}
