using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using BenoitGrains;

namespace BenoitSilo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Benoit silo...");
            var host = await StartSilo();
            Console.WriteLine("Benoit silo is running.");
            
            Console.WriteLine("Press Enter to terminate...");
            Console.ReadLine();

            await host.StopAsync();
        }

        static async Task<ISiloHost> StartSilo()
        {
            // https://dotnet.github.io/orleans/Documentation/clusters_and_clients/configuration_guide/local_development_configuration.html

            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(o =>
                {
                    o.ClusterId = "dev";
                    o.ServiceId = "Benoit";
                })
                .Configure<EndpointOptions>(o => o.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureLogging(l => l.AddConsole())
                .ConfigureApplicationParts(parts => parts
                    .AddApplicationPart(typeof(RenderingDispatcher<int>).Assembly).WithReferences())
                .AddMemoryGrainStorage("BenoitStorage", o => o.NumStorageGrains = 10);
                
            var silo = builder.Build();
            
            await silo.StartAsync();
            return silo;
        }
    }
}
