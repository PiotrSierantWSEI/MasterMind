using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.CLI.Services;
using MasterMind.CLI.Models;
using Xunit;

namespace MasterMind.Tests.Services;

public class SymbolTypeIntegrationTests
{
    [Fact]
    public void GameSettings_SupportsDigitSymbolType()
    {
        var settings = new GameSettings(
            availableColors: [Color.Red, Color.Blue],
            codeLength: 4,
            maxAttempts: 10,
            variant: GameVariant.Standard,
            symbolType: SymbolType.Digits,
            allowedCheats: 0
        );

        Assert.Equal(SymbolType.Digits, settings.SymbolType);
    }

    [Fact]
    public void GameSettings_SupportsColorSymbolType()
    {
        var settings = new GameSettings(
            availableColors: [Color.Red, Color.Blue, Color.Green],
            codeLength: 4,
            maxAttempts: 10,
            variant: GameVariant.Standard,
            symbolType: SymbolType.Colors,
            allowedCheats: 0
        );

        Assert.Equal(SymbolType.Colors, settings.SymbolType);
    }

    [Fact]
    public void DigitSymbol_CanParseDigits()
    {
        var symbol = DigitSymbol.TryParse("5");
        Assert.NotNull(symbol);
        Assert.Equal(5, symbol.Digit);

        Assert.Null(DigitSymbol.TryParse("A"));
        Assert.Null(DigitSymbol.TryParse("10"));
        Assert.Null(DigitSymbol.TryParse("-1"));
    }

    [Fact]
    public void ColorSymbol_CanParseColors()
    {
        var symbol = ColorSymbol.TryParse("Red");
        Assert.NotNull(symbol);
        Assert.Equal(Color.Red, symbol.Color);

        Assert.Null(ColorSymbol.TryParse("Purple"));
        Assert.Null(ColorSymbol.TryParse("123"));
    }

    [Fact]
    public void GameVariant_OnlyStandardAndCheating()
    {
        var standard = GameVariant.Standard;
        var cheating = GameVariant.AllowedCheating;

        Assert.NotEqual(standard, cheating);
    }

    [Fact]
    public void GameSettings_Variant_And_SymbolType_Are_Separate()
    {
        var digitCheating = new GameSettings(
            availableColors: [Color.Red, Color.Blue],
            codeLength: 4,
            maxAttempts: 10,
            variant: GameVariant.AllowedCheating,
            symbolType: SymbolType.Digits,
            allowedCheats: 2
        );

        Assert.Equal(GameVariant.AllowedCheating, digitCheating.Variant);
        Assert.Equal(SymbolType.Digits, digitCheating.SymbolType);
        Assert.Equal(2, digitCheating.AllowedCheats);
    }

    [Fact]
    public void Code_WithDigitSymbols()
    {
        var digitCode = new Code(
            new DigitSymbol(1),
            new DigitSymbol(2),
            new DigitSymbol(3)
        );

        Assert.Equal(3, digitCode.Sequence.Count);
        Assert.All(digitCode.Sequence, symbol => Assert.IsType<DigitSymbol>(symbol));
    }

    [Fact]
    public void Code_WithColorSymbols()
    {
        var colorCode = new Code(
            new ColorSymbol(Color.Red),
            new ColorSymbol(Color.Green),
            new ColorSymbol(Color.Blue)
        );

        Assert.Equal(3, colorCode.Sequence.Count);
        Assert.All(colorCode.Sequence, symbol => Assert.IsType<ColorSymbol>(symbol));
    }
}
