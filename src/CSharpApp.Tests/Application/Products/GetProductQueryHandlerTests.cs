namespace CSharpApp.Tests.Application.Products
{
    public class GetProductQueryHandlerTests
    {
        private readonly Mock<IProductsService> _productsService;
        private readonly Mock<IValidator<GetProductQuery>> _validator;
        public GetProductQueryHandlerTests()
        {
            _productsService = new Mock<IProductsService>();
            _validator = new Mock<IValidator<GetProductQuery>>();
        }

        [Fact]
        public async Task Handle_ValidatorSuccess_ReturnProduct()
        {
            var product = new CallResult<Product> { Success = true, Data = new Product { Id = 1, Title = "Test Product" } };
            _productsService.Setup(x => x.GetProductById(1))
                .ReturnsAsync(product);

            var query = new GetProductQuery(1);
            _validator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var handler = new GetProductQueryHandler(_productsService.Object, _validator.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Data.Id, result.Data.Id);
            Assert.Equal(product.Data.Title, result.Data.Title);
        }

        [Fact]
        public async Task Handle_ValidatorFail_ReturnError()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new("Id", "Product ID must be greater than zero")
            };
            var validationResult = new ValidationResult(validationFailures);

            var query = new GetProductQuery(0);
            _validator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new GetProductQueryHandler(_productsService.Object, _validator.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("Product ID must be greater than zero", result.ErrorMessage);
        }
    }
}
