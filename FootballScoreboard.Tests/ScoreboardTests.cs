
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

            var match = summary.Single();
            Assert.That(match.HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(match.AwayTeam, Is.EqualTo("Canada"));
            Assert.That(match.HomeScore, Is.EqualTo(0));
            Assert.That(match.AwayScore, Is.EqualTo(0));
        }

        [Test]
        public void StartMatch_WithMultipleMatches_ShouldAddAllMatches()
        {
            _scoreboard.StartMatch("Mexico", "Canada");
            _scoreboard.StartMatch("Spain", "Brazil");

            var summary = _scoreboard.GetSummary();

            Assert.That(summary.Count, Is.EqualTo(2));

            Assert.That(summary, Has.One.Matches<Match>(m => m.HomeTeam == "Mexico" && m.AwayTeam == "Canada"));

            Assert.That(summary, Has.One.Matches<Match>(m => m.HomeTeam == "Spain" && m.AwayTeam == "Brazil"));
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

        [TestCase("mexico", "Brazil")]
        [TestCase("Spain", "canada")]
        public void StartMatch_WithInProgressHomeOrAwayTeam_WithReversedCasing_ShouldTreatTheTeamAsSame(string secondHomeTeam, string secondAwayTeam)
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.StartMatch(secondHomeTeam, secondAwayTeam));
            Assert.That(ex.Message, Does.Contain("is already in another match"));
        }

        #endregion


        #region UpdateMatch cases

        [Test]
        public void UpdateScore_WithValidScore_ShouldUpdateMatchScore()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            _scoreboard.UpdateScore("Mexico", "Canada", 2, 1);

            var summary = _scoreboard.GetSummary();

            var match = summary.Single();
            Assert.That(match.HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(match.AwayTeam, Is.EqualTo("Canada"));
            Assert.That(match.HomeScore, Is.EqualTo(2));
            Assert.That(match.AwayScore, Is.EqualTo(1));
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

            var match = summary.Single();

            Assert.That(match.HomeScore, Is.EqualTo(3));
            Assert.That(match.AwayScore, Is.EqualTo(2));
        }

        #endregion


        #region FinishMatch cases

        [Test]
        public void FinishMatch_WithExistingMatch_ShouldRemoveMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");
            _scoreboard.StartMatch("Spain", "Brazil");

            _scoreboard.FinishMatch("Mexico", "Canada");

            var summary = _scoreboard.GetSummary();

            Assert.That(summary.Count, Is.EqualTo(1));

            var match = summary.Single();

            Assert.That(match.HomeTeam, Is.EqualTo("Spain"));
            Assert.That(match.AwayTeam, Is.EqualTo("Brazil"));
            Assert.That(match.HomeScore, Is.EqualTo(0));
            Assert.That(match.AwayScore, Is.EqualTo(0));
        }

        [Test]
        public void FinishMatch_WithNonExistentMatch_ShouldThrowException()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => _scoreboard.FinishMatch("Mexico", "Canada"));
            Assert.That(ex.Message, Does.Contain("match doesn't exist"));
        }

        [Test]
        public void FinishMatch_WithEmptyScoreboard_ShouldReturnEmptyList()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            _scoreboard.FinishMatch("Mexico", "Canada");

            var summary = _scoreboard.GetSummary();
            Assert.That(summary.Count, Is.EqualTo(0));
        }

        [Test]
        public void FinishMatch_WithDifferentCasing_ShouldRemoveMatch()
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            _scoreboard.FinishMatch("mexico", "CANADA");

            var summary = _scoreboard.GetSummary();
            Assert.That(summary.Count, Is.EqualTo(0));
        }

        [TestCase(null, "Canada")]
        [TestCase("", "Canada")]
        [TestCase("Mexico", null)]
        [TestCase("Mexico", "")]
        public void FinishMatch_WithInvalidTeams_ShouldThrowException(string? homeTeam, string? awayTeam)
        {
            _scoreboard.StartMatch("Mexico", "Canada");

            var ex = Assert.Throws<ArgumentException>(() => _scoreboard.FinishMatch(homeTeam, awayTeam));
            Assert.That(ex.Message, Does.Contain("team name cannot be null or empty"));
        }

        #endregion

        #region GetSummary cases

        [Test]
        public void GetSummary_ShouldReturnMatchesOrderedByTotalScoreDescending_AndByMostRecentlyStarted()
        {
            _scoreboard.StartMatch("Mexico", "Canada");
            _scoreboard.StartMatch("Spain", "Brazil");
            _scoreboard.StartMatch("Germany", "France");

            _scoreboard.UpdateScore("Mexico", "Canada", 2, 2);    // totalScore 4
            _scoreboard.UpdateScore("Spain", "Brazil", 1, 1);     // totalScore 2
            _scoreboard.UpdateScore("Germany", "France", 2, 2);   // totalScore 4

            var summary = _scoreboard.GetSummary();

            Assert.That(summary.Count, Is.EqualTo(3));

            // Matches with total 4 should come first, newest first.
            Assert.That(summary[0].HomeTeam, Is.EqualTo("Germany")); //Germany vs France started after Mexico vs Canada
            Assert.That(summary[0].AwayTeam, Is.EqualTo("France"));

            Assert.That(summary[1].HomeTeam, Is.EqualTo("Mexico"));
            Assert.That(summary[1].AwayTeam, Is.EqualTo("Canada"));

            // Match with total 2 should be last
            Assert.That(summary[2].HomeTeam, Is.EqualTo("Spain"));
            Assert.That(summary[2].AwayTeam, Is.EqualTo("Brazil"));
        }

        [Test]
        public void GetSummary_WithNoMatches_ShouldReturnEmptyList()
        {
            var summary = _scoreboard.GetSummary();
            Assert.That(summary.Count, Is.EqualTo(0));
        }

        #endregion
    }
}
