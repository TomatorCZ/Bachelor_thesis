using Pchp.Core;
using Pchp.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PhpBlazor;
using Microsoft.AspNetCore.Components;

namespace PhpBlazor
{
    public class RouteManager
    {
        public static Route[] Routes { get; private set; }

        public static void Initiliase(Assembly[] assemblies)
        {
            List<Route> routes = new List<Route>();

            foreach (var assm in assemblies)
            {
                Context.AddScriptReference(assm);

                foreach (var t in assm.ExportedTypes)
                {
                    if (t.IsAbstract && t.IsSealed)
                    {
                        var sattr = ReflectionUtils.GetScriptAttribute(t);
                        if (sattr != null && sattr.Path != null && t.GetCustomAttribute<PharAttribute>() == null)
                        {
                            routes.Add(new Route(typeof(PhpScript), sattr.Path.Split('/')));
                        }
                    }
                    else if (t.IsSubclassOf(typeof(PhpComponent)))
                    {
                        var pattr = Attribute.GetCustomAttribute(t, typeof(RouteAttribute)) as RouteAttribute;
                        if (pattr != null)
                        {
                            routes.Add(new Route(t, pattr.Template.Split('/')));
                        }
                    }
                }
            }

            Routes = routes.ToArray();
        }

        public static MatchResult Match(string[] segments)
        {
            if (segments.Length == 0)
            {
                var indexRoute =Routes.SingleOrDefault(x => x.UriSegments.Length == 1 && x.UriSegments[0] == "index.php");
                return (indexRoute == null) ? MatchResult.NoMatch() : MatchResult.Match(indexRoute);
            }

            foreach (var item in Routes)
            {
                var result = item.Match(segments);
                if (result.IsMatch)
                    return result;
            }

            return MatchResult.NoMatch();
        }
    }
}
