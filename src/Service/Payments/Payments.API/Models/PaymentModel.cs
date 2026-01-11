using Payments.API.Enums;

namespace Payments.API.Models;

public class PaymentModel
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PaidAt { get; set; }

    public PaymentModel()
    {
        CreatedAt = DateTime.UtcNow;
        PaymentStatus = PaymentStatus.Pending;
    }
}
