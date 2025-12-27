using System.ComponentModel.DataAnnotations;

namespace Gozon.Payments.Domain;

public class Account
{
    [Key]
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }

    // Версия для защиты от гонки потоков
    [Timestamp]
    public uint Version { get; set; }
}