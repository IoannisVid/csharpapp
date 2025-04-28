namespace CSharpApp.Tests.Application.Products
{
    public class ProductServiceTests
    {
        private readonly ILogger<ProductsService> _logger;
        private readonly IOptions<RestApiSettings> _settings;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        public ProductServiceTests()
        {
            _logger = Mock.Of<ILogger<ProductsService>>();
            _settings = Options.Create(new RestApiSettings { Products = "products" });
            _httpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task GetProducts_Success_ReturnProductList()
        {
            var expectedProducts = new List<Product> {
                new() { Id = 1, Title = "Test Product", Price = 20 },
                new() { Id = 2, Title = "Another Test Product", Price = 10 }};

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/products")
                .Respond("application/json", JsonSerializer.Serialize(expectedProducts));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);

            var service = new ProductsService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetProducts();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetProductById_Success_ReturnProduct()
        {
            var expectedProduct = new Product { Id = 1, Title = "Test Product", Price = 20 };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/products/1")
                .Respond("application/json", JsonSerializer.Serialize(expectedProduct));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new ProductsService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetProductById(1);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedProduct.Id, result.Data.Id);
            Assert.Equal(expectedProduct.Title, result.Data.Title);
            Assert.Equal(expectedProduct.Price, result.Data.Price);
        }

        [Fact]
        public async Task GetProductById_Fail_ReturnErrorMessage()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://fakeapi.com/products/1")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"message\": \"Something went wrong\"}", Encoding.UTF8, "application/json")
                });

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new ProductsService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.GetProductById(1);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Something went wrong", result.ErrorMessage);
        }

        [Fact]
        public async Task PostCreateProduct_Success_ReturnProduct()
        {
            var createProduct = new CreateProduct { Title = "Test Product", Price = 40, CategoryId = 1, Description = "A description", Images = new List<string> { "testimage.png" } };
            var product = new Product { Id = 1, Title = "Test Product", Price = 40, Description = "A description" };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/products")
                .Respond("application/json", JsonSerializer.Serialize(product));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new ProductsService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.CreateProduct(createProduct);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Id, result.Data.Id);
            Assert.Equal(product.Price, result.Data.Price);
        }

        [Fact]
        public async Task PostCreateProduct_Fail_ReturnErrorMessage()
        {
            var createProduct = new CreateProduct();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/products")
                .Respond(req => new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"message\": \"Something went wrong\"}", Encoding.UTF8, "application/json")
                });

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("Client")).Returns(client);
            var service = new ProductsService(_settings, _logger, _httpClientFactory.Object);

            var result = await service.CreateProduct(createProduct);

            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Something went wrong", result.ErrorMessage);
        }
    }
}