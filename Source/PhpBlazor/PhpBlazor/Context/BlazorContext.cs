using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
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
        private IJSRuntime _js;

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
            ctx._js = component?.Js;
            
            //
            ctx.AutoloadFiles();

            //
            return ctx;
        }
        #endregion

        public void SetCurrentComponent(PhpScript component)
        {
            _component = component;
            _js = component?.Js;
        }
        public void SetJs(IJSRuntime js) => _js = js;

        public void SetGet(Dictionary<string, StringValues> querry)
        {
            foreach (var item in querry)
            {
                Get.Add(item.Key, item.Value.ToString());
            }
        }

        public void SetPost()
        {
            if (CallJs<bool>(JsResource.IsPost))
            {
                var postData = CallJs<Dictionary<string, string>>(JsResource.getPost);
                foreach (var item in postData)
                {
                    Post.Add(item.Key, item.Value);
                }
            }
        }

        public void SetFiles()
        {
            if (CallJs<bool>(JsResource.IsFiles))
            {
                var files = CallJs<FormFile[]>(JsResource.getFiles);
                foreach (var file in files)
                {
                    Files.Add(file.fieldName, file);
                }
            }
        }

        #region Rendering
        public void ComponentStateHadChanged()
        {
            _component.Changed();
        }

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
        public void CallJsVoid(string function, params object[] args) => (_js as IJSInProcessRuntime).InvokeVoid(function, args);

        public TResult CallJs<TResult>(string function, params object[] args) => (_js as IJSInProcessRuntime).Invoke<TResult>(function, args);

        public void CallJsVoidAsync(string function, params object[] args) => _js.InvokeVoidAsync(function, args);

        public ValueTask<TResult> CallJsAsync<TResult>(string function, params object[] args) => _js.InvokeAsync<TResult>(function, args);
        #endregion
    }
}
