using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy symulujące pełne przebiegania gry z oszustwem
/// </summary>
public class CheatingVariantEndToEndTests
{
    private readonly Color[] _colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue };
    private readonly IGameStrategy _strategy = new FilteringStrategy();

    [Fact]
    public void GameFlow_PlayerCheatsMildly_WithinTolerance_GameContinues()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempt1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(attempt1, new GameResult(0, 0));

        var attempt2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(attempt2, new GameResult(0, 0));
        var cheatsDetected = game.CheckForCheating(secretCode);

        Assert.False(cheatsDetected);
        Assert.Equal(2, game.DetectedCheats);
        Assert.Equal(2, game.Attempts.Count);
    }

    [Fact]
    public void GameFlow_PlayerExceedsTolerance_GameEndsWithDetection()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 1);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempt1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(attempt1, new GameResult(0, 0));

        var attempt2 = new Code([Color.Red, Color.Blue, Color.Blue]);
        game.ProvideComputerFeedback(attempt2, new GameResult(0, 0));

        var cheatsDetected = game.CheckForCheating(secretCode);

        Assert.True(cheatsDetected);
        Assert.Equal(2, game.DetectedCheats);
    }

    [Fact]
    public void GameFlow_PlayerPlaysHonestly_NoDetection()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 1);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempt1 = new Code([Color.Red, Color.Blue, Color.Blue]);
        var result1 = Game.CalculateResultStatic(attempt1, secretCode);
        game.ProvideComputerFeedback(attempt1, result1);

        var attempt2 = new Code([Color.Yellow, Color.Yellow, Color.Green]);
        var result2 = Game.CalculateResultStatic(attempt2, secretCode);
        game.ProvideComputerFeedback(attempt2, result2);

        Assert.False(game.CheckForCheating(secretCode));
        Assert.Equal(0, game.DetectedCheats);
    }

    [Fact]
    public void GameFlow_ZeroTolerance_FirstCheatStopsGame()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempt1 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(attempt1, new GameResult(0, 0));

        Assert.True(game.CheckForCheating(secretCode));
        Assert.Equal(1, game.DetectedCheats);
    }

    [Fact]
    public void GameFlow_MixedHonestAndDishonest_CountsOnlyDishonest()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempt1 = new Code([Color.Red, Color.Blue, Color.Green]);
        var result1 = Game.CalculateResultStatic(attempt1, secretCode);
        game.ProvideComputerFeedback(attempt1, result1);

        var attempt2 = new Code([Color.Red, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(attempt2, new GameResult(0, 0));

        var attempt3 = new Code([Color.Yellow, Color.Green, Color.Red]);
        var result3 = Game.CalculateResultStatic(attempt3, secretCode);
        game.ProvideComputerFeedback(attempt3, result3);

        var attempt4 = new Code([Color.Blue, Color.Yellow, Color.Green]);
        game.ProvideComputerFeedback(attempt4, new GameResult(0, 0));

        Assert.False(game.CheckForCheating(secretCode));
        Assert.Equal(2, game.DetectedCheats);
        Assert.Equal(4, game.Attempts.Count);
    }

    [Fact]
    public void GameFlow_AttemptLimitReached_BeforeCheatingDetection()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 5, allowedCheats: 1);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        for (int i = 0; i < 5; i++)
        {
            var guess = new Code([Color.Blue, Color.Blue, Color.Blue]);
            var result = Game.CalculateResultStatic(guess, secretCode);
            game.ProvideComputerFeedback(guess, result);
        }

        Assert.Equal(5, game.Attempts.Count);
        Assert.Equal(5, game.Attempts.Count);
    }

    [Fact]
    public void GameFlow_SurvivalAtLimit_ExactlyAtThreshold()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 3);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempts = new[]
        {
            new Code([Color.Red, Color.Yellow, Color.Green]),
            new Code([Color.Red, Color.Blue, Color.Blue]),
            new Code([Color.Blue, Color.Yellow, Color.Green])
        };

        foreach (var guess in attempts)
        {
            game.ProvideComputerFeedback(guess, new GameResult(0, 0));
        }

        Assert.False(game.CheckForCheating(secretCode));
        Assert.Equal(3, game.DetectedCheats);
    }

    [Fact]
    public void GameFlow_OneOverThreshold_Immediate()
    {
        var game = new ComputerGuessingGame(_strategy, _colors, codeLength: 3, maxAttempts: 10, allowedCheats: 2);
        var secretCode = new Code([Color.Red, Color.Yellow, Color.Green]);

        var attempts = new[]
        {
            new Code([Color.Red, Color.Yellow, Color.Green]),
            new Code([Color.Red, Color.Blue, Color.Blue]),
            new Code([Color.Blue, Color.Yellow, Color.Green])
        };

        foreach (var guess in attempts)
        {
            game.ProvideComputerFeedback(guess, new GameResult(0, 0));
        }

        Assert.True(game.CheckForCheating(secretCode));
        Assert.Equal(3, game.DetectedCheats);
    }

    [Fact]
    public void GameFlow_VariationWithDifferentCodes()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Magenta };
        var game = new ComputerGuessingGame(_strategy, colors, codeLength: 4, maxAttempts: 10, allowedCheats: 0);
        var secretCode = new Code([Color.Yellow, Color.Green, Color.Blue, Color.Magenta]);

        var attempt1 = new Code([Color.Yellow, Color.Green, Color.Blue, Color.Magenta]);
        game.ProvideComputerFeedback(attempt1, new GameResult(0, 0));

        Assert.True(game.CheckForCheating(secretCode));
        Assert.Equal(1, game.DetectedCheats);
    }
}
