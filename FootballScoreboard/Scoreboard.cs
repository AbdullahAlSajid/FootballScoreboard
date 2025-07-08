
namespace FootballScoreboard
{
    public class Scoreboard
    {
        private List<Match> _inProgressMatches = new List<Match>();

        public void StartMatch(string homeTeam, string awayTeam)
        {
            if (string.IsNullOrWhiteSpace(homeTeam) || string.IsNullOrWhiteSpace(awayTeam))
                throw new ArgumentException("Team name cannot be null or empty");

            if (homeTeam == awayTeam)
                throw new ArgumentException("Home and away teams must be different");

            foreach (var inProgressMatch in _inProgressMatches)
            {
                if (inProgressMatch.HomeTeam == homeTeam && inProgressMatch.AwayTeam == awayTeam || inProgressMatch.AwayTeam == homeTeam && inProgressMatch.AwayTeam == homeTeam)
                    throw new InvalidOperationException("The match is already in progress");

                if (inProgressMatch.HomeTeam == homeTeam || inProgressMatch.AwayTeam == homeTeam)
                    throw new InvalidOperationException("Home team is already in a match");

                if (inProgressMatch.HomeTeam == awayTeam || inProgressMatch.AwayTeam == awayTeam)
                    throw new InvalidOperationException("Away team is already in a match");
            }


            var match = new Match(homeTeam, awayTeam);
            _inProgressMatches.Add(match);
        }
        public List<Match> GetSummary()
        {
            return _inProgressMatches;
        }
    }
}
