using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Pchp.Core;

namespace PhpBlazor
{
    [PhpHidden]
    public static class WebAssemblyHostBuilderExtensions
    {
        public static void AddPhp(this WebAssemblyHostBuilder host)
        {
            host.Services.AddSingleton<RouteManager>();
        }
    }
}
