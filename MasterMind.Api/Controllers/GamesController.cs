using Microsoft.AspNetCore.Mvc;
using MasterMind.Core.Models;

namespace MasterMind.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    // GET: api/games
    [HttpGet]
    public IActionResult GetAll() => Ok(new[] { new { id = "sample", description = "Placeholder - implement game listing" } });

    // GET: api/games/{id}
    [HttpGet("{id}")]
    public IActionResult Get(string id) => Ok(new { id, description = "Placeholder - implement game retrieval" });

    // POST: api/games/{id}/guess
    [HttpPost("{id}/guess")]
    public IActionResult Guess(string id, [FromBody] object? guess)
    {
        // Placeholder - implement guess evaluation using Core.Game
        return BadRequest(new { error = "Not implemented" });
    }
}
