using MasterMind.Core;
using MasterMind.Core.Models;
using MasterMind.Core.Strategy;
using MasterMind.CLI.Models;

namespace MasterMind.CLI.UI;

public class GameDisplayService
{
    private readonly GameSettings _settings;

    public GameDisplayService(GameSettings settings)
    {
        _settings = settings;
    }

    public void DisplayGameStart(string title)
    {
        UIHelpers.PrintHeader(title);
        Console.WriteLine();

        if (_settings.SymbolType == SymbolType.Digits)
        {
            Console.WriteLine("Dostępne cyfry: 0-9");
        }
        else
        {
            UIHelpers.PrintColorLegend(_settings.AvailableColors);
        }

        string symbolType = _settings.SymbolType == SymbolType.Digits ? "cyfr" : "kolorów";
        Console.WriteLine($"Kod zawiera {_settings.CodeLength} {symbolType}, masz {_settings.MaxAttempts} prób.");
        Console.WriteLine();
    }

    public void DisplayAttemptResult(Code guess, GameResult result)
    {
        Console.WriteLine($"Twój kod:     {UIHelpers.ColorizeCode(guess)}");
        bool isWin = result.ExactMatches == _settings.CodeLength;
        Console.WriteLine($"Wynik:        {result.ExactMatches} dokładne ◆, {result.WrongPositionMatches} źle umieszczone ◇{(isWin ? "\n" : "")}");
        if (isWin)
        {
            Console.WriteLine("WYGRANA!\n");
        }
    }

    public void DisplayGameEnd(Game game)
    {
        if (game.State == GameState.Won)
        {
            UIHelpers.PrintHeaderWithSubtitle("GRATULACJE!", $"Wygrałeś w {game.Attempts.Count} próbach");
        }
        else if (game.State == GameState.Lost)
        {
            UIHelpers.PrintHeader("Przegrałeś... Koniec gry");
            Console.WriteLine($"Sekretny kod to: {UIHelpers.ColorizeCode(game.GetSecretCode())}");
        }
        else if (game.State == GameState.Surrendered)
        {
            UIHelpers.PrintHeader("Poddałeś się...");
            Console.WriteLine($"Sekretny kod to: {UIHelpers.ColorizeCode(game.GetSecretCode())}");
        }
        Console.WriteLine("\nNaciśnij dowolny klawisz aby wrócić do menu...");
    }

    public void DisplayComputerGameStart(string strategyName)
    {
        UIHelpers.PrintHeader("Tworzenie Sekretnego Kodu");
        Console.WriteLine();

        if (_settings.SymbolType == SymbolType.Digits)
        {
            Console.WriteLine("Dostępne cyfry: 0-9");
        }
        else
        {
            UIHelpers.PrintColorLegend(_settings.AvailableColors);
        }

        Console.WriteLine();
    }

    public void DisplayComputerGuess(int attemptNumber, int maxAttempts, Code guess, GameResult? result)
    {
        Console.WriteLine($"Próba {attemptNumber}/{maxAttempts}:");
        Console.WriteLine($"Komputer zgaduje: {UIHelpers.ColorizeCode(guess)}");
        if (result != null)
        {
            Console.WriteLine($"Wynik: {result.ExactMatches} dokładne, {result.WrongPositionMatches} źle umieszczone");
        }
        Console.WriteLine();
    }

    public void DisplayComputerGameEnd(ComputerGuessingGame game, Code playerSecret)
    {
        if (game.CheckForCheating(playerSecret))
        {
            UIHelpers.PrintHeader("WYKRYTO OSZUKIWANIE!");
            Console.WriteLine("\nTwoje odpowiedzi są sprzeczne!");
            Console.WriteLine($"Liczba niespójności: {game.DetectedCheats}");
            Console.WriteLine($"Sekretny kod: {UIHelpers.ColorizeCode(playerSecret)}");
            return;
        }

        if (game.State == GameState.Won)
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   Komputer Cię odgadł!             ║");
            Console.WriteLine($"║     Po {game.Attempts.Count,2} próbach                  ║");
            Console.WriteLine("╚════════════════════════════════════╝");
        }
        else if (game.State == GameState.Lost)
        {
            UIHelpers.PrintHeader("Komputer się nie poddał!");
        }
        Console.WriteLine($"Twój sekretny kod: {UIHelpers.ColorizeCode(playerSecret)}");

        if (game.DetectedCheats > 0)
        {
            Console.WriteLine($"Wykryłem {game.DetectedCheats} niespójności w Twoich odpowiedziach");
        }

        Console.WriteLine("\nKoniec gry. Wybierz tryb z menu głównego.");
        Console.WriteLine("\n===============================\n");
    }
}
