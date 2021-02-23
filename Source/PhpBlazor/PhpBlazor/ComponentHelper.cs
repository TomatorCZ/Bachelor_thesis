using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

[assembly: PhpExtension]

namespace PhpBlazor
{
    public static class ComponentHelper
    {
        public static void StateHasChanged(Context ctx) => ((BlazorContext)ctx).ComponentStateHadChanged();

        public static TResult CallJS<TResult>(Context ctx, string method, params object[] args)
        {
            return ((BlazorContext)ctx).CallToJS<TResult>(method, args);
        }
        public static void CallJSVoid(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallToJSVoid(method, args);

        public static void Foo<T>(T arg)
        {
            
        }
    }
}
