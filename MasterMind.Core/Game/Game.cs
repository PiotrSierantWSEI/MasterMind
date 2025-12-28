namespace MasterMind.Core;

using Models;

public class Game
{
    private readonly Code _secretCode;
    private readonly List<Attempt> _attempts;
    private GameState _state;
    private readonly int _maxAttempts;
    private readonly int _codeLength;

    public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();
    public GameState State => _state;
    public int MaxAttempts => _maxAttempts;
    public int CurrentAttempt => _attempts.Count + 1;
    public int RemainingAttempts => Math.Max(0, _maxAttempts - _attempts.Count);
    public int CodeLength => _codeLength;

    public Game(Code secretCode, int maxAttempts = 12, int codeLength = 4)
    {
        if (maxAttempts < 1)
            throw new ArgumentException("Maksymalna liczba prób musi być >= 1", nameof(maxAttempts));
        if (codeLength < 1)
            throw new ArgumentException("Długość kodu musi być >= 1", nameof(codeLength));

        _secretCode = secretCode ?? throw new ArgumentNullException(nameof(secretCode));
        _maxAttempts = maxAttempts;
        _codeLength = codeLength;
        _attempts = [];
        _state = GameState.InProgress;
    }

    public GameResult EvaluateAttempt(Code proposal)
    {
        if (proposal == null)
            throw new ArgumentNullException(nameof(proposal));
        if (_state != GameState.InProgress)
            throw new InvalidOperationException($"Gra już się zakończyła. Stan: {_state}");
        if (proposal.Sequence.Count != _secretCode.Sequence.Count)
            throw new ArgumentException(
                $"Kod musi zawierać {_secretCode.Sequence.Count} kolorów",
                nameof(proposal));

        var result = CalculateResult(proposal, _secretCode);
        var attempt = new Attempt(CurrentAttempt, proposal, result);
        _attempts.Add(attempt);

        if (IsWin(result))
            _state = GameState.Won;
        else if (_attempts.Count >= _maxAttempts)
            _state = GameState.Lost;

        return result;
    }

    public Code GetSecretCode()
    {
        if (_state == GameState.InProgress)
            throw new InvalidOperationException("Nie można ujawnić sekretnego kodu podczas rozgrywki");
        return _secretCode;
    }

    public void Surrender()
    {
        if (_state != GameState.InProgress)
            throw new InvalidOperationException("Gra już się zakończyła");
        _state = GameState.Surrendered;
    }

    public void Reset()
    {
        _attempts.Clear();
        _state = GameState.InProgress;
    }

    private static GameResult CalculateResult(Code proposal, Code secret)
    {
        var proposalList = proposal.Sequence.ToList();
        var secretList = secret.Sequence.ToList();
        int exactMatches = 0;
        int wrongPositionMatches = 0;

        var proposalRemaining = new List<IGameSymbol>();
        var secretRemaining = new List<IGameSymbol>();

        for (int i = 0; i < proposalList.Count; i++)
        {
            if (proposalList[i].Equals(secretList[i]))
            {
                exactMatches++;
            }
            else
            {
                proposalRemaining.Add(proposalList[i]);
                secretRemaining.Add(secretList[i]);
            }
        }

        foreach (var symbol in proposalRemaining)
        {
            var found = secretRemaining.FirstOrDefault(s => s.Equals(symbol));
            if (found != null)
            {
                wrongPositionMatches++;
                secretRemaining.Remove(found);
            }
        }

        return new GameResult(exactMatches, wrongPositionMatches);
    }

    public static GameResult CalculateResultStatic(Code proposal, Code secret) => CalculateResult(proposal, secret);

    private bool IsWin(GameResult result) => result.ExactMatches == _codeLength;
}

public enum GameState
{
    InProgress, Won, Lost, Surrendered
}
