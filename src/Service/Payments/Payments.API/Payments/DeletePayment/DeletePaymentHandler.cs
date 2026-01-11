namespace Payments.API.Payments.DeletePayment;

public record DeletePaymentCommand(Guid Id) : ICommand<DeletePaymentResult>;
public record DeletePaymentResult(bool Success);
internal class DeletePaymentCommandHandler(IDocumentSession session, ILogger<DeletePaymentCommandHandler> logger)
    : ICommandHandler<DeletePaymentCommand, DeletePaymentResult>
{
    public async Task<DeletePaymentResult> Handle(DeletePaymentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeletePaymentCommandHandler.Handle called with {@Command}", command);

        session.Delete(command.Id);
        await session.SaveChangesAsync();

        return new DeletePaymentResult(true);
    }
}
