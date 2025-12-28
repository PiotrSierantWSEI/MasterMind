namespace MasterMind.Core.Models;

/// <summary>
/// Abstrakcja dla symboli używanych w grze (kolory, cyfry, itp.)
/// Pozwala na elastyczne rozszerzanie gry o nowe typy symboli
/// </summary>
public interface IGameSymbol
{
    /// <summary>
    /// Tekstowa reprezentacja symbolu (np. "Red", "0", "1", itd.)
    /// </summary>
    string Display { get; }

    /// <summary>
    /// Zwraca typ symbolu (Color, Digit, itp.)
    /// </summary>
    string SymbolType { get; }

    /// <summary>
    /// Porównuje dwa symbole
    /// </summary>
    bool Equals(IGameSymbol? other);

    /// <summary>
    /// Zwraca unikalny hash symbolu
    /// </summary>
    int GetHashCode();
}
