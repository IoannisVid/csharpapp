using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Categories.Queries;

namespace CSharpApp.Tests.Application.Categories
{
    public class GetCategoryQueryHandlerTests
    {
        private readonly Mock<ICategoriesService> _categoriesService;
        private readonly Mock<IValidator<GetCategoryQuery>> _validator;
        public GetCategoryQueryHandlerTests()
        {
            _categoriesService = new Mock<ICategoriesService>();
            _validator = new Mock<IValidator<GetCategoryQuery>>();
        }

        [Fact]
        public async Task Handle_ValidatorSuccess_ReturnCategory()
        {
            var category = new CallResult<Category> { Success = true, Data = new Category { Id = 1, Name = "Test Category" } };
            _categoriesService.Setup(x => x.GetCategoryById(1))
                .ReturnsAsync(category);

            var query = new GetCategoryQuery(1);
            _validator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var handler = new GetCategoryQueryHandler(_categoriesService.Object, _validator.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(category.Data.Id, result.Data.Id);
            Assert.Equal(category.Data.Name, result.Data.Name);
        }

        [Fact]
        public async Task Handle_ValidatorFail_ReturnError()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new("Id", "Category ID must be greater than zero")
            };
            var validationResult = new ValidationResult(validationFailures);

            var query = new GetCategoryQuery(0);
            _validator.Setup(v => v.ValidateAsync(query, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new GetCategoryQueryHandler(_categoriesService.Object, _validator.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("Category ID must be greater than zero", result.ErrorMessage);
        }
    }
}
