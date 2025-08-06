using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s27400_APBD_Project.IncomePart.Services;

namespace s27400_APBD_Project.IncomePart.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncomeController : ControllerBase
{
    private readonly IIncomeService _incomeService;

    public IncomeController(IIncomeService incomeService)
    {
        _incomeService = incomeService;
    }

    [HttpGet("realIncome/company/{currencyCode}")]
    public async Task<IActionResult> GetRealIncome(string currencyCode, CancellationToken token)
    {
        return Ok(await _incomeService.GetIncomeCompany(currencyCode, token));
    }

    [HttpGet("estimatedIncome/company/{currencyCode}")]
    public async Task<IActionResult> GetEstimatedIncome(string currencyCode, CancellationToken token)
    {
        return Ok(await _incomeService.GetEstimatedIncomeCompany(currencyCode, token));
    }

    [HttpGet("realIncome/product/{prodcutId}/{currencyCode}")]
    public async Task<IActionResult> GetRealProductIncome(string currencyCode, int prodcutId, CancellationToken token)
    {
        return Ok(await _incomeService.GetIncomeProduct(currencyCode, prodcutId, token));
    }

    [HttpGet("estimatedIncome/product/{prodcutId}/{currencyCode}")]
    public async Task<IActionResult> GetEstimatedProductIncome(string currencyCode, int prodcutId,
        CancellationToken token)
    {
        return Ok(await _incomeService.GetEstimatedIncomeProduct(currencyCode, prodcutId, token));
    }
}