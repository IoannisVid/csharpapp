namespace CSharpApp.Tests.Application.Categories
{
    public class GetAllCategoriesQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnCategory()
        {
            var categories = new List<Category> {
                new() { Id = 1, Name = "Test Category", Image = "testimage.png" },
                new() { Id = 2, Name = "Another Test Product", Image = "testimage2.png" }};
            var _categoriesService = new Mock<ICategoriesService>();
            _categoriesService.Setup(x => x.GetCategories())
                .ReturnsAsync(categories);

            var query = new GetAllCategoriesQuery();
            var handler = new GetAllCategoriesQueryHandler(_categoriesService.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
