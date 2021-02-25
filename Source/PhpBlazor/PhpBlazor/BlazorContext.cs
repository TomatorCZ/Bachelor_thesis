using Microsoft.JSInterop;
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
            ctx.Output = Console.Out;
            //
            ctx.AutoloadFiles();

            //
            return ctx;
        }

        public void ComponentStateHadChanged() => _component.Changed();

        public void StartRender(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            Output = new BlazorWriter(builder);
        }

        public void StopRender()
        {
            Output.Dispose();
            Output = Console.Out;
        }

        [JSInvokable]
        public void CallFromJS(string function, params object[] args)
        {
            Call(function, PhpValue.FromClr(args));
        }

        public TResult CallToJS<TResult>(string function, params object[] args)
        {
            return ((IJSInProcessRuntime)_component.JS).Invoke<TResult>(function, args);
        }

        public void CallToJSVoid(string function, params object[] args) => ((IJSInProcessRuntime)_component.JS).InvokeVoid(function, args);
    }
}
