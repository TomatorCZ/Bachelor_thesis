using Pchp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace PhpBlazor
{
    public class BlazorContext : Context
    {
        private PhpRouterComponent _component;

        protected BlazorContext(IServiceProvider services) : base(services) 
        {
        }

        public static BlazorContext Create(PhpRouterComponent component) 
        {
            var ctx = new BlazorContext(null)
            {
                RootPath = Directory.GetCurrentDirectory(),
                EnableImplicitAutoload = true,
            };

            ctx.WorkingDirectory = ctx.RootPath;
            ctx.InitOutput(null);
            ctx.InitSuperglobals();
            ctx._component = component;

            //
            ctx.AutoloadFiles();

            //
            return ctx;
        }


        public void StartRender(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            Output = new BlazorWriter(builder);
        }

        public void StopRender()
        {
            Output.Dispose();
        }
    }
}
