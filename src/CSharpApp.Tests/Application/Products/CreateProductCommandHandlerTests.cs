namespace CSharpApp.Tests.Application.Products
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductsService> _productService;
        private readonly Mock<IValidator<CreateProductCommand>> _validator;
        public CreateProductCommandHandlerTests()
        {
            _productService = new Mock<IProductsService>();
            _validator = new Mock<IValidator<CreateProductCommand>>();
        }

        [Fact]
        public async Task Handle_ValidatorSuccess_ReturnProduct()
        {
            var product = new CallResult<Product> { Success = true, Data = new Product { Id = 1, Title = "Test Product", Price = 40, Description = "A description" } };
            _productService.Setup(x => x.CreateProduct(It.IsAny<CreateProduct>()))
                .ReturnsAsync(product);

            var command = new CreateProductCommand { Title = "Test Product", Price = 40, CategoryId = 1, Description = "A description", Images = new List<string> { "imageurl" } };
            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var handler = new CreateProductCommandHandler(_productService.Object, _validator.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Data.Id, result.Data.Id);
            Assert.Equal(product.Data.Title, result.Data.Title);
        }

        [Fact]
        public async Task Handle_ValidatorFail_ReturnError()
        {
            var command = new CreateProductCommand { Title = "Test Product", Price = 0, CategoryId = 1, Description = "A description", Images = new List<string> { "imageurl" } };
            var validationFailures = new List<ValidationFailure>
            {
                new("Price", "Price must be greater than zero")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new CreateProductCommandHandler(_productService.Object, _validator.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("Price must be greater than zero", result.ErrorMessage);
        }
    }
}
