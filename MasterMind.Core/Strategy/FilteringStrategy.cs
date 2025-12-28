using MasterMind.Core.Models;

namespace MasterMind.Core.Strategy;

public class FilteringStrategy : IGameStrategy
{
    public string Name => "Filtrowanie (Knuth)";
    private List<Code> _possibleCodes = [];
    private Random _random = new();
    private const int SamplesToEvaluate = 100;

    public Code GenerateGuess(List<Attempt> previousAttempts, IGameSymbol[] availableSymbols, int codeLength)
    {
        if (previousAttempts.Count == 0)
        {
            _possibleCodes = GenerateAllPossibleCodes(availableSymbols, codeLength);
            return GetStartingGuess(availableSymbols, codeLength);
        }

        if (_possibleCodes.Count == 1)
            return _possibleCodes[0];

        if (_possibleCodes.Count == 0)
            return new Code(Enumerable.Repeat(availableSymbols[0], codeLength).ToArray());

        var bestGuess = _possibleCodes[0];
        int bestScore = int.MaxValue;

        var samplesToCheck = Math.Min(SamplesToEvaluate, _possibleCodes.Count);
        for (int i = 0; i < samplesToCheck; i++)
        {
            var candidate = _possibleCodes[_random.Next(_possibleCodes.Count)];
            var score = ScoreGuess(candidate);

            if (score < bestScore)
            {
                bestScore = score;
                bestGuess = candidate;
            }
        }

        return bestGuess;
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

    private int ScoreGuess(Code candidate)
    {
        var groupedByResult = new Dictionary<GameResult, int>();
        foreach (var possibleCode in _possibleCodes)
        {
            var result = Game.CalculateResultStatic(candidate, possibleCode);
            if (groupedByResult.ContainsKey(result))
                groupedByResult[result]++;
            else
                groupedByResult[result] = 1;
        }
        return groupedByResult.Count == 0 ? int.MaxValue : groupedByResult.Values.Max();
    }

    private static Code GetStartingGuess(IGameSymbol[] availableSymbols, int codeLength)
    {
        var symbols = new IGameSymbol[codeLength];
        for (int i = 0; i < codeLength / 2; i++)
        {
            symbols[i] = availableSymbols[0];
            symbols[codeLength / 2 + i] = availableSymbols[1];
        }
        return new Code(symbols);
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
