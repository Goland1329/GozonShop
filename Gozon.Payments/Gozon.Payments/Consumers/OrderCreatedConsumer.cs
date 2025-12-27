using Gozon.Orders.Contracts;
using Gozon.Payments.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Gozon.Payments.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly PaymentsDbContext _db;

    public OrderCreatedConsumer(PaymentsDbContext db)
    {
        _db = db;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        // Inbox сам проверит дубликаты
        var msg = context.Message;
        var account = await _db.Accounts.FindAsync(msg.UserId);

        if (account == null || account.Balance < msg.Amount)
        {
            await context.Publish(new PaymentFailedEvent { OrderId = msg.OrderId });
            return;
        }

        account.Balance -= msg.Amount;

        try
        {
            await context.Publish(new PaymentSucceededEvent { OrderId = msg.OrderId });
            // Если кто-то успел изменить баланс параллельно, тут упадет ошибка
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw; // Повторим попытку
        }
    }
}