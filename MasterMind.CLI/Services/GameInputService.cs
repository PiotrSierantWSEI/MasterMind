using MasterMind.Core.Models;
using MasterMind.CLI;
using MasterMind.CLI.Models;

namespace MasterMind.CLI.Services;

public class GameInputService
{
    private readonly GameSettings _settings;

    public GameInputService(GameSettings settings)
    {
        _settings = settings;
    }

    public Code? ReadPlayerGuess()
    {
        // gra z cyframi czy z kolorami
        if (_settings.SymbolType == SymbolType.Digits)
        {
            return ReadDigitGuess();
        }
        else
        {
            return ReadColorGuess();
        }
    }

    private Code? ReadColorGuess()
    {
        while (true)
        {
            var validColors = GetValidColorCodes();
            Console.Write($"Wprowadź kod ({_settings.CodeLength} kolorów, dostępne: {validColors}, lub 'q' by poddać się): ");
            var input = Console.ReadLine()?.ToUpperInvariant() ?? "";

            if (input == "Q")
                return null;

            if (input.Length != _settings.CodeLength)
            {
                Console.WriteLine($"Kod musi zawierać dokładnie {_settings.CodeLength} znaki!");
                continue;
            }

            var symbols = new List<IGameSymbol>();
            bool valid = true;
            foreach (var ch in input)
            {
                if (GameConstants.ColorCharMap.TryGetValue(ch, out var color) && _settings.AvailableColors.Contains(color))
                {
                    symbols.Add(new ColorSymbol(color));
                }
                else
                {
                    Console.WriteLine($"'{ch}' to nieprawidłowy kolor!");
                    valid = false;
                    break;
                }
            }

            if (valid)
                return new Code(symbols.ToArray());
        }
    }

    private Code? ReadDigitGuess()
    {
        while (true)
        {
            Console.Write($"Wprowadź kod ({_settings.CodeLength} cyfr 0-9, lub 'q' by poddać się): ");
            var input = Console.ReadLine()?.ToUpperInvariant() ?? "";

            if (input == "Q")
                return null;

            if (input.Length != _settings.CodeLength)
            {
                Console.WriteLine($"Kod musi zawierać dokładnie {_settings.CodeLength} cyfr!");
                continue;
            }

            var symbols = new List<IGameSymbol>();
            bool valid = true;
            foreach (var ch in input)
            {
                var digit = DigitSymbol.TryParse(ch.ToString());
                if (digit != null && digit.Digit <= 9)
                {
                    symbols.Add(digit);
                }
                else
                {
                    Console.WriteLine($"'{ch}' to nie jest cyfra (0-9)!");
                    valid = false;
                    break;
                }
            }

            if (valid)
                return new Code(symbols.ToArray());
        }
    }

    public int GetStrategyChoice()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out var choice) && (choice == 1 || choice == 2))
            {
                Console.WriteLine(choice);
                return choice;
            }
            Console.WriteLine("\nNieprawidłowy wybór! Wybierz 1 lub 2:");
            Console.Write("Wybór (1-2): ");
        }
    }

    private string GetValidColorCodes()
    {
        var codes = string.Join("", _settings.AvailableColors.Select(c => GameConstants.ColorCharMap.FirstOrDefault(x => x.Value == c).Key));
        return codes;
    }

    public static GameResult ReadGameResult(int codeLength)
    {
        while (true)
        {
            Console.Write("Ile dokładnych trafień (exact matches)? ");
            if (!int.TryParse(Console.ReadLine(), out int exactMatches) || exactMatches < 0 || exactMatches > codeLength)
            {
                Console.WriteLine($"Wpisz liczbę od 0 do {codeLength}");
                continue;
            }

            Console.Write("Ile trafień na złej pozycji (wrong position)? ");
            if (!int.TryParse(Console.ReadLine(), out int wrongPosition) || wrongPosition < 0 || wrongPosition > codeLength)
            {
                Console.WriteLine($"Wpisz liczbę od 0 do {codeLength}");
                continue;
            }

            // exact + wrong nie mogą przekroczyć długości kodu
            if (exactMatches + wrongPosition > codeLength)
            {
                Console.WriteLine($"Suma nie może przekroczyć {codeLength}! Spróbuj ponownie.\n");
                continue;
            }

            return new GameResult(exactMatches, wrongPosition);
        }
    }
}
