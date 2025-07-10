# Football Scoreboard

A simple library (using C#, .NET and NUnit) for managing all the ongoing matches and their scores during the World Cup.

## Features

- Start new matches with initial score 0-0
- Update match scores
- Finish matches in progress
- Get summary of matches ordered by total score and most recently started match

## Development Approach
This project follows Test-Driven Development (TDD).

## Getting Started

### Project Structure

```
FootballScoreboard/
├── Scoreboard.cs     # Main scoreboard logic
├── Match.cs          # Match data model
└── Tests/
    └── ScoreboardTests.cs # Comprehensive test suite
```

### Prerequisites

- .NET 8.0 or later
- NUnit (for running tests)

### Build

Clone the repository and build the solution:

```
git clone https://github.com/AbdullahAlSajid/FootballScoreboard
cd FootballScoreboard
dotnet build
```

### Run Tests

```
dotnet test
```

## Usage

### Basic Example

```
using FootballScoreboard;

// Create a new scoreboard
var scoreboard = new Scoreboard();

// Start matches
scoreboard.StartMatch("Mexico", "Canada");
scoreboard.StartMatch("Spain", "Brazil");
scoreboard.StartMatch("Germany", "France");

// Update scores
scoreboard.UpdateScore("Mexico", "Canada", 0, 5);
scoreboard.UpdateScore("Spain", "Brazil", 10, 2);
scoreboard.UpdateScore("Germany", "France", 2, 2);

// Get summary (ordered by total score, then by most recent)
var summary = scoreboard.GetSummary();

// Finish a match
scoreboard.FinishMatch("Mexico", "Canada");
```

### Library Reference

#### `StartMatch(string homeTeam, string awayTeam)`
Starts a new match with the specified teams and initial score of 0-0.

**Parameters:**
- `homeTeam`: Name of the home team
- `awayTeam`: Name of the away team

#### `UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore)`
Updates the score of an existing match.

**Parameters:**
- `homeTeam`: Name of the home team
- `awayTeam`: Name of the away team  
- `homeScore`: New home team score
- `awayScore`: New away team score

#### `FinishMatch(string homeTeam, string awayTeam)`
Removes a match from the scoreboard.

**Parameters:**
- `homeTeam`: Name of the home team
- `awayTeam`: Name of the away team

#### `GetSummary()`
Returns a list of all ongoing matches ordered by total score (descending) and by most recently started for matches with the same total score.

**Returns:** `List<Match>` - List of ongoing matches


## Assumptions Made

1. **Team Name Matching**: Team names are case-insensitive. "Mexico" and "mexico" are treated as the same team.

2. **Match Uniqueness**: A team can only participate in one match at a time. The system prevents:
   - Starting a match with the same teams ("Mexico vs Canada" when "Mexico vs Canada" already exists)
   - Starting a match with reversed teams ("Canada vs Mexico" when "Mexico vs Canada" already exists)
   - A team playing in multiple matches simultaneously

3. **Score Validation**: Scores cannot be negative. The system validates this during score updates.

4. **Start Time Tracking**: Match start time is recorded using `DateTime.UtcNow` for consistent ordering of matches with the same total score.

5. **Absolute Scores**: The `UpdateScore` method accepts absolute scores, not incremental changes. Decreasing of any team score is allowed.

## Testing

The solution includes comprehensive unit tests covering:

- ✅ All happy path scenarios
- ✅ Edge cases (null/empty inputs, negative scores)
- ✅ Error conditions and exception handling
- ✅ Case sensitivity handling
- ✅ Sorting and ordering logic
- ✅ Multiple match scenarios

