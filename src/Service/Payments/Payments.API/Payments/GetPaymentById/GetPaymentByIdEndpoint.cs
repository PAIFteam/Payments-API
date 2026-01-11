namespace Payments.API.Payments.GetPaymentById;

//public record GetPaymentByIdRequest(Guid PaymentId);
public record GetPaymentByIdResponse(PaymentModel Payment);
public class GetPaymentByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/payments/{paymentId:guid}", async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetPaymentByIdQuery(Id), cancellationToken);
            var response = result.Adapt<GetPaymentByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetPaymentById")
        .Produces<GetPaymentByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Payment By Id")
        .WithDescription("Get Payment By Id");
    }
}
