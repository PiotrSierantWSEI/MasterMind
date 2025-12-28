using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy dla wariantu oszustwa
/// </summary>
public class CheatingVariantGameFlowTests
{
    private readonly Color[] _colors = [Color.Red, Color.Yellow, Color.Green];
    private readonly IGameStrategy _strategy = new BruteForceStrategy();

    [Fact]
    public void Game_DetectsCheat_AtFirstInconsistency_WithZeroTolerance()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(guess1, new GameResult(0, 0));

        var cheating = game.CheckForCheating(secretCode);
        Assert.True(cheating);
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void Game_AllowsCheat_UpToLimit()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(guess1, new GameResult(0, 0));

        var guess2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(guess2, new GameResult(0, 0));

        var guess3 = new Code([Color.Blue, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(guess3, new GameResult(0, 0));

        var cheating = game.CheckForCheating(secretCode);
        Assert.False(cheating);
        Assert.Equal(2, game.DetectedCheats);
    }

    [Fact]
    public void Game_DetectsCheat_WhenExceedsLimit()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(guess1, new GameResult(0, 0));

        var guess2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(guess2, new GameResult(0, 0));

        var guess3 = new Code([Color.Yellow, Color.Green, Color.Blue]);
        game.ProvideComputerFeedback(guess3, new GameResult(0, 0));

        var cheating = game.CheckForCheating(secretCode);
        Assert.True(cheating);
        Assert.Equal(3, game.DetectedCheats);
    }

    [Fact]
    public void Game_CountsOnlyInconsistencies()
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

        var cheating = game.CheckForCheating(secretCode);
        Assert.False(cheating);
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void Game_DetectsMultipleCheatsInSequence()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guesses = new[]
        {
            new Code([Color.Red, Color.Yellow, Color.Green]),
            new Code([Color.Red, Color.Blue, Color.Blue]), 
            new Code([Color.Blue, Color.Yellow, Color.Green]),
            new Code([Color.Yellow, Color.Green, Color.Red])  
        };

        foreach (var guess in guesses)
        {
            game.ProvideComputerFeedback(guess, new GameResult(0, 0));
        }

        var cheating = game.CheckForCheating(secretCode);
        Assert.True(cheating);
        Assert.Equal(4, game.DetectedCheats);
    }

    [Fact]
    public void GameResult_Consistency_SameResultsDontCount()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 1);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var guess = new Code([Color.Red, Color.Blue, Color.Green]);
        var correctResult = Game.CalculateResultStatic(guess, secretCode);

        game.ProvideComputerFeedback(guess, correctResult);
        game.ProvideComputerFeedback(guess, correctResult);

        var cheating = game.CheckForCheating(secretCode);
        Assert.False(cheating);
        Assert.Equal(0, game.DetectedCheats);
    }

    [Fact]
    public void Attempts_TrackAllGuesses_IncludingCheats()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);

        for (int i = 0; i < 5; i++)
        {
            var guess = new Code([Color.Blue, Color.Blue, Color.Blue]);
            game.ProvideComputerFeedback(guess, new GameResult(0, 0));
        }

        Assert.Equal(5, game.Attempts.Count);
    }

    [Fact]
    public void CheatDetection_WorksWithDifferentCodeLengths()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 4, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green, Color.Blue]);

        var guess = new Code([Color.Red, Color.Yellow, Color.Green, Color.Blue]);
        game.ProvideComputerFeedback(guess, new GameResult(0, 0));

        var cheating = game.CheckForCheating(secretCode);
        Assert.True(cheating);
        Assert.Equal(1, game.DetectedCheats);
    }
}
