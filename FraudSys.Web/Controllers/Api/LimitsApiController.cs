using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Web.Controllers.Api;

[ApiController]
[Route("api/limits")]
public class LimitsApiController : ControllerBase
{
    private readonly LimitService _service;

    public LimitsApiController(LimitService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLimitRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _service.CreateAsync(request);
            return CreatedAtAction(nameof(Get), new { agency = request.Agency, accountNumber = request.AccountNumber }, null);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{agency}/{accountNumber}")]
    public async Task<IActionResult> Get(string agency, string accountNumber)
    {
        try
        {
            var account = await _service.GetAsync(agency, accountNumber);
            return Ok(account);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{agency}/{accountNumber}")]
    public async Task<IActionResult> Update(string agency, string accountNumber, [FromBody] UpdateLimitRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            await _service.UpdateAsync(agency, accountNumber, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{agency}/{accountNumber}")]
    public async Task<IActionResult> Delete(string agency, string accountNumber)
    {
        try
        {
            await _service.DeleteAsync(agency, accountNumber);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}