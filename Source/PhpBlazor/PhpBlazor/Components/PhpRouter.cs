using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public enum SessionLifetime { Persistant, OnNavigationChange}
    public class PhpRouter : IComponent, IHandleAfterRender, IDisposable
    {
        #region Component's parameters
        [Parameter]
        public Assembly[] Assemblies { get; set; }
        
        [Parameter]
        public SessionLifetime SessionLifetime { get; set; } = SessionLifetime.Persistant;

        [Parameter] 
        public RenderFragment<BlazorContext> NotFound { get; set; }
        [Parameter] 
        public RenderFragment<RouteData> Found { get; set; }
        #endregion

        #region Component's injects
        [Inject] 
        public RouteManager RouteManager { get; set; }
        [Inject] 
        private NavigationManager NavigationManager { get; set; }
        [Inject] 
        private INavigationInterception NavigationInterception { get; set; }
        #endregion

        private BlazorContext _context = null;
        bool _navigationInterceptionEnabled = false;
        RenderHandle _renderHandle;

        #region IComponent
        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);


             _context = BlazorContext.Create();

            RouteManager.Initiliase(Assemblies);

            Refresh();
            return Task.CompletedTask;
        }
        #endregion

        public void HandleLocationChanged(object sender, LocationChangedEventArgs args) {
            if (SessionLifetime == SessionLifetime.OnNavigationChange)
                _context = BlazorContext.Create();
            Refresh();
        } 
        
        public void Refresh()
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var querryParameters = QueryHelpers.ParseQuery(relativeUri);

            if (relativeUri.IndexOf('?') > -1)
            {
                relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));
            }

            var segments = relativeUri.Trim().Split('/', StringSplitOptions.RemoveEmptyEntries);
            var matchResult = RouteManager.Match(segments);
            
            if (matchResult.IsMatch)
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("QuerryPart", querryParameters);
                parameters.Add("Context", _context);
                parameters.Add("Script", String.Join('/',matchResult.MatchedRoute.UriSegments));

                var routeData = new RouteData(matchResult.MatchedRoute.Handler, parameters);
                _renderHandle.Render(Found(routeData));
            }
            else
            {
                _renderHandle.Render(NotFound(_context));
            }

        }

        #region IHandleAfterRender
        public Task OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
            _context?.Dispose();
        }
        #endregion

    }
}
