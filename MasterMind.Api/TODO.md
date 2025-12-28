# TODO — MasterMind API (przygotowanie pod frontend)

TODO API do współpracy z frontendem.

---

## 1) Zaprojektować API i DTO

- Endpointy:
  - POST /api/games — utwórz nową grę (z ustawieniami: codeLength, symbolType, maxAttempts)
  - GET /api/games — lista istniejących (opcjonalnie z filtrowaniem/pagingiem)
  - GET /api/games/{id} — pobierz szczegóły gry (stan, próby, wynik)
  - POST /api/games/{id}/guesses — zgłoszenie zgadywania (tablica symboli)
  - GET /api/games/{id}/state — skrócony stan gry
- DTOs: GameDto, CreateGameDto, GuessDto, GuessResultDto, ErrorDto
- Kryteria akceptacji: OpenAPI (Swagger) z przykładami request/response dostępny i akceptowalny przez frontend

## 2) Implementacja zarządzania grami

- Zaimplementować serwis repozytorium in-memory
- POST /api/games tworzy grę i zwraca ID (201)
- GET zwraca poprawne DTO

## 3) Implementacja endpointu zgadywania

- POST /api/games/{id}/guesses wywołuje Game.EvaluateAttempt i zwraca wynik oraz aktualizuje stan gry
- Obsługa scenariuszy: gra nie istnieje (404), wejście niepoprawne (400), gra zakończona (409 lub 400 według decyzji)

## 4) Walidacja i obsługa błędów

- Walidacja formatów i długości kodu
- Spójny format błędu (tutaj chciałbym zrobić interface błędów dla całego API)

## 5) CORS i konfiguracja frontend origin

- Dodać konfigurowalne policy CORS (dev: http://localhost:3000)

---
