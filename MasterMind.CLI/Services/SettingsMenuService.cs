using MasterMind.Core.Models;
using MasterMind.CLI.Models;

namespace MasterMind.CLI.Services;

public class SettingsMenuService
{
    private const int MinColors = 2;
    private const int MaxColors = 6;
    private const int MinCodeLength = 3;
    private const int MaxCodeLength = 5;
    private const int MinAttempts = 8;
    private const int MaxAttempts = 15;

    public GameSettings GetGameSettings(GameVariant? variant = null, SymbolType? symbolType = null, bool askForCheats = false)
    {
        UIHelpers.PrintHeader("Konfiguracja Gry");
        Console.WriteLine();

        Color[] colors;
        int codeLength;
        int maxAttempts;

        var finalSymbolType = symbolType ?? SymbolType.Colors;
        var finalVariant = variant ?? GameVariant.Standard;

        if (finalSymbolType == SymbolType.Digits)
        {
            Console.WriteLine("🔢 Gra z cyframi (0-9)\n");
            colors = GameConstants.DefaultColors;
            codeLength = GetCodeLength();
            maxAttempts = GetMaxAttempts();
        }
        else // gra z kolorami
        {
            var colorCount = GetColorCount();
            colors = SelectColors(colorCount);
            codeLength = GetCodeLength();
            maxAttempts = GetMaxAttempts();
        }

        int allowedCheats = 0;
        if (askForCheats && finalVariant == GameVariant.AllowedCheating)
        {
            allowedCheats = GetAllowedCheats();
        }

        Console.WriteLine();
        Console.WriteLine("╔════════════════════════════════════╗");

        if (finalSymbolType == SymbolType.Digits)
        {
            Console.WriteLine($"║ Typ: Cyfry (0-9)");
        }
        else
        {
            Console.WriteLine($"║ Kolory: {string.Join(",", colors.Select(c => c.ToString()[0]))}");
        }

        Console.WriteLine($"║ Długość kodu: {codeLength}");
        Console.WriteLine($"║ Liczba prób: {maxAttempts}");

        if (finalVariant == GameVariant.AllowedCheating)
        {
            Console.WriteLine($"║ Dozwolone oszustwa: {allowedCheats}");
        }
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.WriteLine();

        return new GameSettings(colors, codeLength, maxAttempts, finalVariant, finalSymbolType, allowedCheats);
    }

    private int GetColorCount()
    {
        while (true)
        {
            Console.WriteLine($"Wybierz liczbę kolorów ({MinColors}-{MaxColors}):");
            Console.WriteLine("Dostępne kolory: R(ed), Y(ellow), G(reen), B(lue), M(agenta), C(yan)");
            Console.Write($"Liczba kolorów ({MinColors}-{MaxColors}): ");

            if (int.TryParse(Console.ReadLine(), out int count) && count >= MinColors && count <= MaxColors)
            {
                return count;
            }

            Console.WriteLine($"Nieprawidłowy wybór! Wpisz liczbę od {MinColors} do {MaxColors}.\n");
        }
    }

    private Color[] SelectColors(int count)
    {
        Console.WriteLine($"\nWybierz {count} kolorów (wpisz {count} kodów bez spacji, np. {new string('R', Math.Min(count, 3))}...):");
        Console.WriteLine("Dostępne: R(ed), Y(ellow), G(reen), B(lue), M(agenta), C(yan)");

        while (true)
        {
            Console.Write($"Wpisz {count} kolorów: ");
            var input = Console.ReadLine()?.ToUpperInvariant() ?? "";

            if (input.Length != count)
            {
                Console.WriteLine($"Wpisz dokładnie {count} znaki! (wpisałeś {input.Length})");
                continue;
            }

            var selectedColors = new HashSet<Color>();
            bool valid = true;

            foreach (var ch in input)
            {
                if (GameConstants.ColorCharMap.TryGetValue(ch, out var color))
                {
                    if (!selectedColors.Add(color))
                    {
                        Console.WriteLine($"Kolor {color} powtórzony!");
                        valid = false;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"'{ch}' to nieprawidłowy kolor!");
                    valid = false;
                    break;
                }
            }

            if (valid && selectedColors.Count == count)
            {
                Console.WriteLine($"Wybrane kolory: {string.Join(", ", selectedColors)}");
                return [.. selectedColors];
            }
        }
    }

    private int GetCodeLength()
    {
        while (true)
        {
            Console.Write($"\nDługość kodu ({MinCodeLength}-{MaxCodeLength}): ");

            if (int.TryParse(Console.ReadLine(), out int length) && length >= MinCodeLength && length <= MaxCodeLength)
            {
                return length;
            }

            Console.WriteLine($"Nieprawidłowy wybór! Wpisz liczbę od {MinCodeLength} do {MaxCodeLength}.");
        }
    }

    private int GetMaxAttempts(int minAttempts = MinAttempts)
    {
        int maxAttemptsLimit = MaxAttempts;
        while (true)
        {
            Console.Write($"Liczba prób ({minAttempts}-{maxAttemptsLimit}): ");

            if (int.TryParse(Console.ReadLine(), out int attempts) && attempts >= minAttempts && attempts <= maxAttemptsLimit)
            {
                return attempts;
            }

            Console.WriteLine($"Nieprawidłowy wybór! Wpisz liczbę od {minAttempts} do {maxAttemptsLimit}.");
        }
    }

    private int GetAllowedCheats()
    {
        while (true)
        {
            Console.Write("\nIle niespójności gracz może dać w odpowiedziach (0-3)? ");

            if (int.TryParse(Console.ReadLine(), out int cheats) && cheats >= 0 && cheats <= 3)
            {
                return cheats;
            }

            Console.WriteLine("Nieprawidłowy wybór! Wpisz liczbę od 0 do 3.");
        }
    }
}
