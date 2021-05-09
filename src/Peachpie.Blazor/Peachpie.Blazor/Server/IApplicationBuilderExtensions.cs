using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peachpie.Blazor
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// Provides navigation to static resources defined in appsettings.json. It finds the AdditionalStaticWebAssets section, where are defined Path of the assets and BasePath used in a browser.
        /// </summary>
        public static void UseAdditionalWebStaticAssets(this IApplicationBuilder app, IConfiguration config)
        {
            foreach (var item in config.GetSection("AdditionalStaticWebAssets").GetChildren())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(item["Path"]),
                    RequestPath = item["BasePath"]
                });
            }
        }
    }
}
