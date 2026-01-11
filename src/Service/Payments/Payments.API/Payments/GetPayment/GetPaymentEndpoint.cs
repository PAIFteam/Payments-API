namespace Payments.API.Payments.GetPayment;

//public record GetPaymentRequest(Guid PaymentId);
public record GetPaymentResponse(IEnumerable<PaymentModel> Payment);
public class GetPaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/payments", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetPaymentQuery(), cancellationToken);
            var response = result.Adapt<GetPaymentResponse>();
            return Results.Ok(response);
        })
        .WithName("GetPayment")
        .Produces<GetPaymentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Payment")
        .WithDescription("Get Payment");
    }
}
