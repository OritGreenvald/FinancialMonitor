using FinancialMonitor.Application.DTOs;
using FinancialMonitor.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinancialMonitor.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateTransactionRequest request)
    {
        var transaction = await _transactionService.CreateAsync(request);

        return Ok(transaction);
    }

    [HttpGet]
    public async Task<IActionResult> GetLatest(
    [FromQuery] int count = 10)
    {
        if (count <= 0 || count > 100)
        {
            return BadRequest("Count must be between 1 and 100.");
        }

        var transactions = await _transactionService.GetLatestAsync(count);

        return Ok(transactions);
    }
}
