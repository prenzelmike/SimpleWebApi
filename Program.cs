using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SimpleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            X509Certificate2 certificate = null;
            if (args.Length > 0)
            {
                Console.WriteLine(string.Format("Args on command line: {0}", String.Join(',', args)));
            }

            using (var store = new X509Store(StoreName.My))
            {
                Console.WriteLine("Looking in cert store");
                store.Open(OpenFlags.ReadOnly);
                Console.WriteLine(string.Format("{0} Certificates found", store.Certificates.Count));
                var certsAll = store.Certificates;
                if (certsAll.Count > 0)
                    foreach (var cert in store.Certificates)
                    {
                        Console.WriteLine(string.Format("{0}, {1}", cert.SubjectName.Name, cert.GetNameInfo(X509NameType.DnsName,false)));
                    }
                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", false);
                if (certs.Count > 0)
                {
                    certificate = certs[0];
                    Console.WriteLine("Certificate found {0}", certificate.SubjectName.Name);
                }
            }
            CreateWebHostBuilder(args)
            //see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1
            .ConfigureKestrel(serverOptions =>
                {
                    serverOptions.Listen(IPAddress.Loopback, 4000);
                    serverOptions.Listen(IPAddress.Loopback, 4001,
                    listenoptions =>
                    {
                        //listenoptions.UseHttps("B2225E30880205794C423F0F2827618C886F0C48.pfx","");
                        listenoptions.UseHttps("localhost.pfx", "dalamus");
                        //listenoptions.UseHttps(certificate);
                    });
                })
            //or use list of urls on the command line when starting app, e.g.
            // dotnet SimpleWebApi.dll urls "http://localhost:4000; https://localhost:4001" 
            .Build()
            .Run();
        }


        // set host url either here or in launch.json configurations array 
        // as setting "env". Setting here is used when application is started
        // as application. If no setting for url present here url defaults to 
        // http://localhost:5000 and https://localhost:5001
        // When debugging setting in launch.json is used
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        // does not work with .netcore 3.0 anymore when starting app
        // by command line dotnet SimpleWeb.dll
        //.UseUrls("https://localhost:4001, http://localhost:4000");

    }
}
