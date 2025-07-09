
namespace FootballScoreboard
{
    public class Match
    {
        public string HomeTeam { get; private set; }
        public string AwayTeam { get; private set; }
        public int HomeScore { get; private set; }
        public int AwayScore { get; private set; }


        public Match(string homeTeam, string awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            HomeScore = 0;
            AwayScore = 0;
        }
        public void UpdateScore(int homeScore, int awayScore)
        {
            HomeScore = homeScore;
            AwayScore = awayScore;
        }

    }
}
