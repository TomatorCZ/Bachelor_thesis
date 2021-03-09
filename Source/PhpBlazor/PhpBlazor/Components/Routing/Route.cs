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

        public Route(Type handler, string[] uriSegments)
        {
            Handler = handler;
            UriSegments = uriSegments;
        }

        public MatchResult Match(string[] segments)
        {
            if (segments.Length != UriSegments.Length)
            {
                return MatchResult.NoMatch();
            }

            for (var i = 0; i < UriSegments.Length - 1; i++)
            {
                if (string.Compare(segments[i], UriSegments[i], StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return MatchResult.NoMatch();
                }
            }

            // Ignoring .php
            string lastSegment = UriSegments[UriSegments.Length - 1];
            if (lastSegment.EndsWith(".php"))
                lastSegment = lastSegment.Remove(lastSegment.Length - 4);

            if (string.Compare(lastSegment, segments[segments.Length - 1], StringComparison.OrdinalIgnoreCase) != 0)
            {
                return MatchResult.NoMatch();
            }

            return MatchResult.Match(this);
        }
    }
}
