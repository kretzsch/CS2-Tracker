using Microsoft.AspNetCore.Mvc;
using Cs2Tracker.Models;
using Cs2Tracker.Services;

namespace Cs2Tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseController : ControllerBase
{
    private readonly CaseService _caseService;
    private readonly SteamService _steamService;

    public CaseController(CaseService caseService, SteamService steamService)
    {
        _caseService = caseService;
        _steamService = steamService;
    }

    /// <summary>
    /// Get all cases from database (cached prices)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cases = await _caseService.GetAllAsync();
        return Ok(cases);
    }

    /// <summary>
    /// Get specific case by name from database
    /// </summary>
    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var caseItem = await _caseService.GetByNameAsync(name);

        if (caseItem == null)
            return NotFound(new { message = $"Case '{name}' not found in database" });

        return Ok(caseItem);
    }

    /// <summary>
    /// Get live price from Steam API (not cached)
    /// </summary>
    [HttpGet("live-price")]
    public async Task<IActionResult> GetLivePrice(string name)
    {
        var price = await _steamService.GetPriceAsync(name);

        if (price == null)
            return NotFound(new { message = $"Could not fetch price for '{name}' from Steam" });

        return Ok(new
        {
            @case = name,
            price = price.Value,
            source = "Steam API (live)",
            fetchedAt = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Manually refresh a specific case price from Steam API and save to database
    /// </summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshPrice([FromBody] RefreshRequest request)
    {
        var price = await _steamService.GetPriceAsync(request.Name);

        if (price == null)
            return BadRequest(new { message = $"Could not fetch price for '{request.Name}' from Steam" });

        await _caseService.AddOrUpdateAsync(new Case
        {
            Name = request.Name,
            Price = price.Value,
            UpdatedAt = DateTime.UtcNow
        });

        return Ok(new
        {
            message = "Price updated successfully",
            @case = request.Name,
            price = price.Value,
            updatedAt = DateTime.UtcNow
        });
    }
}

public class RefreshRequest
{
    public string Name { get; set; } = "";
}
