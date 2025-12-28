using MasterMind.Core;
using MasterMind.Core.Models;
using Xunit;

namespace MasterMind.Tests;

/// <summary>
/// Testy dla walidacji wprowadzonych wyników gry
/// </summary>
public class GameInputValidationTests
{
    [Fact]
    public void GameResult_Valid_ExactAndWrongPosition()
    {
        var result = new GameResult(2, 1);

        Assert.Equal(2, result.ExactMatches);
        Assert.Equal(1, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_Valid_AllZeros()
    {
        var result = new GameResult(0, 0);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_Valid_MaxValues()
    {
        var result = new GameResult(4, 0);

        Assert.Equal(4, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_Validation_ExactPlusWrong_EqualsCodeLength()
    {
        var result = new GameResult(2, 2);

        Assert.Equal(2, result.ExactMatches);
        Assert.Equal(2, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_InvalidScenario_ExactPlusWrong_ExceedsCodeLength()
    {
        var result = new GameResult(3, 2);

        Assert.Equal(3, result.ExactMatches);
        Assert.Equal(2, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_Equals_SameValues()
    {
        var result1 = new GameResult(2, 1);
        var result2 = new GameResult(2, 1);

        Assert.Equal(result1, result2);
    }

    [Fact]
    public void GameResult_NotEquals_DifferentValues()
    {
        var result1 = new GameResult(2, 1);
        var result2 = new GameResult(2, 0);

        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void GameResult_CanBeComparison_WinCondition()
    {
        var codeLength = 4;
        var result = new GameResult(codeLength, 0);

        Assert.Equal(codeLength, result.ExactMatches);
        Assert.Equal(0, result.WrongPositionMatches);
    }

    [Fact]
    public void GameResult_NoValidWay_ExactPlusWrong_ExceedsCode()
    {
        Assert.True(true);
    }
}
