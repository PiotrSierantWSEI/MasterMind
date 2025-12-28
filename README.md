# MasterMind

## Opis projektu

MasterMind to implementacja klasycznej gry Mastermind w C# (.NET 9). Projekt jest podzielony na moduły odpowiedzialne za logikę gry (Core), interfejs konsolowy (CLI) oraz testy jednostkowe.

## Wymagania

- .NET 9 SDK
- System: Windows / Linux / macOS
- Dowolne IDE (Visual Studio, VS Code) lub edytor wspierający .NET

## Budowa i uruchomienie

Zbuduj całe rozwiązanie:

```bash
dotnet build MasterMind.sln
```

Uruchom aplikację CLI (interaktywna gra):

```bash
dotnet run --project MasterMind.CLI
```

Uruchomienie testów:

```bash
dotnet test
```

## Konfiguracja gry

Ustawienia gry znajdują się w `MasterMind.CLI/Models/GameSettings.cs`. Najważniejsze opcje:

- `CodeLength` — długość sekretnego kodu
- `MaxAttempts` — maksymalna liczba prób
- `SymbolType` — `Colors` lub `Digits`
- `Variant` — np. `Standard`, `AllowedCheating`
- `AvailableColors` — lista dostępnych kolorów

CLI udostępnia menu do zmiany ustawień przy uruchomieniu.

## Struktura repozytorium

- `MasterMind.Core/` — logika gry, modele, strategie
- `MasterMind.CLI/` — interfejs konsolowy, serwisy wejścia/wyjścia
- `MasterMind.Tests/` — testy xUnit
