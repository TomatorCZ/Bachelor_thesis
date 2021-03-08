using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp4.Client.Pages
{
    [Route("class1")]
    public class Class1 : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            builder.AddMarkupContent(0, "<p>");
            builder.AddContent(1, "Google");
            builder.AddMarkupContent(2, "</p>");
        }
    }
}
