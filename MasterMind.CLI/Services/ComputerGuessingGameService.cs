using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using MasterMind.CLI;
using MasterMind.CLI.Models;
using MasterMind.CLI.UI;
using System.Collections.Generic;

namespace MasterMind.CLI.Services;

public class ComputerGuessingGameService
{
    private readonly GameSettings _settings;
    private readonly GameInputService _inputService;
    private readonly GameDisplayService _displayService;
    private List<(Code guess, GameResult result)> _attemptHistory = [];
    private bool _cheatLimitReached = false;

    public ComputerGuessingGameService(GameSettings settings)
    {
        _settings = settings;
        _inputService = new GameInputService(settings);
        _displayService = new GameDisplayService(settings);
    }

    public void Play()
    {
        _displayService.DisplayComputerGameStart(_settings.Variant == GameVariant.AllowedCheating
            ? "ComputerGuessingGame (Z oszustwami)"
            : "ComputerGuessingGame (Standard)");

        var strategyChoice = new MenuService().DisplayStrategyMenu();
        IGameStrategy strategy = strategyChoice == 2
            ? new FilteringStrategy()
            : new BruteForceStrategy();

        IGameSymbol[] aiSymbols = _settings.SymbolType == SymbolType.Digits
            ? GetDigitSymbolsForStrategy()
            : GetColorSymbolsForStrategy();

        var computerGame = new ComputerGuessingGame(
            strategy,
            aiSymbols,
            codeLength: _settings.CodeLength,
            maxAttempts: _settings.MaxAttempts,
            allowedCheats: _settings.AllowedCheats
        );

        var playerSecretCode = _inputService.ReadPlayerGuess();
        if (playerSecretCode == null)
        {
            Console.WriteLine("Anulowano grę.");
            return;
        }

        UIHelpers.PrintHeader($"Strategia: {strategy.Name}\n");
        Console.WriteLine("Komputer będzie odgadywać Twój kod...");
        if (_settings.Variant == GameVariant.AllowedCheating)
        {
            Console.WriteLine($"(Możesz dać maksymalnie {_settings.AllowedCheats} niespójne odpowiedzi)");
        }
        Console.WriteLine();
        _attemptHistory.Clear();

        int attemptCount = 0;
        while (attemptCount < _settings.MaxAttempts && computerGame.State == GameState.InProgress)
        {
            attemptCount++;

            Code computerGuess;
            try
            {
                computerGuess = computerGame.GetComputerGuess();
            }
            catch (InvalidOperationException)
            {
                // Gra się skończyła, przerywamy pętlę
                break;
            }

            _displayService.DisplayComputerGuess(
                computerGame.CurrentAttempt,
                computerGame.MaxAttempts,
                computerGuess,
                null  // Nie wyświetlamy wyniku - poczekamy na odpowiedź gracza
            );

            // Jeśli gracz osiągnął limit oszustw - gra automatyczna
            GameResult result;
            if (_cheatLimitReached && _settings.Variant == GameVariant.AllowedCheating)
            {
                result = Game.CalculateResultStatic(computerGuess, playerSecretCode);
                Console.WriteLine($"Dokładne trafienia: {result.ExactMatches}, Błędne pozycje: {result.WrongPositionMatches}");
            }
            else
            {
                Console.WriteLine("Wpisz Twój wynik na to zgadywanie:");
                result = GameInputService.ReadGameResult(_settings.CodeLength);
            }

            _attemptHistory.Add((computerGuess, result));
            computerGame.ProvideComputerFeedback(computerGuess, result);

            if (_settings.Variant == GameVariant.AllowedCheating)
            {
                int detectedCheats = DetectInconsistencies(playerSecretCode);
                if (detectedCheats > 0)
                {
                    Console.WriteLine($"[DEBUG] Niespójności: {detectedCheats}/{_settings.AllowedCheats}");
                }

                if (!_cheatLimitReached && detectedCheats == _settings.AllowedCheats)
                {
                    _cheatLimitReached = true;
                    Console.WriteLine($"\nUWAGA: Wykorzystałeś wszystkie {_settings.AllowedCheats} możliwości oszustwa!");
                }
            }
        }

        int finalCheatsDetected = DetectInconsistencies(playerSecretCode);

        if (_settings.Variant == GameVariant.AllowedCheating && finalCheatsDetected > 0)
        {
            Console.WriteLine($"\nWykryłem {finalCheatsDetected} niespójności w Twoich odpowiedziach\n");
        }
        else if (_settings.Variant == GameVariant.Standard && finalCheatsDetected > 0)
        {
            UIHelpers.PrintHeader("\nBŁĄD W ODPOWIEDZIACH");
            Console.WriteLine($"\nTwoje odpowiedzi były sprzeczne z rzeczywistością!");
            Console.WriteLine($"Liczba sprzeczności: {finalCheatsDetected}");
            Console.WriteLine($"Sekretny kod: {UIHelpers.ColorizeCode(playerSecretCode)}");
            Console.WriteLine("\nGra zakończona. Wybierz tryb z menu głównego.");
            Console.WriteLine("\n===============================\n");
            return;
        }

        _displayService.DisplayComputerGameEnd(computerGame, playerSecretCode);
    }

    /// <summary>
    /// Sprawdza ile razy gracz kłamie
    /// </summary>
    private int DetectInconsistencies(Code playerSecretCode)
    {
        int inconsistencies = 0;
        foreach (var (guess, givenResult) in _attemptHistory)
        {
            var expectedResult = Game.CalculateResultStatic(guess, playerSecretCode);
            if (!expectedResult.Equals(givenResult))
            {
                inconsistencies++;
            }
        }
        return inconsistencies;
    }

    private static IGameSymbol[] GetColorSymbolsForStrategy()
    {
        return
        [
            new ColorSymbol(Color.Red),
            new ColorSymbol(Color.Yellow),
            new ColorSymbol(Color.Green),
            new ColorSymbol(Color.Blue),
            new ColorSymbol(Color.Magenta),
            new ColorSymbol(Color.Cyan)
        ];
    }

    private static IGameSymbol[] GetDigitSymbolsForStrategy()
    {
        return [.. Enumerable.Range(0, 10).Select(i => (IGameSymbol)new DigitSymbol(i))];
    }
}

