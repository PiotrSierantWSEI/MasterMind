namespace MasterMind.Core.Models;

/// <summary>
/// Adapter dla cyfr 0-9
/// Pozwala na grę z cyframi zamiast kolorów
/// </summary>
public class DigitSymbol : IGameSymbol
{
    private const int MinDigit = 0;
    private const int MaxDigit = 9;

    public int Digit { get; }

    public string Display => Digit.ToString();
    public string SymbolType => "Digit";

    public DigitSymbol(int digit)
    {
        if (digit < MinDigit || digit > MaxDigit)
            throw new ArgumentException($"Cyfra musi być w zakresie {MinDigit}-{MaxDigit}", nameof(digit));

        Digit = digit;
    }

    /// <summary>
    /// Parsuje string na DigitSymbol
    /// </summary>
    public static DigitSymbol? TryParse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (int.TryParse(input.Trim(), out int digit) && digit >= MinDigit && digit <= MaxDigit)
            return new DigitSymbol(digit);

        return null;
    }

    public bool Equals(IGameSymbol? other)
    {
        if (other is not DigitSymbol ds)
            return false;
        return Digit == ds.Digit;
    }

    public override bool Equals(object? obj) => Equals(obj as IGameSymbol);

    public override int GetHashCode() => Digit.GetHashCode();

    public override string ToString() => Display;
}
