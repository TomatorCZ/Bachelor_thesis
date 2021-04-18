using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Pchp.Core;
using System;

namespace PhpBlazor
{
    public abstract class PhpComponent : ComponentBase
    {
        protected Context _ctx;

        [Parameter]
        public Context Ctx 
        {
            get => _ctx;
            set { _ctx = value; }
        }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public ILoggerFactory LoggerFactory { get; set; }

        protected sealed override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            BuildRenderTree(new RenderTreeBuilder(builder, this));
        }

        protected abstract void BuildRenderTree(RenderTreeBuilder builder);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _ctx ??= BlazorContext.Create(Js, LoggerFactory);
        }
    }
}
