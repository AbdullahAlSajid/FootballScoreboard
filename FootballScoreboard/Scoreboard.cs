
namespace FootballScoreboard
{
    public class Scoreboard
    {
        private List<Match> _matches = new List<Match>();

        public void StartMatch(string homeTeam, string awayTeam)
        {
            var match = new Match(homeTeam, awayTeam);
            _matches.Add(match);
        }
        public List<Match> GetSummary()
        {
            return _matches;
        }

    }
}
