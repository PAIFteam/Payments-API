namespace Payments.API.Extensions;

public static class UserEndpointsExtensions
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");

        api.MapGet("/health", () => Results.Ok(new
        {
            Service = "payments-api",
            Status = "Healthy"
        }))
        .WithName("GetPaymentsHealth")
        .WithSummary("Health check do serviço de pagamentos")
        .Produces(StatusCodes.Status200OK);
    }
}
