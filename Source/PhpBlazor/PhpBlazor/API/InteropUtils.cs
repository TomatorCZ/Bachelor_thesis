using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public static class InteropUtils
    {
        public static void CallJsVoid(Context ctx, string method, params object[] args) => ((BlazorContext)ctx).CallJsVoid(method, args);

        public static void CallJsCustomVoid(Context ctx, string method, int arg1, string arg2) => ((BlazorContext)ctx).CallJsVoid(method, arg1, arg2);

        public static string ToString(string value) => value;
    }

    [PhpType]
    public static partial class GenericHelper
    {
        public static TResult CallJs<TResult>(Context ctx, string method, params object[] args)
        {
            return ((BlazorContext)ctx).CallJs<TResult>(method, args);
        }

        public static PhpArray CallJsArray<TResult>(Context ctx, string method, params object[] args)
        {
            return new PhpArray(((BlazorContext)ctx).CallJs<TResult[]>(method, args));
        }
    }
}
