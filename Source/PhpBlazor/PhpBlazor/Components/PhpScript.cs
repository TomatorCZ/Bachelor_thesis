using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class  PhpScript : ComponentBase, IDisposable
    {
        #region Component's parameters
        [Parameter]
        public string Script { get; set; }

        [Parameter]
        public BlazorContext Context { get; set; }
        
        [Parameter]
        public Assembly[] Assemblies { get; set; }

        [Parameter]
        public Dictionary<string, StringValues> QuerryPart { get; set; }
        #endregion

        #region Component's injects
        [Inject]
        public NavigationManager Navigation { get; set; }
        
        [Inject]
        public IJSRuntime Js { get; set; }
        #endregion

        private Context.ScriptInfo _script;
        private bool _sharedContext = true;

        #region ComponentBase
        public override Task SetParametersAsync(ParameterView parameters)
        {
            base.SetParametersAsync(parameters);

            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);

            if (QuerryPart == null)
                QuerryPart = QueryHelpers.ParseQuery(uri.Query);

            if (Script == null)
                Script = Navigation.ToBaseRelativePath(Navigation.Uri);

            initializeSession();
            
            return Task.CompletedTask;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRenderAsync(firstRender);

            Context.OnAfterRender();

            // Convert forms to client side.
            Context.CallJsVoid("window.php.forms.turnFormsToClientSide");

            return Task.CompletedTask;
        }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (_script.IsValid)
                _script.Evaluate(Context, builder);
            else
                Console.WriteLine($"Script {_script} is invalid");
        }
        #endregion

        private void initializeSession()
        {
            if (Context == null)
            {
                _sharedContext = false;
                Context = BlazorContext.Create(this);
            }
            else
            {
                Context.SetCurrentComponent(this);
            }

            if (Assemblies != null)
            {
                foreach (Assembly assem in Assemblies)
                {
                    if (assem != null)
                        BlazorContext.AddScriptReference(assem);
                }
            }

            Context.SetGet(QuerryPart);
            Context.SetPost();
            Context.SetFiles();

            _script = BlazorContext.TryGetDeclaredScript(Script);
        }

        public void Changed() => StateHasChanged();

        #region IDisposable
        public void Dispose()
        {
            if (!_sharedContext)
            {
                Context?.Dispose();
            }
        }
        #endregion
    }
}
