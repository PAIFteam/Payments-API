namespace Payments.API.Payments.UpdatePayment;

public record UpdatePaymentRequest(Guid Id, decimal Amount, PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus, DateTime CreatedAt, DateTime PaidAt);
public record UpdatePaymentResponse(bool IsSuccess);
public class UpdatePaymentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/payments/{id:guid}", async (Guid id, UpdatePaymentRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<UpdatePaymentCommand>();
            var result = await sender.Send(command, cancellationToken);
            var response = new UpdatePaymentResponse(true);
            return Results.Ok(response);
        })
        .WithName("UpdatePayment")
        .Produces<UpdatePaymentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Payment")
        .WithDescription("Update Payment");
    }
}
