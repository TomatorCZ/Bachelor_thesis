using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Pchp.Core;
using System;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public enum SessionLifetime { Persistant, OnNavigationChanged }
    public enum PhpScriptProviderType { Router, ScriptProvider, Script }

    public class PhpScriptProvider : IComponent, IHandleAfterRender, IDisposable
    {
        #region Parameters
        [Parameter]
        public PhpScriptProviderType Type { get; set; } = PhpScriptProviderType.Router;

        [Parameter]
        public string ScriptName { get; set; } = "index.php";

        [Parameter]
        public RenderFragment NotFound { get; set; } = delegate (Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { };

        [Parameter]
        public RenderFragment Navigating { get; set; } = delegate (Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder) { };

        [Parameter]
        public SessionLifetime ContextLifetime { get; set; } = SessionLifetime.OnNavigationChanged;
        #endregion

        #region Injection
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        private PhpComponentRouteManager RouteManager { get; set; }

        [Inject]
        private ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private INavigationInterception NavigationInterception { get; set; }
        #endregion

        private RenderHandle _renderHandle;
        private ILogger<PhpScriptProvider> _logger;
        private bool _navigationInterceptionEnabled;
        private BlazorContext _ctx;

        #region IComponent
        void IComponent.Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _logger = LoggerFactory.CreateLogger<PhpScriptProvider>();
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            _ctx = BlazorContext.Create(this);

            Refresh();
            return Task.CompletedTask;
        }
        #endregion

        private void Refresh()
        {
            // Find Script
            var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var querryParameters = QueryHelpers.ParseQuery(NavigationManager.ToAbsoluteUri(relativeUri).Query);

            if (relativeUri.IndexOf('?') > -1)
                relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));

            if (Type != PhpScriptProviderType.Script)
            {
                ScriptName = String.IsNullOrWhiteSpace(relativeUri) ? "index" : relativeUri;
                if (!ScriptName.EndsWith(".php"))
                    ScriptName = ScriptName + ".php";
            }
                
            Log.NavigatingToScript(_logger, ScriptName, NavigationManager.Uri);
            Log.QuerryParameters(_logger, querryParameters);

            _ctx.SetGet(querryParameters);
            _ctx.SetPost();
            var task = _ctx.SetFilesAsync();

            task.ContinueWith((Task t) => Render(ScriptName));
            
            if (task.Status != TaskStatus.RanToCompletion)
            {
                if (Navigating != null)
                    _renderHandle.Render(Navigating);
            }
            else
            {
                Render(ScriptName);
            }
        }

        private void Render(string ScriptName)
        {
            var script = Context.TryGetDeclaredScript(ScriptName);

            if (!script.IsValid)
            {
                var prepareForSegmentation = ScriptName.EndsWith(".php") ? ScriptName.Remove(ScriptName.Length - 4) : ScriptName;
                var segments = prepareForSegmentation.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var result = RouteManager.Match(segments);

                if (result.IsMatch)
                {
                    _renderHandle.Render((builder) => {
                        builder.OpenComponent(0, result.MatchedRoute.Handler);
                        builder.CloseElement();
                    });
                }
                else
                {
                    _renderHandle.Render(NotFound);
                }
            }
            else
            {
                _renderHandle.Render((builder) => script.Evaluate(_ctx, builder));
            }
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs args)
        {
            if (ContextLifetime == SessionLifetime.OnNavigationChanged)
                _ctx = BlazorContext.Create(this);

            Refresh();
        }

        #region IHandleAfterRender
        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled && Type == PhpScriptProviderType.Router)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            _ctx.CallJsVoid(JsResource.turnFormsToClient);

            return Task.CompletedTask;
        }
        #endregion

        #region IDisposable
        void IDisposable.Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
        #endregion
    }
}
