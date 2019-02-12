using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.CodeGeneration;
using Orleans.Configuration;
using Orleans.Logging;
using BenoitCommons;
using BenoitGrainInterfaces;
using BenoitGrains;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

[assembly: KnownType(typeof(List<I2DMap<int>>))]
[assembly: KnownType(typeof(List<Map2D<int>>))]

namespace BenoitClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Benoit Client...");
            var client = await StartClient();

            var options = new RenderingOptions()
            {
                FrameWidth = 350,
                FrameHeight = 200,
                BatchSize = 2000,
                BailoutValue = 1 << 12,
                MaxIteration = 10000
            };

            PrintOptions(options);

            var observer = new RenderObserver();
            observer.OnReceivedRenderedMovie += (guid, movie) =>
            {
                SaveMovie($"{guid.ToString()}.gif", options, movie.Value);
            };

            observer.OnReceivedRenderedFrame += (guid, frame) =>
            {
                SaveFrame($"{guid.ToString()}.png", options, frame.Value);
            };

            // Frame options
            var center = new Complex(-0.743643887037158d, 0.131825904205311d);
            var scale = 0.01d;

            // Zoom options
            var frames = 200;
            var scaleMultiplier = 0.8d; // < 1 will magnify, > 1 will zoom out.

            var dispatcher = client.GetGrain<IRenderingDispatcher<int>>(Guid.NewGuid());
            await dispatcher.SetOptions(options);

            var observerRef = await client.CreateObjectReference<IRenderObserver<int>>(observer);
            await dispatcher.Subscribe(observerRef);

            //await dispatcher.BeginRenderMovie(Guid.NewGuid(), center, scale, scaleMultiplier, frames);
            await dispatcher.BeginRenderFrame(Guid.NewGuid(), center, scale);

            Console.WriteLine("Awaiting response from the server... (Enter to cancel)");
            Console.ReadLine();

            await client.Close();
        }

        static async Task<IClusterClient> StartClient()
        {
            // https://dotnet.github.io/orleans/Documentation/clusters_and_clients/configuration_guide/local_development_configuration.html

            var builder = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(o =>
                {
                    o.ClusterId = "dev";
                    o.ServiceId = "Benoit";
                })
                .ConfigureApplicationParts(parts => parts
                    .AddApplicationPart(typeof(IRenderingDispatcher<int>).Assembly).WithReferences()
                    .AddApplicationPart(typeof(RenderingDispatcher<int>).Assembly).WithReferences()
                    .AddApplicationPart(typeof(I2DMap<int>).Assembly).WithReferences()
                    .AddApplicationPart(typeof(I2DMap<int>[]).Assembly).WithReferences()
                    .AddApplicationPart(typeof(Map2D<int>[]).Assembly).WithReferences())
                .ConfigureLogging(l => l.AddConsole());

            var client = builder.Build();

            await client.Connect();
            return client;
        }

        static void PrintOptions(RenderingOptions options)
        {
            Console.WriteLine("RenderingOptions:");
            Console.WriteLine($" FrameWidth: {options.FrameWidth}");
            Console.WriteLine($" FrameHeight: {options.FrameHeight}");
            Console.WriteLine($" MaxIteration: {options.MaxIteration}");
            Console.WriteLine($" BailoutValue: {options.BailoutValue}");
            Console.WriteLine($" BatchSize: {options.BatchSize}");
        }

        static void SaveFrame(string path, RenderingOptions options, I2DMap<int> map)
        {
            Console.WriteLine($"Saving frame to {path}...");

            using (Image<Gray16> image = new Image<Gray16>(map.Width, map.Height))
            {
                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        image[x, y] = new Gray16((ushort)Lerp(ushort.MaxValue, 0, map[x, y] / (double)options.MaxIteration));
                    }
                }

                using (var file = File.Open(path, FileMode.Create))
                    image.SaveAsPng(file);
            }

            Console.WriteLine("Successfully saved!");
        }

        static void SaveMovie(string path, RenderingOptions options, I2DMap<int>[] maps)
        {
            Console.WriteLine($"Saving movie to {path}...");

            using (Image<Gray16> movie = new Image<Gray16>(options.FrameWidth, options.FrameHeight))
            {
                for (int i = 0; i < maps.Length; i++)
                {
                    var map = maps[i];

                    var frame = movie.Frames.CreateFrame();

                    for (int y = 0; y < map.Height; y++)
                    {
                        for (int x = 0; x < map.Width; x++)
                        {
                            frame[x, y] = new Gray16((ushort)Lerp(ushort.MaxValue, 0, map[x, y] / (double)options.MaxIteration));
                        }
                    }

                    var metaData = frame.MetaData.GetFormatMetaData(GifFormat.Instance);

                    metaData.FrameDelay = 12;
                }

                movie.Frames.RemoveFrame(0);

                using (var file = File.Open(path, FileMode.Create))
                    movie.SaveAsGif(file);
            }
            
            Console.WriteLine("Successfully saved!");
        }

        static double Lerp(double first, double second, double by)
        {
            return first * (1 - by) + second * by;
        }
    }
}
