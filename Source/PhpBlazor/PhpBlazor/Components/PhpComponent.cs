using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace PhpBlazor
{
    public abstract class PhpComponent : ComponentBase
    {
        #region Component's parameters
        [Parameter]
        public Dictionary<string, StringValues> QuerryPart { get; set; }
        #endregion

        protected sealed override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            BuildRenderTree(new RenderTreeBuilder(builder, this));
        }

        protected abstract void BuildRenderTree(RenderTreeBuilder builder);
    }
}
