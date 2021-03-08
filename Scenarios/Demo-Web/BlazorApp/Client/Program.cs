using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Configure logging
            builder.Logging.SetMinimumLevel(LogLevel.Debug); // Debug does not work

            // Add PHP
            builder.RootComponents.Add(typeof(PhpBlazor.PhpScriptProvider), "#app");
            builder.Services.AddSingleton(new PhpBlazor.PhpComponentRouteManager(new[] { typeof(force).Assembly}));

            await builder.Build().RunAsync();
        }
    }
}
