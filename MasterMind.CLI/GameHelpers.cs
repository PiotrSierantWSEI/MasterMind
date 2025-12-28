namespace MasterMind.CLI;

using System.Collections.Generic;
using System.Linq;
using MasterMind.Core;
using MasterMind.Core.Models;

public static class GameConstants
{
    public const int DefaultCodeLength = 4;
    public const int DefaultMaxAttempts = 12;
    public static readonly Color[] DefaultColors = { Color.Red, Color.Yellow, Color.Green, Color.Blue, Color.Magenta, Color.Cyan };

    public static readonly Dictionary<char, Color> ColorCharMap = new()
    {
        { 'R', Color.Red },
        { 'Y', Color.Yellow },
        { 'G', Color.Green },
        { 'B', Color.Blue },
        { 'M', Color.Magenta },
        { 'C', Color.Cyan }
    };

    // Mapy do kolorowania w konsoli
    public static readonly Dictionary<Color, string> ColorEscapeMap = new()
    {
        { Color.Red, "\x1b[91m" },
        { Color.Yellow, "\x1b[93m" },
        { Color.Green, "\x1b[92m" },
        { Color.Blue, "\x1b[94m" },
        { Color.Magenta, "\x1b[95m" },
        { Color.Cyan, "\x1b[96m" }
    };

    // Mapy do wyświetlania kolorowych kropek w konsoli
    public static readonly Dictionary<Color, string> ColorCircleMap = new()
    {
        { Color.Red, "\x1b[91m●\x1b[0m" },
        { Color.Yellow, "\x1b[93m●\x1b[0m" },
        { Color.Green, "\x1b[92m●\x1b[0m" },
        { Color.Blue, "\x1b[94m●\x1b[0m" },
        { Color.Magenta, "\x1b[95m●\x1b[0m" },
        { Color.Cyan, "\x1b[96m●\x1b[0m" }
    };
}

public static class UIHelpers
{

    public static void PrintHeader(string text)
    {
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine($"║  {text,-32}  ║");
        Console.WriteLine("╚════════════════════════════════════╝");
    }

    public static void PrintHeaderWithSubtitle(string title, string subtitle)
    {
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine($"║  {title,-32}  ║");
        Console.WriteLine($"║  {subtitle,-32}  ║");
        Console.WriteLine("╚════════════════════════════════════╝");
    }

    public static void PrintColorLegend(Color[] colors)
    {
        Console.WriteLine("Legenda kolorów:");
        foreach (var color in colors)
        {
            var escape = GameConstants.ColorEscapeMap[color];
            var letter = color.ToString()[0];
            Console.WriteLine($"  {escape}{letter}\x1b[0m = {color}");
        }
    }

    public static string ColorizeCode(Code code) =>
        string.Join(" ", code.Sequence.Select(s => SymbolToCircle(s)));

    public static string SymbolToCircle(IGameSymbol symbol)
    {
        if (symbol is ColorSymbol cs)
            return GameConstants.ColorCircleMap[cs.Color];
        else if (symbol is DigitSymbol ds)
            return $"\x1b[97m{ds.Display}\x1b[0m"; // Białe cyfry
        else
            return symbol.Display;
    }
}
