namespace CSharpApp.Tests.Application.Categories
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICategoriesService> _categoriesService;
        private readonly Mock<IValidator<CreateCategoryCommand>> _validator;
        public CreateCategoryCommandHandlerTests()
        {
            _categoriesService = new Mock<ICategoriesService>();
            _validator = new Mock<IValidator<CreateCategoryCommand>>();
        }

        [Fact]
        public async Task Handle_ValidatorSuccess_ReturnCategory()
        {
            var category = new CallResult<Category> { Success = true, Data = new Category { Id = 1, Name = "Test Category", Image = "testimage.png" } };
            _categoriesService.Setup(x => x.CreateCategory(It.IsAny<CreateCategory>()))
                .ReturnsAsync(category);

            var command = new CreateCategoryCommand { Name = "Test Category", Image = "testimage.png" };
            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var handler = new CreateCategoryCommandHandler(_categoriesService.Object, _validator.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(category.Data.Id, result.Data.Id);
            Assert.Equal(category.Data.Name, result.Data.Name);
            Assert.Equal(category.Data.Image, result.Data.Image);
        }

        [Fact]
        public async Task Handle_ValidatorFail_ReturnError()
        {
            var command = new CreateCategoryCommand { Name = "Test Category", Image = "" };
            var validationFailures = new List<ValidationFailure>
            {
                new("Image", "An image is required")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new CreateCategoryCommandHandler(_categoriesService.Object, _validator.Object);
            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("An image is required", result.ErrorMessage);
        }
    }
}
