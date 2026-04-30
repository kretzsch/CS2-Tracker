using Microsoft.AspNetCore.Mvc;
using Cs2Tracker.Services;

namespace Cs2Tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseController : ControllerBase
{
    private readonly SteamService _steam;

    public CaseController(SteamService steam)
    {
        _steam = steam;
    }

    [HttpGet("price")]
    public async Task<IActionResult> GetPrice(string name)
    {
        var price = await _steam.GetPriceAsync(name);

        return Ok(new
        {
            @case = name,
            price
        });
    }
}
