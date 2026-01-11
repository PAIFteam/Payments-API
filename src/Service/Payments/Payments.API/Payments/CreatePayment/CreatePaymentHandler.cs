namespace Payments.API.Payments.CreatePayment;

public record CreatePaymentCommand(decimal Amount, PaymentMethod PaymentMethod, 
    PaymentStatus PaymentStatus, DateTime CreatedAt, DateTime PaidAt) : ICommand<CreatePaymentResult>; 
public record CreatePaymentResult(Guid PaymentId);
internal class CreatePaymentCommandHandler(IDocumentSession session, ILogger<CreatePaymentCommandHandler> logger)
    : ICommandHandler<CreatePaymentCommand, CreatePaymentResult>
{
    public async Task<CreatePaymentResult> Handle(CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        var payment = new PaymentModel
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            PaymentMethod = command.PaymentMethod,
            PaymentStatus = command.PaymentStatus,
            CreatedAt = command.CreatedAt,
            PaidAt = command.PaidAt
        };

        session.Store(payment);
        await session.SaveChangesAsync(cancellationToken);

        return new CreatePaymentResult(payment.Id);
    }
}
