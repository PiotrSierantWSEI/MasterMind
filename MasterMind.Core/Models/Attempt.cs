namespace MasterMind.Core.Models;

public class Attempt
{
    public Code Proposal { get; }
    public GameResult Result { get; }
    public int AttemptNumber { get; }

    public Attempt(int attemptNumber, Code proposal, GameResult result)
    {
        if (attemptNumber < 1)
            throw new ArgumentException("Numer próby musi być >= 1", nameof(attemptNumber));
        AttemptNumber = attemptNumber;
        Proposal = proposal ?? throw new ArgumentNullException(nameof(proposal));
        Result = result;
    }
}
