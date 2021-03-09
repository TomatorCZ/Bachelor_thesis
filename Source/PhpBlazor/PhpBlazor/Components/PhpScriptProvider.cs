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
        public ILoggerFactory LoggerFactory { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private INavigationInterception NavigationInterception { get; set; }
        #endregion

        private RenderHandle _renderHandle;
        private ILogger<PhpScriptProvider> _logger;
        private bool _navigationInterceptionEnabled;
        private BlazorContext _ctx;
        private string _previousRelativeUri = null;

        #region IComponent
        void IComponent.Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _logger = LoggerFactory.CreateLogger<PhpScriptProvider>();

            _previousRelativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (_previousRelativeUri.IndexOf('?') > -1)
                _previousRelativeUri = _previousRelativeUri.Substring(0, _previousRelativeUri.IndexOf('?'));
            
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (ContextLifetime == SessionLifetime.OnNavigationChanged || _ctx == null)
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
                
            Log.QuerryParameters(_logger, querryParameters);

            _ctx.SetGet(querryParameters);
            _ctx.SetPost();
            var task = _ctx.SetFilesAsync();

            if (task.Status != TaskStatus.RanToCompletion)
            {
                if (Navigating != null)
                    _renderHandle.Render(Navigating);

                task.ContinueWith((Task t) => Render(ScriptName));
            }
            else
            {
                Render(ScriptName);
            }
        }

        private void Render(string ScriptName)
        {
            Log.RenderingScript(_logger, ScriptName);
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
            var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (relativeUri.IndexOf('?') > -1)
                relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));

            if (Type == PhpScriptProviderType.ScriptProvider || (Type == PhpScriptProviderType.Script && relativeUri != _previousRelativeUri))
            {
                _previousRelativeUri = relativeUri;
                return;
            }

            _previousRelativeUri = relativeUri;

            Log.Navigating(_logger, args.Location);

            if (ContextLifetime == SessionLifetime.OnNavigationChanged)
            {
                _ctx.Dispose();
                _ctx = BlazorContext.Create(this);
            }

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
            if (_ctx != null)
            {
                _ctx?.Dispose();
            }
        }
        #endregion
    }
}
