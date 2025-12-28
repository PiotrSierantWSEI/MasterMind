using MasterMind.Core.Models;
using MasterMind.CLI.Models;
using Xunit;

namespace MasterMind.Tests;

public class GameVariantTests
{
    [Fact]
    public void GameSettings_Default_HasStandardVariant()
    {
        var settings = GameSettings.CreateDefault();

        Assert.Equal(GameVariant.Standard, settings.Variant);
        Assert.Equal(2, settings.AllowedCheats);
    }

    [Fact]
    public void GameSettings_WithAllowedCheatingVariant_StoresTolerance()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green };

        var settings = new GameSettings(
            colors,
            codeLength: 3,
            maxAttempts: 10,
            variant: GameVariant.AllowedCheating,
            allowedCheats: 2
        );

        Assert.Equal(GameVariant.AllowedCheating, settings.Variant);
        Assert.Equal(2, settings.AllowedCheats);
    }

    [Fact]
    public void GameVariant_Standard_HasValueOne()
    {
        Assert.Equal(1, (int)GameVariant.Standard);
    }

    [Fact]
    public void GameVariant_AllowedCheating_HasValueTwo()
    {
        Assert.Equal(2, (int)GameVariant.AllowedCheating);
    }

    [Fact]
    public void GameSettings_AllowedCheatsRange_ZeroToThree()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green };

        for (int cheats = 0; cheats <= 3; cheats++)
        {
            var settings = new GameSettings(colors, 3, 10, GameVariant.AllowedCheating, SymbolType.Colors, cheats);
            Assert.Equal(cheats, settings.AllowedCheats);
        }
    }

    [Fact]
    public void GameSettings_WithVariant_PreservesOtherProperties()
    {
        var colors = new[] { Color.Red, Color.Yellow, Color.Green, Color.Blue };

        var settings = new GameSettings(colors, 4, 12, GameVariant.AllowedCheating, SymbolType.Colors, 2);

        Assert.Equal(GameVariant.AllowedCheating, settings.Variant);
        Assert.Equal(4, settings.CodeLength);
        Assert.Equal(12, settings.MaxAttempts);
        Assert.Equal(4, settings.AvailableColors.Length);
    }
}
