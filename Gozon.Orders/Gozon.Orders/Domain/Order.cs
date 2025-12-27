using System.ComponentModel.DataAnnotations;

namespace Gozon.Orders.Domain;

public class Order
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } // NEW, FINISHED, CANCELLED
}