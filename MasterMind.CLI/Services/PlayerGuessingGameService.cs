using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.CLI.Models;
using MasterMind.CLI.UI;
using System.Linq;

namespace MasterMind.CLI.Services;

public class PlayerGuessingGameService
{
    private readonly GameSettings _settings;
    private readonly GameInputService _inputService;
    private readonly GameDisplayService _displayService;

    public PlayerGuessingGameService(GameSettings settings)
    {
        _settings = settings;
        _inputService = new GameInputService(settings);
        _displayService = new GameDisplayService(settings);
    }

    public void Play()
    {
        var secretCode = GenerateRandomCode(_settings.CodeLength);
        var game = new Game(secretCode, maxAttempts: _settings.MaxAttempts, codeLength: _settings.CodeLength);

        // switch expression - wyrażenie porównuje krotkę (_settings.SymbolType, _settings.Variant) z zestawem wzorców i zwraca odpowiadający łańcuch znaków. _ odpowiada za dopasowanie do dowolnej wartości.
        string gameType = (_settings.SymbolType, _settings.Variant) switch
        {
            (SymbolType.Digits, _) => "Gracz vs Komputer (Cyfry 0-9)",
            (_, GameVariant.AllowedCheating) => "Gracz vs Komputer (Z tolerancją na oszustwa)",
            _ => "Gracz vs Komputer (Standard)"
        };
        _displayService.DisplayGameStart(gameType);

        while (game.State == GameState.InProgress)
        {
            Console.WriteLine($"Próba {game.CurrentAttempt}/{game.MaxAttempts}:");
            var guess = _inputService.ReadPlayerGuess();

            if (guess == null)
            {
                game.Surrender();
                break;
            }

            var result = game.EvaluateAttempt(guess);
            _displayService.DisplayAttemptResult(guess, result);
        }

        _displayService.DisplayGameEnd(game);
    }

    private Code GenerateRandomCode(int length)
    {
        var random = new Random();

        if (_settings.SymbolType == SymbolType.Digits)
        {
            var symbols = Enumerable.Range(0, length)
                .Select(_ => (IGameSymbol)new DigitSymbol(random.Next(0, 10)))
                .ToArray();
            return new Code(symbols);
        }
        else
        {
            var colors = Enumerable.Range(0, length)
                .Select(_ => _settings.AvailableColors[random.Next(_settings.AvailableColors.Length)])
                .ToArray();
            return new Code(colors);
        }
    }
}
