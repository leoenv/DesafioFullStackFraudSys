using FraudSys.Application.DTOs;
using FraudSys.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Web.Controllers;

public class LimitController : Controller
{
    private readonly LimitService _service;

    public LimitController(LimitService service)
    {
        _service = service;
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateLimitRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        try
        {
            await _service.CreateAsync(request);
            TempData["Success"] = "Limit cadastrado com sucesso!";
            return RedirectToAction(nameof(Details), new { agency = request.Agency, accountNumber = request.AccountNumber });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(request);
        }
    }

    public IActionResult Search() => View();

    public async Task<IActionResult> Details(string agency, string accountNumber)
    {
        try
        {
            var account = await _service.GetAsync(agency, accountNumber);
            return View(account);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    public async Task<IActionResult> Edit(string agency, string accountNumber)
    {
        var account = await _service.GetAsync(agency, accountNumber);
        return View(new UpdateLimitRequest { PixLimit = account.PixLimit });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(string agency, string accountNumber, UpdateLimitRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        await _service.UpdateAsync(agency, accountNumber, request);
        TempData["Success"] = "Limit atualizado com sucesso!";
        return RedirectToAction(nameof(Details), new { agency, accountNumber });
    }

    public async Task<IActionResult> Delete(string agency, string accountNumber)
    {
        var account = await _service.GetAsync(agency, accountNumber);
        return View(account);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(string agency, string accountNumber)
    {
        await _service.DeleteAsync(agency, accountNumber);
        TempData["Success"] = "Conta removida com sucesso!";
        return RedirectToAction("Index", "Home");
    }
}