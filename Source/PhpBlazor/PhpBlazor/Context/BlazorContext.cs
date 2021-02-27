using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class BlazorContext : Context
    {
        private PhpRouterComponent _component;
        private IPhpCallable _callAfterRender;

        protected BlazorContext(IServiceProvider services) : base(services){}

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

        #region Rendering
        public void ComponentStateHadChanged() => _component.Changed();

        public void StartRender(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            Output = BlazorWriter.CreateTree(builder);
        }

        public void StopRender()
        {
            Output.Dispose();
            Output = BlazorWriter.CreateConsole();
        }

        public void CallAfterRender(IPhpCallable function)
        {
            _callAfterRender = function;
        }

        public void CallAfterRender()
        {
            _callAfterRender?.Invoke(this);
            _callAfterRender = null;
        }
        #endregion

        #region JSInterop
        [JSInvokable("CallPhpVoid")]
        public void CallPhpVoid(string function) => Call(function);

        [JSInvokable("CallPhpString")]
        public void CallPhpVoid(string function, string data) => Call(function, data);

        public void CallJsVoid(string function, params object[] args)
        {
            ((IJSInProcessRuntime)_component.JS).InvokeVoid(function, args);
        }

        public void CallJsVoid(string function, params int[] args)
        {
            ((IJSInProcessRuntime)_component.JS).InvokeVoid(function, args);
        }

        public TResult CallJs<TResult>(string function, params object[] args) => ((IJSInProcessRuntime)_component.JS).Invoke<TResult>(function, args);
        #endregion
    }
}
