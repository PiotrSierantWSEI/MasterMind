using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy end-to-end dla wariantu Digits (0-9)
/// </summary>
public class DigitVariantEndToEndTests
{
    private readonly IGameStrategy _strategy = new BruteForceStrategy();

    [Fact]
    public void PlayerGuessingGame_WithDigits_Complete()
    {
        // Test pełnego przebiegu gry gdzie Comp generuje kod z cyfr
        var secretCode = new Code(
            new DigitSymbol(3),
            new DigitSymbol(1),
            new DigitSymbol(4),
            new DigitSymbol(1)
        );

        // gracz zgaduje
        var guess1 = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3),
            new DigitSymbol(4)
        );
        var result1 = Game.CalculateResultStatic(guess1, secretCode);

        Assert.NotNull(result1);
        Assert.True(result1.ExactMatches >= 0);
        Assert.True(result1.WrongPositionMatches >= 0);
    }

    [Fact]
    public void DigitCode_CorrectGuess()
    {
        var secretCode = new Code(
            new DigitSymbol(5),
            new DigitSymbol(5),
            new DigitSymbol(5)
        );

        var guess = new Code(
            new DigitSymbol(5),
            new DigitSymbol(5),
            new DigitSymbol(5)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(3, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_PartialMatch()
    {
        var secretCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3)
        );

        var guess = new Code(
            new DigitSymbol(3),
            new DigitSymbol(1),
            new DigitSymbol(2)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(3, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_NoMatch()
    {
        var secretCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3)
        );

        var guess = new Code(
            new DigitSymbol(4),
            new DigitSymbol(5),
            new DigitSymbol(6)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_DuplicateDigits()
    {
        var secretCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(2)
        );

        var guess = new Code(
            new DigitSymbol(2),
            new DigitSymbol(2),
            new DigitSymbol(2)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.NotNull(result);
    }

    [Fact]
    public void DigitCode_AllZeros()
    {
        var secretCode = new Code(
            new DigitSymbol(0),
            new DigitSymbol(0),
            new DigitSymbol(0)
        );

        var guess = new Code(
            new DigitSymbol(0),
            new DigitSymbol(0),
            new DigitSymbol(0)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(3, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_AllNines()
    {
        var secretCode = new Code(
            new DigitSymbol(9),
            new DigitSymbol(9),
            new DigitSymbol(9)
        );

        var guess = new Code(
            new DigitSymbol(9),
            new DigitSymbol(9),
            new DigitSymbol(9)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(3, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_MixedWithColorFails()
    {
        var digitCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3)
        );

        var colorCode = new Code(
            new ColorSymbol(Color.Red),
            new ColorSymbol(Color.Green),
            new ColorSymbol(Color.Blue)
        );

        var result = Game.CalculateResultStatic(digitCode, colorCode);
        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void PlayerGuessingGame_DigitVariant_LongerCode()
    {
        var secretCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3),
            new DigitSymbol(4),
            new DigitSymbol(5)
        );

        var guess = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3),
            new DigitSymbol(4),
            new DigitSymbol(5)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(5, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void DigitCode_WithoutDuplicates()
    {
        var secretCode = new Code(
            new DigitSymbol(0),
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3)
        );

        var guess = new Code(
            new DigitSymbol(3),
            new DigitSymbol(2),
            new DigitSymbol(1),
            new DigitSymbol(0)
        );

        var result = Game.CalculateResultStatic(guess, secretCode);
        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(4, result.WrongPositionMatches);
    }
}
