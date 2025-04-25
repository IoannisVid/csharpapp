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
                var product = await productsService.GetProductById(id);
                if (product == null)
                    return Results.BadRequest();
                return Results.Ok(product);
            })
            .WithName("GetProductById")
            .HasApiVersion(1.0);
        }
    }
}
