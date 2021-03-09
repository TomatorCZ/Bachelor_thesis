using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static void AddPhp(this WebAssemblyHostBuilder builder, Assembly[] assemblies)
        {
            builder.Services.AddSingleton(new PhpComponentRouteManager(assemblies));
        }
    }
}
