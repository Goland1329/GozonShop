
namespace Gozon.Orders.Contracts;

// Те же самые контракты, что и в заказах.
public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId { get; init; }
    public decimal Amount { get; init; }
}

public record PaymentSucceededEvent
{
    public Guid OrderId { get; init; }
}

public record PaymentFailedEvent
{
    public Guid OrderId { get; init; }
}