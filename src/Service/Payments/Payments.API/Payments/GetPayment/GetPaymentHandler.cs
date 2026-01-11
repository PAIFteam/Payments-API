namespace Payments.API.Payments.GetPayment; 

public record GetPaymentQuery() : IQuery<GetPaymentResult>;
public record GetPaymentResult(IEnumerable<PaymentModel> Payment);
internal class GetPaymentQueryHandler(IDocumentSession session, ILogger<GetPaymentQueryHandler> logger)
    : IQueryHandler<GetPaymentQuery, GetPaymentResult>
{
    public async Task<GetPaymentResult> Handle(GetPaymentQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetPaymentQueryHandler.Handle called with {@Query}", query);

        var payment = await session.Query<PaymentModel>()
            .ToListAsync(cancellationToken);

        return new GetPaymentResult(payment);
    }
}
