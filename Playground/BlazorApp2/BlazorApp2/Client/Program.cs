using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Collections.Concurrent;
using System.Linq;

namespace BlazorApp2.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ForceLoadOfPHPAssemblies();
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            await builder.Build().RunAsync();
        }

        public static void ForceLoadOfPHPAssemblies()
        {
            var force1 = typeof(ForceClass);
        }
    }
}
