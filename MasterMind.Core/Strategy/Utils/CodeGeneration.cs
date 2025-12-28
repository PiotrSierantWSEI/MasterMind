using MasterMind.Core.Models;

namespace MasterMind.Core.Strategy;

public static class CodeGeneration
{
    public static List<Code> GenerateAllPossibleCodes(Color[] availableColors, int codeLength)
    {
        var codes = new List<Code>();
        GenerateCodesRecursive(availableColors, codeLength, [], codes);
        return codes;
    }

    private static void GenerateCodesRecursive(Color[] colors, int remaining, List<Color> current, List<Code> result)
    {
        if (remaining == 0)
        {
            result.Add(new Code(current.ToArray()));
            return;
        }

        foreach (var color in colors)
        {
            current.Add(color);
            GenerateCodesRecursive(colors, remaining - 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }
}
