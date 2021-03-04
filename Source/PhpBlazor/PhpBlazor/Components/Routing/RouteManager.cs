using Pchp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class RouteManager
    {
        public Route[] Routes { get; private set; }

        public static void Initiliase(Assembly[] assemblies)
        {
            foreach (var assm in assemblies)
                Context.AddScriptReference(assm);
            
        }

        public static MatchResult Match(string[] uriSegments)
        {
            return MatchResult.Empty;
        }
    }
}
