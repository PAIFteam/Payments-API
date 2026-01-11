namespace Payments.API.Payments.GetPaymentById;

public record GetPaymentByIdQuery(Guid Id) : IQuery<GetPaymentByIdResult>; 
public record GetPaymentByIdResult(PaymentModel Payment);
internal class GetPaymentByIdQueryHandler(IDocumentSession session, ILogger<GetPaymentByIdQueryHandler> logger)
    : IQueryHandler<GetPaymentByIdQuery, GetPaymentByIdResult>
{
    public async Task<GetPaymentByIdResult> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetPaymentByIdQuery for PaymentId: {PaymentId}", query);

        var payment = await session.LoadAsync<PaymentModel>(query.Id, cancellationToken);

        if(payment is null)
        {
            throw new PaymentNotFoundException();
        }

        return new GetPaymentByIdResult(payment);
    }
}
