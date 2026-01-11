namespace Payments.API.Payments.UpdatePayment; 

public record UpdatePaymentCommand(Guid Id, decimal Amount, PaymentMethod PaymentMethod, 
    PaymentStatus PaymentStatus, DateTime CreatedAt, DateTime PaidAt) : ICommand<UpdatePaymentResult>;
public record UpdatePaymentResult(PaymentModel Payment);
internal class UpdatePaymentCommandHandler(IDocumentSession session, ILogger<UpdatePaymentCommandHandler> logger)
    : ICommandHandler<UpdatePaymentCommand, UpdatePaymentResult>
{
    public async Task<UpdatePaymentResult> Handle(UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdatePaymentHandler.Handle called with {@Command}", command);

        var payment = await session.LoadAsync<PaymentModel>(command.Id, cancellationToken);

        if(payment is null)
        {
            throw new PaymentNotFoundException();
        }

        payment.Amount = command.Amount;
        payment.PaymentMethod = command.PaymentMethod;
        payment.PaymentStatus = command.PaymentStatus;
        payment.CreatedAt = command.CreatedAt;
        payment.PaidAt = command.PaidAt;

        session.Store(payment);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdatePaymentResult(payment);
    }
}
