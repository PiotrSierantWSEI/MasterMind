using MasterMind.Core.Models;

namespace MasterMind.Core.Strategy;

public interface IGameStrategy
{
    string Name { get; }
    Code GenerateGuess(List<Attempt> previousAttempts, IGameSymbol[] availableSymbols, int codeLength);
    void UpdateFeedback(Code guess, GameResult result);
    void Reset();
}

