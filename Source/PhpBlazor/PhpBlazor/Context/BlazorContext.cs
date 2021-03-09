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
        private PhpScriptProvider _component;
        private IJSRuntime _js;
        private FileManager _fileManager;

        #region Create
        protected BlazorContext(IServiceProvider services) : base(services)
        {
            Output = Console.Out;
            _objRef = DotNetObjectReference.Create<BlazorContext>(this);
        }

        public static BlazorContext Create(PhpScriptProvider component)
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
            ctx._js = component.Js;
            ctx._fileManager = new FileManager(ctx);
            
            //
            ctx.AutoloadFiles();

            //
            return ctx;
        }
        #endregion

        #region Rendering
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
        #endregion

        #region Set Globals
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

        public async Task SetFilesAsync()
        {
            var files = _fileManager.FetchFiles();
            foreach (var item in files)
            {
                Files.Add(item.fieldName, item);
            }

            await _fileManager.DownloadFilesAsync();
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
            _objRef?.Dispose();
        }

        public string GetDownloadFile(int id)
        {
            return _fileManager.GetFileData(id);
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
