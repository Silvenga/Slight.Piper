using System;
using System.Web.Http;

using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;

using Owin;

using PiperProject.Models;

namespace PiperProject {

    public class Startup {

        public void Configuration(IAppBuilder appBuilder) {

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {
                    id = RouteParameter.Optional
                }
            );

            appBuilder.UseCors(CorsOptions.AllowAll);
            appBuilder.UseWebApi(config);
        }

        private static void Main() {

            var baseAddress = "http://" + Config.Host + "/";

            // Start OWIN host 
            using(WebApp.Start<Startup>(baseAddress)) {

                Console.WriteLine("Running on: " + baseAddress);
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }

            Console.WriteLine("Stopping");
        }

    }
}
