using MasterMind.CLI;
using MasterMind.CLI.Models;

namespace MasterMind.CLI.Services;

public enum GameMode
{
    PlayerGuessing = 1,
    ComputerGuessing = 2,
    Exit = 3
}

public class MenuService
{
    private readonly SettingsMenuService _settingsMenu = new();

    public static GameMode DisplayMainMenuAndGetChoice()
    {
        UIHelpers.PrintHeader("MASTERMIND - Wybór Trybu");
        Console.WriteLine("1. Gracz vs Komputer (komputer losuje kod)");
        Console.WriteLine("2. Komputer vs Gracz (gracz tworzy kod)");
        Console.WriteLine("3. Wyjście");
        Console.Write("\nWybierz tryb (1-3): ");

        while (true)
        {
            try
            {
                var choice = Console.ReadKey(true);
                Console.WriteLine(choice.KeyChar);

                if (choice.KeyChar == '1')
                    return GameMode.PlayerGuessing;
                else if (choice.KeyChar == '2')
                    return GameMode.ComputerGuessing;
                else if (choice.KeyChar == '3')
                    return GameMode.Exit;
                else
                    Console.WriteLine("Nieprawidłowy wybór! Wybierz 1, 2 lub 3:");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Wejście niedostępne, wybieram tryb 1...");
                return GameMode.PlayerGuessing;
            }
        }
    }

    public GameSettings GetGameSettings(GameVariant? variant = null, SymbolType? symbolType = null, bool askForCheats = false)
    {
        var settings = _settingsMenu.GetGameSettings(variant, symbolType, askForCheats);
        Console.WriteLine("Naciśnij dowolny klawisz aby kontynuować...");
        Console.ReadKey(true);
        return settings;
    }

    public int DisplayStrategyMenu()
    {
        Console.WriteLine("Wybierz strategię komputera:");
        Console.WriteLine("1. Brute Force (szybki, losowy)");
        Console.WriteLine("2. Filtrowanie - Knuth (inteligentny)");
        Console.Write("\nWybór (1-2): ");

        return new GameInputService(GameSettings.CreateDefault()).GetStrategyChoice();
    }

    public SymbolType DisplaySymbolTypeMenu()
    {
        Console.WriteLine("\nWybierz typ symboli:");
        Console.WriteLine("1. Kolory");
        Console.WriteLine("2. Cyfry (0-9)");
        Console.Write("\nWybór (1-2): ");

        while (true)
        {
            try
            {
                var choice = Console.ReadKey(true);
                Console.WriteLine(choice.KeyChar);

                if (choice.KeyChar == '1')
                    return SymbolType.Colors;
                else if (choice.KeyChar == '2')
                    return SymbolType.Digits;
                else
                    Console.WriteLine("Nieprawidłowy wybór! Wybierz 1 lub 2:");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Wejście niedostępne, wybieram kolory...");
                return SymbolType.Colors;
            }
        }
    }

    public GameVariant DisplayVariantMenu()
    {
        Console.WriteLine("\nWybierz wariant gry:");
        Console.WriteLine("1. Standardowa (brak oszustw)");
        Console.WriteLine("2. Z tolerancją na oszustwa (gracz może dać niespójne odpowiedzi)");
        Console.Write("\nWybór (1-2): ");

        while (true)
        {
            try
            {
                var choice = Console.ReadKey(true);
                Console.WriteLine(choice.KeyChar);

                if (choice.KeyChar == '1')
                    return GameVariant.Standard;
                else if (choice.KeyChar == '2')
                    return GameVariant.AllowedCheating;
                else
                    Console.WriteLine("Nieprawidłowy wybór! Wybierz 1 lub 2:");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Wejście niedostępne, wybieram wariant Standard...");
                return GameVariant.Standard;
            }
        }
    }
}
