using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Web.Controllers;

public class PixTransactionController : Controller
{
    private readonly PixTransactionService _service;
    public PixTransactionController(PixTransactionService service) => _service = service;

    public IActionResult Validate() => View();

    [HttpPost]
    public async Task<IActionResult> Validate(PixTransactionRequest request)
    {
        if (!ModelState.IsValid) return View(request);
        var result = await _service.ValidateAndConsumeAsync(request);
        ViewBag.Result = result;
        return View(request);
    }
}