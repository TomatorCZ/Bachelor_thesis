using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class Route
    {
        public Type Handler;
        public string[] UriSegments;

        public MatchResult Match(string[] uriSegments) => throw new NotImplementedException();
    }
}
