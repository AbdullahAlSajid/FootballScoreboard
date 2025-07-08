
namespace FootballScoreboard.Tests
{
    public class ScoreboardTests
    {
        [Test]
        public void StartMatch_WithHomeAndAwayTeam_ShouldAddMatch()
        {
            var scoreboard = new Scoreboard();

            scoreboard.StartMatch("Mexico", "Canada");

            var summary = scoreboard.GetSummary();

            Assert.That(summary.Count, Is.EqualTo(1));
            Assert.That(summary[0].HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(summary[0].AwayTeam, Is.EqualTo("Canada"));
            Assert.That(summary[0].HomeScore, Is.EqualTo(0));
            Assert.That(summary[0].AwayScore, Is.EqualTo(0));
        }
    }
}
