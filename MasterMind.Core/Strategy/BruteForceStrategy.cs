using MasterMind.Core.Models;
namespace MasterMind.Core.Strategy;

public class BruteForceStrategy : IGameStrategy
{
    public string Name => "Brute Force";
    private List<Code> _possibleCodes = [];
    private Random _random = new();

    public Code GenerateGuess(List<Attempt> previousAttempts, IGameSymbol[] availableSymbols, int codeLength)
    {
        if (previousAttempts.Count == 0)
            _possibleCodes = GenerateAllPossibleCodes(availableSymbols, codeLength);

        if (_possibleCodes.Count == 0)
            return new Code(Enumerable.Repeat(availableSymbols[0], codeLength).ToArray());

        return _possibleCodes[_random.Next(_possibleCodes.Count)];
    }

    public void UpdateFeedback(Code guess, GameResult result)
    {
        _possibleCodes = [.. _possibleCodes.Where(code => Game.CalculateResultStatic(guess, code).Equals(result))];
    }

    public void Reset()
    {
        _possibleCodes.Clear();
        _random = new Random();
    }

    private static List<Code> GenerateAllPossibleCodes(IGameSymbol[] availableSymbols, int codeLength)
    {
        var codes = new List<Code>();

        void Generate(List<IGameSymbol> current)
        {
            if (current.Count == codeLength)
            {
                codes.Add(new Code(current.ToArray()));
                return;
            }

            foreach (var symbol in availableSymbols)
            {
                current.Add(symbol);
                Generate(current);
                current.RemoveAt(current.Count - 1);
            }
        }

        Generate([]);
        return codes;
    }
}
