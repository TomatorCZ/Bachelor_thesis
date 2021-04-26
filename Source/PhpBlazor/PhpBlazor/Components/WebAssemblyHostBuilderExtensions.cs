using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Peachpie.Blazor
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static void AddPhp(this WebAssemblyHostBuilder builder, Assembly[] assemblies)
        {
            builder.Services.AddSingleton(new PhpComponentRouteManager(assemblies));
        }
    }
}
