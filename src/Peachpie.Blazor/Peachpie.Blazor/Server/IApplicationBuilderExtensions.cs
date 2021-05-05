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
        public static void UseAdditionalWebStaticAssets(this IApplicationBuilder app, IConfiguration config)
        {
            foreach (var item in config.GetSection("AdditionalWebStaticAssets").GetChildren())
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
