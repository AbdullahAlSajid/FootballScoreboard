
namespace FootballScoreboard
{
    public class Scoreboard
    {
        private List<Match> _inProgressMatches = new List<Match>();

        public void StartMatch(string homeTeam, string awayTeam)
        {
            ValidateTeamNames(homeTeam, awayTeam);
            CheckMatchNotAlreadyInProgress(homeTeam, awayTeam);
            CheckTeamsNotInAnotherMatch(homeTeam, awayTeam);

            var match = new Match(homeTeam, awayTeam);
            _inProgressMatches.Add(match);
        }

        public void UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
        {
            ValidateTeamNames(homeTeam, awayTeam);

            if (homeScore < 0)
                throw new ArgumentException("Home score can't be negative");

            if (awayScore < 0)
                throw new ArgumentException("Away score can't be negative");

            var match = FindMatch(homeTeam, awayTeam);
            if (match == null)
            {
                throw new InvalidOperationException("The match doesn't exist");
            }

            match.UpdateScore(homeScore, awayScore);
        }

        public void FinishMatch(string homeTeam, string awayTeam)
        {
            ValidateTeamNames(homeTeam, awayTeam);

            var match = FindMatch(homeTeam, awayTeam);
            if (match == null)
            {
                throw new InvalidOperationException("The match doesn't exist");
            }

            _inProgressMatches.Remove(match);
        }

        public List<Match> GetSummary()
        {
            //Matches are ordered by their total score (HomeScore+AwayScore). The matches with the 
            //same total score will be returned ordered by the most recently started match in the scoreboard.

            var sortedSummary = _inProgressMatches.OrderByDescending(m => m.HomeScore + m.AwayScore).ThenByDescending(m => m.StartTime).ToList();

            return sortedSummary;
        }

        private Match? FindMatch(string homeTeam, string awayTeam)
        {
            foreach (var match in _inProgressMatches)
            {
                if (string.Equals(match.HomeTeam, homeTeam, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(match.AwayTeam, awayTeam, StringComparison.OrdinalIgnoreCase))
                {
                    return match;
                }
            }

            return null;
        }

        private void ValidateTeamNames(string homeTeam, string awayTeam)
        {
            if (string.IsNullOrWhiteSpace(homeTeam))
                throw new ArgumentException("Home team name cannot be null or empty");

            if (string.IsNullOrWhiteSpace(awayTeam))
                throw new ArgumentException("Away team name cannot be null or empty");

            if (string.Equals(homeTeam, awayTeam, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Home and away teams must be different");
        }

        private void CheckMatchNotAlreadyInProgress(string homeTeam, string awayTeam)
        {
            foreach (var match in _inProgressMatches)
            {
                if (string.Equals(match.HomeTeam, homeTeam, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(match.AwayTeam, awayTeam, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("The match is already in progress");
                }

                if (string.Equals(match.HomeTeam, awayTeam, StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(match.AwayTeam, homeTeam, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Teams are already playing in a match");
                }
            }
        }

        private void CheckTeamsNotInAnotherMatch(string homeTeam, string awayTeam)
        {
            foreach (var match in _inProgressMatches)
            {
                if (string.Equals(match.HomeTeam, homeTeam, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(match.AwayTeam, homeTeam, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"{homeTeam} is already in another match");
                }

                if (string.Equals(match.HomeTeam, awayTeam, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(match.AwayTeam, awayTeam, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException($"{awayTeam} is already in another match");
                }
            }
        }

    }
}
