using MasterMind.Core;
using MasterMind.Core.Models;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy dla DigitSymbol
/// </summary>
public class DigitSymbolTests
{
    [Fact]
    public void DigitSymbol_Creation_Valid()
    {
        var digit = new DigitSymbol(5);

        Assert.Equal(5, digit.Digit);
        Assert.Equal("5", digit.Display);
        Assert.Equal("Digit", digit.SymbolType);
    }

    [Fact]
    public void DigitSymbol_Creation_MinMax()
    {
        var min = new DigitSymbol(0);
        var max = new DigitSymbol(9);

        Assert.Equal(0, min.Digit);
        Assert.Equal(9, max.Digit);
    }

    [Fact]
    public void DigitSymbol_Creation_OutOfRange_Throws()
    {
        Assert.Throws<ArgumentException>(() => new DigitSymbol(-1));
        Assert.Throws<ArgumentException>(() => new DigitSymbol(10));
    }

    [Fact]
    public void DigitSymbol_Equals_Same()
    {
        var digit1 = new DigitSymbol(3);
        var digit2 = new DigitSymbol(3);

        Assert.True(digit1.Equals(digit2));
    }

    [Fact]
    public void DigitSymbol_Equals_Different()
    {
        var digit1 = new DigitSymbol(3);
        var digit2 = new DigitSymbol(4);

        Assert.False(digit1.Equals(digit2));
    }

    [Fact]
    public void DigitSymbol_Equals_WithColor_False()
    {
        var digit = new DigitSymbol(3);
        var color = new ColorSymbol(Color.Red);

        Assert.False(digit.Equals(color));
    }

    [Fact]
    public void DigitSymbol_TryParse_Valid()
    {
        var digit = DigitSymbol.TryParse("5");

        Assert.NotNull(digit);
        Assert.Equal(5, digit!.Digit);
    }

    [Fact]
    public void DigitSymbol_TryParse_Invalid()
    {
        var digit1 = DigitSymbol.TryParse("X");
        var digit2 = DigitSymbol.TryParse("10");
        var digit3 = DigitSymbol.TryParse("-1");

        Assert.Null(digit1);
        Assert.Null(digit2);
        Assert.Null(digit3);
    }

    [Fact]
    public void DigitSymbol_ToString()
    {
        var digit = new DigitSymbol(7);

        Assert.Equal("7", digit.ToString());
    }

    [Fact]
    public void DigitSymbol_GetHashCode()
    {
        var digit1 = new DigitSymbol(5);
        var digit2 = new DigitSymbol(5);
        var digit3 = new DigitSymbol(6);

        Assert.Equal(digit1.GetHashCode(), digit2.GetHashCode());
        Assert.NotEqual(digit1.GetHashCode(), digit3.GetHashCode());
    }
}

/// <summary>
/// Testy dla gry z cyframi
/// </summary>
public class DigitGameTests
{
    [Fact]
    public void Game_WithDigits_CalculateResult_ExactMatch()
    {
        var secret = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));
        var proposal = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));

        var result = Game.CalculateResultStatic(proposal, secret);

        Assert.Equal(3, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void Game_WithDigits_CalculateResult_NoMatch()
    {
        var secret = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));
        var proposal = new Code(new DigitSymbol(4), new DigitSymbol(5), new DigitSymbol(6));

        var result = Game.CalculateResultStatic(proposal, secret);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void Game_WithDigits_CalculateResult_WrongPosition()
    {
        var secret = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));
        var proposal = new Code(new DigitSymbol(2), new DigitSymbol(3), new DigitSymbol(1));

        var result = Game.CalculateResultStatic(proposal, secret);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(3, result.WrongPositionMatches);
    }

    [Fact]
    public void Game_WithDigits_CalculateResult_Mixed()
    {
        var secret = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3), new DigitSymbol(4));
        var proposal = new Code(new DigitSymbol(1), new DigitSymbol(5), new DigitSymbol(3), new DigitSymbol(2));

        var result = Game.CalculateResultStatic(proposal, secret);

        Assert.Equal(2, result.ExactMatches); 
        Assert.Equal(1, result.WrongPositionMatches);
    }

    [Fact]
    public void Code_WithDigits_Creation()
    {
        var symbols = new IGameSymbol[]
        {
            new DigitSymbol(0),
            new DigitSymbol(5),
            new DigitSymbol(9)
        };

        var code = new Code(symbols);

        Assert.Equal(3, code.Sequence.Count);
        Assert.True(code.Sequence[0].Equals(new DigitSymbol(0)));
        Assert.True(code.Sequence[1].Equals(new DigitSymbol(5)));
        Assert.True(code.Sequence[2].Equals(new DigitSymbol(9)));
    }

    [Fact]
    public void Code_WithDigits_Equals()
    {
        var code1 = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));
        var code2 = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));
        var code3 = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(4));

        Assert.True(code1.Equals(code2));
        Assert.False(code1.Equals(code3));
    }

    [Fact]
    public void Code_Mixed_ColorAndDigit_NotEqual()
    {
        var codeColor = new Code(Color.Red, Color.Yellow, Color.Green);
        var codeDigit = new Code(new DigitSymbol(1), new DigitSymbol(2), new DigitSymbol(3));

        Assert.False(codeColor.Equals(codeDigit));
    }
}

/// <summary>
/// Testy dla ColorSymbol
/// </summary>
public class ColorSymbolTests
{
    [Fact]
    public void ColorSymbol_Creation_Valid()
    {
        var symbol = new ColorSymbol(Color.Red);

        Assert.Equal(Color.Red, symbol.Color);
        Assert.Equal("Red", symbol.Display);
        Assert.Equal("Color", symbol.SymbolType);
    }

    [Fact]
    public void ColorSymbol_Equals_Same()
    {
        var symbol1 = new ColorSymbol(Color.Blue);
        var symbol2 = new ColorSymbol(Color.Blue);

        Assert.True(symbol1.Equals(symbol2));
    }

    [Fact]
    public void ColorSymbol_Equals_Different()
    {
        var symbol1 = new ColorSymbol(Color.Blue);
        var symbol2 = new ColorSymbol(Color.Red);

        Assert.False(symbol1.Equals(symbol2));
    }

    [Fact]
    public void ColorSymbol_TryParse_Valid()
    {
        var symbol = ColorSymbol.TryParse("Red");

        Assert.NotNull(symbol);
        Assert.Equal(Color.Red, symbol!.Color);
    }

    [Fact]
    public void ColorSymbol_TryParse_Prefix()
    {
        var symbol = ColorSymbol.TryParse("r");

        Assert.NotNull(symbol);
        Assert.Equal(Color.Red, symbol!.Color);
    }

    [Fact]
    public void ColorSymbol_AllColors()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Magenta, Color.Cyan };

        foreach (var color in colors)
        {
            var symbol = new ColorSymbol(color);
            Assert.NotNull(symbol);
            Assert.Equal(color, symbol.Color);
        }
    }
}
