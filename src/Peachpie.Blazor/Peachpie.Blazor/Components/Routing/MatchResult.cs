namespace Peachpie.Blazor
{
    public class MatchResult
    {
        private MatchResult(bool isMatch, Route matchedRoute)
        {
            IsMatch = isMatch;
            MatchedRoute = matchedRoute;
        }

        public bool IsMatch { get; }
        public Route MatchedRoute { get; }

        public static MatchResult Match(Route matchedRoute) => new MatchResult(true, matchedRoute);
        public static MatchResult NoMatch() => new MatchResult(false, null);
    }
}
