using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy scenariuszowe - różne warianty gier z kolorami i cyframi
/// </summary>
public class GameScenarioTests
{
    // ============= WARIANTY Z KOLORAMI =============

    [Fact]
    public void ColorVariant_PlayerGuessing_Standard_3Colors_Length3()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green };
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: 3, maxAttempts: 8, allowedCheats: 0);
        var secretCode = new Code(new[] { Color.Red, Color.Yellow, Color.Green });

        var guess = new Code(new[] { Color.Red, Color.Yellow, Color.Green });
        game.ProvideComputerFeedback(guess, new GameResult(3, 0));

        Assert.Equal(GameState.Won, game.State);
        Assert.Single(game.Attempts);
    }

    [Fact]
    public void ColorVariant_PlayerGuessing_Standard_6Colors_Length4()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Magenta, Color.Cyan };
        var strategy = new FilteringStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: 4, maxAttempts: 8, allowedCheats: 0);

        var guess = new Code(new[] { Color.Red, Color.Blue, Color.Magenta, Color.Cyan });
        game.ProvideComputerFeedback(guess, new GameResult(4, 0));

        Assert.Equal(GameState.Won, game.State);
    }

    [Fact]
    public void ColorVariant_PlayerGuessing_AllowedCheating_ExceedingCheats_3Colors()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green };
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: 3, maxAttempts: 8, allowedCheats: 1);
        var secretCode = new Code(new[] { Color.Red, Color.Yellow, Color.Green });

        var attempt1 = new Code(new[] { Color.Red, Color.Yellow, Color.Green });
        game.ProvideComputerFeedback(attempt1, new GameResult(0, 0));

        var attempt2 = new Code(new[] { Color.Blue, Color.Blue, Color.Blue });
        game.ProvideComputerFeedback(attempt2, new GameResult(1, 0));

        var attempt3 = new Code(new[] { Color.Red, Color.Red, Color.Red });
        game.ProvideComputerFeedback(attempt3, new GameResult(0, 1));

        var cheatsDetected = game.CheckForCheating(secretCode);
        Assert.True(cheatsDetected);
        Assert.Equal(3, game.DetectedCheats);
    }

    [Fact]
    public void ColorVariant_ComputerGuessing_Standard_4Colors_Length3()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue };
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: 3, maxAttempts: 8, allowedCheats: 0);

        var guess1 = game.GetComputerGuess();
        game.ProvideComputerFeedback(guess1, new GameResult(0, 1));

        Assert.NotNull(guess1);
        Assert.Equal(GameState.InProgress, game.State);
    }

    // ============= WARIANTY Z CYFRAMI =============

    [Fact]
    public void DigitVariant_PlayerGuessing_Standard_AllDigits_Length3()
    {
        var digits = Enumerable.Range(0, 10)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: 3, maxAttempts: 8, allowedCheats: 0);
        var secretCode = new Code(new[] {
            (IGameSymbol)new DigitSymbol(7),
            new DigitSymbol(9),
            new DigitSymbol(2)
        });

        var guess = new Code(new[] {
            (IGameSymbol)new DigitSymbol(7),
            new DigitSymbol(9),
            new DigitSymbol(2)
        });
        game.ProvideComputerFeedback(guess, new GameResult(3, 0));

        Assert.Equal(GameState.Won, game.State);
    }

    [Fact]
    public void DigitVariant_PlayerGuessing_Standard_5Digits_Length4()
    {
        var digits = Enumerable.Range(0, 5)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new FilteringStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: 4, maxAttempts: 8, allowedCheats: 0);
        var secretCode = new Code(new[] {
            (IGameSymbol)new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3),
            new DigitSymbol(4)
        });

        var guess = new Code(new[] {
            (IGameSymbol)new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3),
            new DigitSymbol(4)
        });
        game.ProvideComputerFeedback(guess, new GameResult(4, 0));

        Assert.Equal(GameState.Won, game.State);
    }

    [Fact]
    public void DigitVariant_PlayerGuessing_AllowedCheating_3Cheats_10Digits_Length5()
    {
        var digits = Enumerable.Range(0, 10)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: 5, maxAttempts: 8, allowedCheats: 3);

        var secretCode = new Code(new[] {
            (IGameSymbol)new DigitSymbol(2),
            new DigitSymbol(5),
            new DigitSymbol(7),
            new DigitSymbol(1),
            new DigitSymbol(8)
        });

        var guess = new Code(new[] {
            (IGameSymbol)new DigitSymbol(2),
            new DigitSymbol(5),
            new DigitSymbol(7),
            new DigitSymbol(1),
            new DigitSymbol(8)
        });
        game.ProvideComputerFeedback(guess, new GameResult(5, 0));

        Assert.Equal(GameState.Won, game.State);
        Assert.Single(game.Attempts);
    }

    [Fact]
    public void DigitVariant_ComputerGuessing_Standard_6Digits_Length3()
    {
        var digits = Enumerable.Range(0, 6)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: 3, maxAttempts: 8, allowedCheats: 0);

        var guess1 = game.GetComputerGuess();
        game.ProvideComputerFeedback(guess1, new GameResult(1, 1)); 

        Assert.NotNull(guess1);
        Assert.Equal(GameState.InProgress, game.State);
    }

    // ============= MIESZANE SCENARIUSZE =============

    [Fact]
    public void MixedScenario_ColorVariant_LongCode_ManyAttempts()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Magenta };
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: 5, maxAttempts: 15, allowedCheats: 1);
        var secretCode = new Code(new[] {
            Color.Magenta, Color.Red, Color.Yellow, Color.Green, Color.Blue
        });

        var attempts = 0;
        while (game.State == GameState.InProgress && attempts < 15)
        {
            var guess = game.GetComputerGuess();
            game.ProvideComputerFeedback(guess, new GameResult(0, 0));
            attempts++;
        }

        Assert.True(attempts > 0);
        Assert.NotEqual(GameState.InProgress, game.State);
    }

    [Fact]
    public void MixedScenario_DigitVariant_LongCode_Standard()
    {
        var digits = Enumerable.Range(0, 8)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: 5, maxAttempts: 12, allowedCheats: 0);

        for (int i = 0; i < 4; i++)
        {
            if (game.State != GameState.InProgress) break;
            var guess = game.GetComputerGuess();
            game.ProvideComputerFeedback(guess, new GameResult(0, 1));
        }

        Assert.NotNull(game.Attempts);
        Assert.True(game.Attempts.Count <= 4);
    }

    [Theory]
    [InlineData(2, 3)]
    [InlineData(3, 4)]
    [InlineData(4, 5)]
    [InlineData(6, 3)]
    public void ColorVariant_VariableParameters_ColorCount_CodeLength(int colorCount, int codeLength)
    {
        var colors = Enum.GetValues<Color>()
            .Cast<Color>()
            .Take(colorCount)
            .ToArray();
        var strategy = new BruteForceStrategy();
        var game = new ComputerGuessingGame(strategy, colors, codeLength: codeLength, maxAttempts: 8, allowedCheats: 0);

        var guess = game.GetComputerGuess();

        Assert.NotNull(guess);
        Assert.Equal(codeLength, guess.Sequence.Count);
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(5, 4)]
    [InlineData(8, 5)]
    [InlineData(10, 3)]
    public void DigitVariant_VariableParameters_DigitCount_CodeLength(int digitCount, int codeLength)
    {
        var digits = Enumerable.Range(0, digitCount)
            .Select(i => (IGameSymbol)new DigitSymbol(i))
            .ToArray();
        var strategy = new FilteringStrategy();
        var game = new ComputerGuessingGame(strategy, digits, codeLength: codeLength, maxAttempts: 8, allowedCheats: 1);

        var guess = game.GetComputerGuess();

        Assert.NotNull(guess);
        Assert.Equal(codeLength, guess.Sequence.Count);
    }
}
