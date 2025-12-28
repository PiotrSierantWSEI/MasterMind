namespace MasterMind.Core.Models;

/// <summary>
/// Adapter dla Color enum
/// Wraps Color aby pracować z abstrakcją symboli
/// </summary>
public class ColorSymbol : IGameSymbol
{
    private static readonly Dictionary<Color, string> ColorNames = new()
    {
        { Color.Red, "Red" },
        { Color.Yellow, "Yellow" },
        { Color.Green, "Green" },
        { Color.Blue, "Blue" },
        { Color.Magenta, "Magenta" },
        { Color.Cyan, "Cyan" }
    };

    private static readonly Dictionary<string, Color> ColorByName = new()
    {
        { "Red", Color.Red },
        { "Yellow", Color.Yellow },
        { "Green", Color.Green },
        { "Blue", Color.Blue },
        { "Magenta", Color.Magenta },
        { "Cyan", Color.Cyan }
    };

    public Color Color { get; }

    public string Display => ColorNames[Color];
    public string SymbolType => "Color";

    public ColorSymbol(Color color)
    {
        Color = color;
    }

    public static ColorSymbol? TryParse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var key = input.Trim().ToLower();
        var found = ColorByName.FirstOrDefault(kvp => kvp.Key.ToLower().StartsWith(key));

        return found.Key != null ? new ColorSymbol(found.Value) : null;
    }

    public bool Equals(IGameSymbol? other)
    {
        if (other is not ColorSymbol cs)
            return false;
        return Color == cs.Color;
    }

    public override bool Equals(object? obj) => Equals(obj as IGameSymbol);

    public override int GetHashCode() => Color.GetHashCode();

    public override string ToString() => Display;
}
