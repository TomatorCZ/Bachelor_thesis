using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class PhpRouteView : IComponent
    {
        [Parameter]
        public RouteData RouteData { get; set; }

        private RenderHandle _renderHandle;

        private readonly RenderFragment _renderPageWithParametersDelegate;

        public PhpRouteView()
        {
            _renderPageWithParametersDelegate = new RenderFragment(Render);
        }


        #region IComponent
        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            _renderHandle.Render(_renderPageWithParametersDelegate);

            return Task.CompletedTask;
        }

        protected virtual void Render(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, RouteData.Handler);
            foreach (KeyValuePair<string, object> routeValue in RouteData.Parameters)
            {
                builder.AddAttribute(1, routeValue.Key, routeValue.Value);
            }
            builder.CloseComponent();
        }
        #endregion
    }
}
