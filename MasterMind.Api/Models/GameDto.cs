namespace MasterMind.Api.Models;

public record GameDto(string Id, int CodeLength, int MaxAttempts, string SymbolType);
