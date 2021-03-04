using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public class MatchResult
    {
        public MatchResult(bool isMatch, Route matchedRoute)
        {
            IsMatch = isMatch;
            MatchedRoute = matchedRoute;
        }

        public bool IsMatch { get; }
        public Route MatchedRoute { get; }


        public static MatchResult Empty => new MatchResult(false, null);
    }
}
