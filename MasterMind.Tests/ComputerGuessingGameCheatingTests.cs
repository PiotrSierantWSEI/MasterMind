using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using Xunit;

namespace MasterMind.Tests;

public class ComputerGuessingGameCheatingTests
{
    private readonly Color[] _colors = [Color.Red, Color.Yellow, Color.Green, Color.Blue];
    private readonly IGameStrategy _strategy = new BruteForceStrategy();

    [Fact]
    public void CheckForCheating_NoInconsistencies_ReturnsFalse()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Blue, Color.Green]);
        var result1 = Game.CalculateResultStatic(guess1, secretCode);
        game.ProvideComputerFeedback(guess1, result1);

        var guess2 = new Code([Color.Yellow, Color.Red, Color.Blue]);
        var result2 = Game.CalculateResultStatic(guess2, secretCode);
        game.ProvideComputerFeedback(guess2, result2);

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.False(cheatingDetected);
        Assert.Equal(0, game.DetectedCheats);
    }

    [Fact]
    public void CheckForCheating_OneInconsistency_WithAllowedZero_ReturnsTrue()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        var falseResult1 = new GameResult(ExactMatches: 0, WrongPositionMatches: 0);
        game.ProvideComputerFeedback(guess1, falseResult1);

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.True(cheatingDetected);
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void CheckForCheating_TwoInconsistencies_WithAllowedTwo_ReturnsFalse()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        var falseResult1 = new GameResult(ExactMatches: 0, WrongPositionMatches: 0);
        game.ProvideComputerFeedback(guess1, falseResult1);

        var guess2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        var falseResult2 = new GameResult(ExactMatches: 0, WrongPositionMatches: 0);
        game.ProvideComputerFeedback(guess2, falseResult2);

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.False(cheatingDetected);
        Assert.Equal(2, game.DetectedCheats);
    }

    [Fact]
    public void CheckForCheating_ThreeInconsistencies_WithAllowedTwo_ReturnsTrue()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(guess1, new GameResult(0, 0));

        var guess2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(guess2, new GameResult(0, 0));

        var guess3 = new Code([Color.Yellow, Color.Green, Color.Blue]);
        game.ProvideComputerFeedback(guess3, new GameResult(0, 0));

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.True(cheatingDetected);
        Assert.Equal(3, game.DetectedCheats);
    }

    [Fact]
    public void CheckForCheating_MixedConsistentAndInconsistent_CountsOnlyInconsistent()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 1);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Blue, Color.Green]);
        var correctResult1 = Game.CalculateResultStatic(guess1, secretCode);
        game.ProvideComputerFeedback(guess1, correctResult1);

        var guess2 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(guess2, new GameResult(0, 0));

        var guess3 = new Code([Color.Blue, Color.Blue, Color.Blue]);
        var correctResult3 = Game.CalculateResultStatic(guess3, secretCode);
        game.ProvideComputerFeedback(guess3, correctResult3);

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.False(cheatingDetected);
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void AllowedCheats_ZeroDefault_CreatesZeroTolerance()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10);

        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(
            new Code([Color.Red, Color.Yellow, Color.Green]),
            new GameResult(0, 0)
        );
        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.True(cheatingDetected);
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void DetectedCheats_Property_ReturnsCountBeforeCheckForCheating()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 5);

        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(
            new Code([Color.Red, Color.Yellow, Color.Green]),
            new GameResult(0, 0)
        );

        Assert.Equal(0, game.DetectedCheats);

        game.CheckForCheating(secretCode);

        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void CheckForCheating_NullSecretCode_ReturnsFalse()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);

        var cheatingDetected = game.CheckForCheating(null!);

        Assert.False(cheatingDetected);
        Assert.Equal(0, game.DetectedCheats);
    }

    [Fact]
    public void Cheating_WithExactMatches_DetectsCorrectly()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 4, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green, Color.Blue]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green, Color.Blue]);
        game.ProvideComputerFeedback(guess1, new GameResult(0, 0));

        var guess2 = new Code([Color.Red, Color.Yellow, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(guess2, new GameResult(0, 0));

        var cheatingDetected = game.CheckForCheating(secretCode);

        Assert.True(cheatingDetected);
        Assert.Equal(2, game.DetectedCheats);
    }
}
