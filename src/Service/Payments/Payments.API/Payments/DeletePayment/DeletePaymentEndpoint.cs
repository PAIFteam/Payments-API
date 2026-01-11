
namespace Payments.API.Payments.DeletePayment;

//public record DeletePaymentRequest(Guid PaymentId);
public record DeletePaymentResponse(bool Success);
public class DeletePaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/payments/{Id:guid}", async (Guid Id, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DeletePaymentCommand(Id), cancellationToken);
            var response = result.Adapt<DeletePaymentResponse>();
            return Results.Ok(response);
        })
        .WithName("DeletePayment")
        .Produces<DeletePaymentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Payment")
        .WithDescription("Delete Payment");
    }
}
