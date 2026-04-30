using Microsoft.AspNetCore.Mvc;
using Cs2Tracker.Api.Services;

namespace Cs2Tracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkinController : ControllerBase
{
    private readonly SkinService _service;

    public SkinController()
    {
        _service = new SkinService();
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_service.GetAll());
    }
}