using Gozon.Orders.Contracts;
using Gozon.Orders.Data;
using Gozon.Orders.Domain;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gozon.Orders.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _db;
    private readonly IPublishEndpoint _publish;

    public OrdersController(OrdersDbContext db, IPublishEndpoint publish)
    {
        _db = db;
        _publish = publish;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Amount = dto.Amount,
            Description = dto.Description,
            Status = "NEW"
        };

        _db.Orders.Add(order);

        // Кидаем событие в Outbox. Оно не уйдет в сеть, пока транзакция не закоммитится.
        await _publish.Publish(new OrderCreatedEvent
        {
            OrderId = order.Id,
            UserId = order.UserId,
            Amount = order.Amount
        });

        await _db.SaveChangesAsync();
        return Ok(order);
    }

    [HttpGet]
    public IActionResult GetList([FromQuery] Guid userId)
    {
        return Ok(_db.Orders.Where(o => o.UserId == userId).ToList());
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        var order = _db.Orders.Find(id);
        return order != null ? Ok(order) : NotFound();
    }
}

public record CreateOrderDto(Guid UserId, decimal Amount, string Description);