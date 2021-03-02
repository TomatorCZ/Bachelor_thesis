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

        public static void Initiliase(Assembly[] assemblies) => throw new NotImplementedException();

        public static MatchResult Match(string[] uriSegments) => throw new NotImplementedException();
    }
}
