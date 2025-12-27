using Gozon.Payments.Data;
using Gozon.Payments.Domain;
using Microsoft.AspNetCore.Mvc;


namespace Gozon.Payments.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentsDbContext _db;
    public PaymentsController(PaymentsDbContext db) { _db = db; }

    [HttpPost("account")]
    public async Task<IActionResult> Create([FromBody] Guid userId)
    {
        if (_db.Accounts.Any(a => a.UserId == userId)) return BadRequest();
        var acc = new Account { UserId = userId, Balance = 0 };
        _db.Accounts.Add(acc);
        await _db.SaveChangesAsync();
        return Ok(acc);
    }

    [HttpPost("topup")]
    public async Task<IActionResult> TopUp([FromBody] TopUpDto dto)
    {
        var acc = await _db.Accounts.FindAsync(dto.UserId);
        if (acc == null) return NotFound();
        acc.Balance += dto.Amount;
        await _db.SaveChangesAsync();
        return Ok(acc);
    }

    [HttpGet("balance/{userId}")]
    public async Task<IActionResult> GetBalance(Guid userId)
    {
        var acc = await _db.Accounts.FindAsync(userId);
        return acc != null ? Ok(new { acc.Balance }) : NotFound();
    }
}
public record TopUpDto(Guid UserId, decimal Amount);