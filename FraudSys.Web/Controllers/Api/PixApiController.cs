using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Web.Controllers.Api;

[ApiController]
[Route("api/pix")]
public class PixApiController : ControllerBase
{
    private readonly PixTransactionService _service;

    public PixApiController(PixTransactionService service)
    {
        _service = service;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] PixTransactionRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _service.ValidateAndConsumeAsync(request);
        return Ok(result);
    }
}