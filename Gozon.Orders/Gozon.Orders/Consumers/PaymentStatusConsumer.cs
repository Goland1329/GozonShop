using Gozon.Orders.Contracts;
using Gozon.Orders.Data;
using MassTransit;

namespace Gozon.Orders.Consumers;

public class PaymentStatusConsumer :
    IConsumer<PaymentSucceededEvent>,
    IConsumer<PaymentFailedEvent>
{
    private readonly OrdersDbContext _db;

    public PaymentStatusConsumer(OrdersDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<PaymentSucceededEvent> context)
    {
        var order = await _db.Orders.FindAsync(context.Message.OrderId);
        if (order != null)
        {
            order.Status = "FINISHED"; // Все ок, оплачено
            await _db.SaveChangesAsync();
        }
    }

    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var order = await _db.Orders.FindAsync(context.Message.OrderId);
        if (order != null)
        {
            order.Status = "CANCELLED"; // Денег нет, отменяем
            await _db.SaveChangesAsync();
        }
    }
}