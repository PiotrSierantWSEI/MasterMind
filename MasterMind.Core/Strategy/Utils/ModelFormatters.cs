using MasterMind.Core.Models;

namespace MasterMind.Core.Strategy;

public static class ModelFormatters
{
    public static string Format(Code code)
    {
        return string.Concat(code.Sequence.Select(c => (c.ToString() ?? "?")[0]));
    }

    public static string Format(GameResult result)
    {
        return $"[Dokładne: {result.ExactMatches}, Niedokładne: {result.WrongPositionMatches}]";
    }

    public static string Format(Attempt attempt)
    {
        return $"Próba {attempt.AttemptNumber}: {Format(attempt.Proposal)} => {Format(attempt.Result)}";
    }
}
