using MasterMind.Core.Models;

namespace MasterMind.CLI.Models;

public class GameSettings
{
    public Color[] AvailableColors { get; set; }
    public int CodeLength { get; set; }
    public int MaxAttempts { get; set; }
    public GameVariant Variant { get; set; }
    public SymbolType SymbolType { get; set; }
    public int AllowedCheats { get; set; }
    public int DigitCount { get; set; } = 10;

    public IGameSymbol[] Symbols
    {
        get
        {
            return SymbolType == SymbolType.Digits
                ? [.. Enumerable.Range(0, DigitCount).Select(i => (IGameSymbol)new DigitSymbol(i))]
                : [.. AvailableColors.Select(c => (IGameSymbol)new ColorSymbol(c))];
        }
    }

    public GameSettings(Color[] availableColors, int codeLength, int maxAttempts, GameVariant variant = GameVariant.Standard, SymbolType symbolType = SymbolType.Colors, int allowedCheats = 2, int digitCount = 10)
    {
        if (availableColors == null || availableColors.Length < 2)
            throw new ArgumentException("Muszą być co najmniej 2 kolory", nameof(availableColors));
        if (codeLength < 1)
            throw new ArgumentException("Długość kodu musi być >= 1", nameof(codeLength));
        if (maxAttempts < 1)
            throw new ArgumentException("Liczba prób musi być >= 1", nameof(maxAttempts));

        AvailableColors = availableColors;
        CodeLength = codeLength;
        MaxAttempts = maxAttempts;
        Variant = variant;
        SymbolType = symbolType;
        AllowedCheats = allowedCheats;
        DigitCount = digitCount;
    }

    public static GameSettings CreateDefault()
    {
        return new GameSettings(
            GameConstants.DefaultColors,
            GameConstants.DefaultCodeLength,
            GameConstants.DefaultMaxAttempts
        );
    }

    public static GameSettings CreateDigitVariant()
    {
        return new GameSettings(
            GameConstants.DefaultColors, // Będzie ignorowany dla SymbolType.Digits
            codeLength: 4,
            maxAttempts: 12,
            variant: GameVariant.Standard,
            symbolType: SymbolType.Digits,
            digitCount: 10
        );
    }
}
