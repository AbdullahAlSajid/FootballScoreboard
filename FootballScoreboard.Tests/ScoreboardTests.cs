
namespace FootballScoreboard.Tests
{
    public class ScoreboardTests
    {
        private Scoreboard _scoreboard;

        [SetUp]
        public void SetUp()
        {
            _scoreboard = new Scoreboard();
        }

        [Test]
        public void StartMatch_WithHomeAndAwayTeam_ShouldAddMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var summary = _scoreboard.GetSummary();

            Assert.That(summary.Count, Is.EqualTo(1));
            Assert.That(summary[0].HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(summary[0].AwayTeam, Is.EqualTo("Canada"));
            Assert.That(summary[0].HomeScore, Is.EqualTo(0));
            Assert.That(summary[0].AwayScore, Is.EqualTo(0));
        }

        [Test]
        public void StartMatch_WithMultipleMatches_ShouldAddAllMatches()
        {
            _scoreboard.StartMatch("Mexico", "Canada");
            _scoreboard.StartMatch("Spain", "Brazil");

            var summary = _scoreboard.GetSummary();
            Assert.That(summary.Count, Is.EqualTo(2));
            Assert.That(summary[0].HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(summary[0].AwayTeam, Is.EqualTo("Canada"));
            Assert.That(summary[1].HomeTeam, Is.EqualTo("Spain"));
            Assert.That(summary[1].AwayTeam, Is.EqualTo("Brazil"));
        }

        [Test]
        public void StartMatch_WithSameHomeAndAwayTeam_ShouldThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch("Mexico", "Mexico"));
            Assert.That(ex.Message, Does.Contain("Home and away teams must be different"));
        }

        [Test]
        public void StartMatch_WithDuplicateMatch_ShouldThrowException()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Mexico", "Canada"));
            Assert.That(ex.Message, Does.Contain("The match is already in progress"));

            ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Canada", "Mexico"));
            Assert.That(ex.Message, Does.Contain("The match is already in progress"));
        }

        [Test]
        public void StartMatch_WithInProgressHomeOrAwayTeam_ShouldThrowException()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Mexico", "Brazil"));
            Assert.That(ex.Message, Does.Contain("Home team is already in a match"));

            ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Spain", "Canada"));
            Assert.That(ex.Message, Does.Contain("Away team is already in a match"));
        }

        [Test]
        public void StartMatch_WithNullHomeTeam_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch(null, "Canada"));
            Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch("Mexico", null));

        }

        [Test]
        public void StartMatch_WithEmptyAwayTeam_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch("Mexico", ""));
            Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch("", "Canada"));
        }
    }
}
