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
        private DotNetObjectReference<BlazorContext> _objRef;
        private PhpScript _component;
        private IPhpCallable _afterRender;

        #region Create
        protected BlazorContext(IServiceProvider services) : base(services)
        {
            Output = Console.Out;
            _objRef = DotNetObjectReference.Create<BlazorContext>(this);
        }

        public static BlazorContext Create() => Create(null);

        public static BlazorContext Create(PhpScript component)
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
        #endregion

        public void SetCurrentComponent(PhpScript component) => _component = component;

        #region Rendering
        public void ComponentStateHadChanged() => _component.Changed();

        public void StartRender(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            Output = BlazorWriter.CreateTree(builder);
        }

        public void StopRender()
        {
            Output.Flush();
            Output.Dispose();
            Output = BlazorWriter.CreateConsole();
        }

        public void CallAfterRender(IPhpCallable function)
        {
            _afterRender = function;
        }

        public void OnAfterRender() 
        {
            _afterRender?.Invoke(this);
            _afterRender = null;
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _objRef?.Dispose();
        }

        #region JSInterop
        //TODO: CallPhpFromJS
        public void CallJsVoid(string function, params object[] args) =>((IJSInProcessRuntime)_component.Js).InvokeVoid(function, args);

        public TResult CallJs<TResult>(string function, params object[] args) => ((IJSInProcessRuntime)_component.Js).Invoke<TResult>(function, args);
        #endregion
    }
}
