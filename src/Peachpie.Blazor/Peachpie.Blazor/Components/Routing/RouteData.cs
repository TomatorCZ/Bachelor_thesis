using System;
using System.Collections.Generic;

namespace Peachpie.Blazor
{
    public struct RouteData
    {
        public Type Handler { get; private set; }
        public Dictionary<string, object> Parameters { get; set; }

        public RouteData(Type handler, Dictionary<string, object> parameters)
        {
            Handler = handler;
            Parameters = parameters;
        }
    }
}
