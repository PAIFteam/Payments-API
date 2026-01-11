namespace Payments.API.Exceptions;

public class PaymentNotFoundException : Exception
{
    public PaymentNotFoundException() : base("Payment not found.")
    {
        
    }
}
