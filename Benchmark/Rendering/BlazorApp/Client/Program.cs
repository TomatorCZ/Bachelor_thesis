using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Peachpie.Blazor;
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
            builder.RootComponents.Add<App>("#app");

            // Configure logging
            builder.Logging.SetMinimumLevel(LogLevel.Debug); // Debug does not work

            builder.AddPhp(new[] { typeof(Asteroids.AsteroidsComponent).Assembly });

            await builder.Build().RunAsync();
        }
    }
}
