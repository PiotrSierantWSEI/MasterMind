using MasterMind.Core.Models;

namespace MasterMind.Core.Strategy;

public class ComputerGuessingGame
{
    private readonly IGameStrategy _strategy;
    private readonly int _maxAttempts;
    private readonly int _codeLength;
    private readonly IGameSymbol[] _availableSymbols;
    private readonly List<Attempt> _attempts;
    private GameState _state;
    private Code? _playerSecretCode;
    private bool _cheatingDetected = false;
    private int _detectedCheats = 0;
    private int _allowedCheats = 0;

    public IReadOnlyList<Attempt> Attempts => _attempts.AsReadOnly();
    public GameState State => _state;
    public int MaxAttempts => _maxAttempts;
    public int CurrentAttempt => _attempts.Count + 1;
    public int RemainingAttempts => Math.Max(0, _maxAttempts - _attempts.Count);
    public bool CheatingDetected => _cheatingDetected;
    public int DetectedCheats => _detectedCheats;
    public IGameStrategy Strategy => _strategy;

    public ComputerGuessingGame(
        IGameStrategy strategy,
        IGameSymbol[] availableSymbols,
        int codeLength,
        int maxAttempts = 12,
        int allowedCheats = 0)
    {
        if (codeLength < 1)
            throw new ArgumentException("Długość kodu musi być >= 1", nameof(codeLength));
        if (maxAttempts < 1)
            throw new ArgumentException("Maksymalna liczba prób musi być >= 1", nameof(maxAttempts));

        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        _availableSymbols = availableSymbols ?? throw new ArgumentNullException(nameof(availableSymbols));
        _codeLength = codeLength;
        _maxAttempts = maxAttempts;
        _allowedCheats = allowedCheats;
        _attempts = new List<Attempt>();
        _state = GameState.InProgress;
    }

    public ComputerGuessingGame(
        IGameStrategy strategy,
        Color[] availableColors,
        int codeLength,
        int maxAttempts = 12,
        int allowedCheats = 0)
        : this(strategy, availableColors.Select(c => (IGameSymbol)new ColorSymbol(c)).ToArray(), codeLength, maxAttempts, allowedCheats)
    {
    }

    public Code GetComputerGuess()
    {
        if (_state != GameState.InProgress)
            throw new InvalidOperationException($"Gra już się zakończyła. Stan: {_state}");
        return _strategy.GenerateGuess(_attempts, _availableSymbols, _codeLength);
    }

    public void ProvideComputerFeedback(Code computerGuess, GameResult result)
    {
        if (_state != GameState.InProgress)
            throw new InvalidOperationException($"Gra już się zakończyła. Stan: {_state}");
        if (computerGuess == null)
            throw new ArgumentNullException(nameof(computerGuess));
        if (computerGuess.Sequence.Count != _codeLength)
            throw new ArgumentException($"Kod musi zawierać {_codeLength} kolorów", nameof(computerGuess));

        var attempt = new Attempt(CurrentAttempt, computerGuess, result);
        _attempts.Add(attempt);
        _strategy.UpdateFeedback(computerGuess, result);

        if (IsWin(result))
            _state = GameState.Won;
        else if (_attempts.Count >= _maxAttempts)
            _state = GameState.Lost;
    }

    public bool CheckForCheating(Code actualSecretCode)
    {
        if (actualSecretCode == null)
            return false;

        _playerSecretCode = actualSecretCode;
        _detectedCheats = 0;

        foreach (var attempt in _attempts)
        {
            var expectedResult = Game.CalculateResultStatic(attempt.Proposal, actualSecretCode);
            if (!expectedResult.Equals(attempt.Result))
            {
                _detectedCheats++;
            }
        }

        _cheatingDetected = _detectedCheats > _allowedCheats;
        return _cheatingDetected;
    }

    public Code? GetPlayerSecretCode() => _playerSecretCode;

    public void Reset()
    {
        _attempts.Clear();
        _state = GameState.InProgress;
        _strategy.Reset();
        _playerSecretCode = null;
        _cheatingDetected = false;
    }

    public void PlayerSurrenders()
    {
        if (_state != GameState.InProgress)
            throw new InvalidOperationException("Gra już się zakończyła");
        _state = GameState.Surrendered;
    }

    private bool IsWin(GameResult result) => result.ExactMatches == _codeLength;
}
