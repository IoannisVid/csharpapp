namespace CSharpApp.Api.Endpoints
{
    public static class CategoryRoutes
    {
        public static void MapCategoryRoutes(this IEndpointRouteBuilder versionedEndpointRouteBuilder)
        {
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (IMediator mediator) =>
            {
                var categories = await mediator.Send(new GetAllCategoriesQuery());
                if (categories.Count == 0)
                    return Results.NoContent();
                return Results.Ok(categories);
            })
            .WithName("GetCategories")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories/{id}", async (int id, IMediator mediator) =>
            {
                var result = await mediator.Send(new GetCategoryQuery(id));
                if (!result.Success)
                    return Results.BadRequest(result.ErrorMessage);
                return Results.Ok(result.Data);
            })
            .WithName("GetCategoryById")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (CreateCategoryCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                if (!result.Success)
                    return Results.BadRequest(result.ErrorMessage);
                return Results.CreatedAtRoute("GetCategoryById", new { id = result.Data!.Id }, result.Data);
            })
            .WithName("CreateCategory")
            .HasApiVersion(1.0);
        }
    }
}
