namespace CSharpApp.Api.Endpoints
{
    public static class ProductRoutes
    {
        public static void MapProductRoutes(this IEndpointRouteBuilder versionedEndpointRouteBuilder)
        {
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
            {
                var products = await productsService.GetProducts();
                if (products.Count == 0)
                    return Results.NoContent();
                return Results.Ok(products);
            })
            .WithName("GetProducts")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts/{id}", async (int id, IProductsService productsService) =>
            {
                var result = await productsService.GetProductById(id);
                if (!result.Success)
                    return Results.BadRequest(result.ErrorMessage);
                return Results.Ok(result.Data);
            })
            .WithName("GetProductById")
            .HasApiVersion(1.0);
        }
    }
}
