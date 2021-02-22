using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhpBlazor
{
    public static class ScriptInfoExtensions
    {
        public static PhpValue Evaluate(this Context.ScriptInfo script, BlazorContext ctx, Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            ctx.StartRender(builder);
            var result = script.Evaluate(ctx, ctx.Globals, null);
            ctx.StopRender();

            return result;
        }
    }
}
