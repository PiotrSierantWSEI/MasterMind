namespace MasterMind.Core.Models;

public class Code
{
    public IReadOnlyList<IGameSymbol> Sequence { get; }

    public Code(params IGameSymbol[] symbols)
    {
        if (symbols == null || symbols.Length == 0)
            throw new ArgumentException("Kod musi zawierać co najmniej jeden symbol", nameof(symbols));
        Sequence = symbols.ToList().AsReadOnly();
    }

    public Code(IEnumerable<IGameSymbol> symbols)
    {
        var symbolList = symbols?.ToList() ?? throw new ArgumentNullException(nameof(symbols));
        if (symbolList.Count == 0)
            throw new ArgumentException("Kod musi zawierać co najmniej jeden symbol", nameof(symbols));
        Sequence = symbolList.AsReadOnly();
    }

    public Code(params Color[] colors)
        : this(colors.Select(c => (IGameSymbol)new ColorSymbol(c)).ToArray())
    {
    }

    public bool Equals(Code? other)
    {
        if (other == null) return false;
        return Sequence.SequenceEqual(other.Sequence, new GameSymbolComparer());
    }

    public override bool Equals(object? obj) => Equals(obj as Code);

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var symbol in Sequence)
                hash = hash * 31 + symbol.GetHashCode();
            return hash;
        }
    }

    private class GameSymbolComparer : IEqualityComparer<IGameSymbol>
    {
        public bool Equals(IGameSymbol? x, IGameSymbol? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public int GetHashCode(IGameSymbol obj) => obj?.GetHashCode() ?? 0;
    }
}
