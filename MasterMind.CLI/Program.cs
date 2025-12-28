using MasterMind.CLI.Models;
using MasterMind.CLI.Services;
using MasterMind.CLI.UI;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var menuService = new MenuService();

while (true)
{
    var gameMode = MenuService.DisplayMainMenuAndGetChoice();

    if (gameMode == GameMode.Exit)
        Environment.Exit(0);

    //Console.Clear();
    var symbolType = menuService.DisplaySymbolTypeMenu();
    //Console.Clear();
    var variant = menuService.DisplayVariantMenu();
    //Console.Clear();

    // Jeśli gracz wybrał tryb z oszustwami, pytamy o ile ich dozwolić
    bool askForCheats = variant == GameVariant.AllowedCheating;
    var gameSettings = menuService.GetGameSettings(variant, symbolType: symbolType, askForCheats: askForCheats);

    if (gameMode == GameMode.PlayerGuessing)
    {
        var playerService = new PlayerGuessingGameService(gameSettings);
        playerService.Play();
    }
    else if (gameMode == GameMode.ComputerGuessing)
    {
        var computerService = new ComputerGuessingGameService(gameSettings);
        computerService.Play();
    }
}
