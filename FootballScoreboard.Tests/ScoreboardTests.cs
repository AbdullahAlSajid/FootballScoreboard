
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

        #region StartMatch cases


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
        public void StartMatch_WithDuplicateHomeAndAwayTeamOfInProgressMatch_ShouldThrowException()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Mexico", "Canada"));
            Assert.That(ex.Message, Does.Contain("The match is already in progress"));
        }

        [Test]
        public void StartMatch_WithReversedHomeAndAwayTeamOfInProgressMatch_ShouldThrowException()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("Canada", "Mexico"));
            Assert.That(ex.Message, Does.Contain("Teams are already playing in a match"));
        }

        [TestCase("Mexico", "Brazil")]
        [TestCase("Spain", "Canada")]
        public void StartMatch_WithInProgressHomeOrAwayTeam_ShouldThrowException(string secondHomeTeam, string secondAwayTeam)
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch(secondHomeTeam, secondAwayTeam));
            Assert.That(ex.Message, Does.Contain("is already in another match"));
        }

        [TestCase(null, "Canada")]
        [TestCase("", "Canada")]
        [TestCase("Mexico", null)]
        [TestCase("Mexico", "")]
        public void StartMatch_WithInvalidTeams_ShouldThrowException(string? homeTeam, string? awayTeam)
        {
            var ex = Assert.Throws<ArgumentException>(() => _scoreboard.StartMatch(homeTeam, awayTeam));
            Assert.That(ex.Message, Does.Contain("team name cannot be null or empty"));
        }

        [Test]
        public void StartMatch_WithDifferentCasing_ShouldTreatAsSameMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("mexico", "canada"));
            Assert.That(ex.Message, Does.Contain("The match is already in progress"));
        }

        [Test]
        public void StartMatch_WithReversedCasing_ShouldTreatAsSameMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch("CANADA", "MEXICO"));
            Assert.That(ex.Message, Does.Contain("Teams are already playing in a match"));
        }

        #endregion


        #region UpdateMatch cases

        [Test]
        public void UpdateScore_WithValidScore_ShouldUpdateMatchScore()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            _scoreboard.UpdateScore("Mexico", "Canada", 2, 1);
            var summary = _scoreboard.GetSummary();

            Assert.That(summary[0].HomeScore, Is.EqualTo(2));
            Assert.That(summary[0].AwayScore, Is.EqualTo(1));
        }

        [Test]
        public void UpdateScore_WithNonExistentMatch_ShouldThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.UpdateScore("Mexico", "Canada", 1, 0));
            Assert.That(ex.Message, Does.Contain("match doesn't exist"));

        }

        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void UpdateScore_WithNegativeScore_ShouldThrowException(int homeScore, int awayScore)
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex= Assert.Throws<ArgumentException>(() => _scoreboard.UpdateScore("Mexico", "Canada", homeScore, awayScore));
            Assert.That(ex.Message, Does.Contain("can't be negative"));
        }

        [TestCase(null, "Canada")]
        [TestCase("", "Canada")]
        [TestCase("Mexico", null)]
        [TestCase("Mexico", "")]
        public void UpdateMatch_WithInvalidTeams_ShouldThrowException(string? homeTeam, string? awayTeam)
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<ArgumentException>(() => _scoreboard.UpdateScore(homeTeam, awayTeam, 1, 0));
            Assert.That(ex.Message, Does.Contain("team name cannot be null or empty"));
        }


        [Test]
        public void UpdateScore_WithDifferentCasing_ShouldUpdateExistingMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            _scoreboard.UpdateScore("mexico", "canada", 3, 2);

            var summary = _scoreboard.GetSummary();
            Assert.That(summary[0].HomeScore, Is.EqualTo(3));
            Assert.That(summary[0].AwayScore, Is.EqualTo(2));
        }

        #endregion


    }
}
