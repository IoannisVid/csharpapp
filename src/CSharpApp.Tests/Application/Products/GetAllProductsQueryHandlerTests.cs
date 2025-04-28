namespace CSharpApp.Tests.Application.Products
{
    public class GetAllProductsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnProduct()
        {
            var products = new List<Product> {
                new() { Id = 1, Title = "Test Product", Price = 20 },
                new() { Id = 2, Title = "Another Test Product", Price = 10 }};
            var _productsService = new Mock<IProductsService>();
            _productsService.Setup(x => x.GetProducts())
                .ReturnsAsync(products);

            var query = new GetAllProductsQuery();
            var handler = new GetAllProductsQueryHandler(_productsService.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
