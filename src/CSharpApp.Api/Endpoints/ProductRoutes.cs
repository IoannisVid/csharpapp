using CSharpApp.Application.Products.Commands;

namespace CSharpApp.Api.Endpoints
{
    public static class ProductRoutes
    {
        public static void MapProductRoutes(this IEndpointRouteBuilder versionedEndpointRouteBuilder)
        {
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IMediator mediator) =>
            {
                var products = await mediator.Send(new GetAllProductsQuery());
                if (products.Count == 0)
                    return Results.NoContent();
                return Results.Ok(products);
            })
            .WithName("GetProducts")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetProductQuery(id));
                if (!result.Success)
                    return Results.BadRequest(result.ErrorMessage);
                return Results.Ok(result.Data);
            })
            .WithName("GetProductById")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (CreateProductCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                if (!result.Success)
                    return Results.BadRequest(result.ErrorMessage);
                return Results.CreatedAtRoute("GetProductById", new { id = result.Data!.Id }, result.Data);
            })
            .WithName("CreateProduct")
            .HasApiVersion(1.0);
        }
    }
}
