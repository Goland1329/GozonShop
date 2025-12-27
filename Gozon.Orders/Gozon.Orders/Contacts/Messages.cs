
namespace Gozon.Orders.Contracts;

// Держим контракты прямо здесь, чтобы не тащить лишние зависимости.
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