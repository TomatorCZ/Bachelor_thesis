using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using PhpBlazor;
using System.Collections.Generic;
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
            builder.AddPhp(new[] { typeof(force).Assembly });
            builder.RootComponents.Add(typeof(PhpScriptProvider), "#app");
            
            await builder.Build().RunAsync();
        }
    }
}
