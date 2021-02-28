using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public static class ComponentUtils
    {
        public static void StateHasChanged(Context ctx) => ((BlazorContext)ctx).ComponentStateHadChanged();
        public static void CallAfterRender(Context ctx, IPhpCallable function) => ((BlazorContext)ctx).CallAfterRender(function);
    }
}
