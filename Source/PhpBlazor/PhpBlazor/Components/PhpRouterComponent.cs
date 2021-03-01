﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.JSInterop;

[assembly: PhpExtension]

namespace PhpBlazor
{
    public class PhpRouterComponent : ComponentBase, IDisposable
    {
        [Parameter] public Assembly[] Assemblies { get; set; }
        [Parameter] public string ScriptName { get; set; }
        [Inject] public NavigationManager Navigation { get; set; }
        [Inject] public IJSRuntime JS {get;set;}


        private DotNetObjectReference<BlazorContext> _objRef;
        private BlazorContext _context;
        private Context.ScriptInfo _script;

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (_script.IsValid)
                _script.Evaluate(_context, builder);
            else
                Console.WriteLine($"Script {_script} is invalid");
        }

        protected override void OnInitialized()
		{
            base.OnInitialized();
            //Navigation.LocationChanged += handleLocationChanged;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            initializeSession();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _context.CallAfterRender();
        }

        private void initializeSession()
        {
            _context = BlazorContext.Create(this);
            // References to scripts
            foreach (Assembly assem in Assemblies)
            {
                if (assem != null)
                    BlazorContext.AddScriptReference(assem);
            }
            
            //Init JS
            _objRef = DotNetObjectReference.Create(_context);
            ((JSInProcessRuntime)JS).InvokeVoid("php.init", _objRef);

            // Querry
            //https://chrissainty.com/working-with-query-strings-in-blazor/
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);

            foreach (var item in QueryHelpers.ParseQuery(uri.Query))
                _context.Get.Add(item.Key, item.Value);

            // Script name
            string scriptName = ScriptName;// uri.Segments.LastOrDefault();
            _script = Context.TryGetDeclaredScript(scriptName);
        }

        private void handleLocationChanged(object sender, LocationChangedEventArgs e) 
        {
            _objRef?.Dispose();
            initializeSession();
            StateHasChanged();
        }

        public void Changed() => StateHasChanged();

        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}