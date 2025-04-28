using CSharpApp.Application.Categories;

namespace CSharpApp.Tests.Application.Categories
{
    public class CategoriesServiceTests
    {
        private readonly ILogger<CategoriesService> _logger;
        private readonly IOptions<RestApiSettings> _settings;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        public CategoriesServiceTests()
        {
            _logger = Mock.Of<ILogger<CategoriesService>>();
            _settings = Options.Create(new RestApiSettings { Categories = "categories" });
            _httpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task GetCategories_Success_ReturnCategoriesList()
        {
            var expectedCategories = new List<Category> {
                new() { Id = 1, Name = "Test Category" },
                new() { Id = 2, Name = "Another Test Category" }};

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/categories")
                .Respond("application/json", JsonSerializer.Serialize(expectedCategories));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);

            var service = new CategoriesService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetCategories();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetCategoryById_Success_ReturnProduct()
        {
            var expectedCategory = new Category { Id = 1, Name = "Test Category" };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/categories/1")
                .Respond("application/json", JsonSerializer.Serialize(expectedCategory));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new CategoriesService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetCategoryById(1);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedCategory.Id, result.Data.Id);
            Assert.Equal(expectedCategory.Name, result.Data.Name);
        }

        [Fact]
        public async Task GetCategoryById_Fail_ReturnErrorMessage()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/categories/1")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"message\": \"Something went wrong\"}", Encoding.UTF8, "application/json")
                });

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new CategoriesService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetCategoryById(1);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Something went wrong", result.ErrorMessage);
        }

        [Fact]
        public async Task PostCreateCategory_Success_ReturnProduct()
        {
            var createCategory = new CreateCategory { Name = "Test Category", Image = "imageurl" };
            var category = new Category { Id = 1, Name = "Test Category", Image = "imageurl" };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/categories")
                .Respond("application/json", JsonSerializer.Serialize(category));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new CategoriesService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.CreateCategory(createCategory);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(category.Id, result.Data.Id);
            Assert.Equal(category.Name, result.Data.Name);
            Assert.Equal(category.Image, result.Data.Image);
        }

        [Fact]
        public async Task PostCreateCategory_Fail_ReturnErrorMessage()
        {
            var createCategory = new CreateCategory();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/categories")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"message\": \"Something went wrong\"}", Encoding.UTF8, "application/json")
                });

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new CategoriesService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.CreateCategory(createCategory);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Something went wrong", result.ErrorMessage);
        }
    }
}
