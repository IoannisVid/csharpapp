namespace CSharpApp.Tests.Infrastructure
{
    public class AuthServiceTests
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IOptions<RestApiSettings> _settings;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;

        public AuthServiceTests()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _logger = Mock.Of<ILogger<AuthService>>();

            _settings = Options.Create(new RestApiSettings
            {
                Auth = "https://fakeapi.com/auth",
                Refresh = "https://fakeapi.com/refresh",
                Username = "testuser",
                Password = "testpassword"
            });
        }

        [Fact]
        public async Task GetTokenAsync_Successfull_ReturnToken()
        {
            var token = new AuthToken { AccessToken = "access-token", RefreshToken = "refresh-token" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/auth")
                .Respond("application/json", JsonSerializer.Serialize(token));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("AuthClient")).Returns(client);

            var service = new AuthService(_settings, _httpClientFactory.Object, _logger);

            var accessToken = await service.GetTokenAsync();

            Assert.Equal("access-token", accessToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldCallGetTokenAsync_WhenRefreshTokenExists_ReturnRefreshedToken()
        {
            var token = new AuthToken { AccessToken = "new-access-token", RefreshToken = "refresh-token" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/auth")
                .Respond("application/json", JsonSerializer.Serialize(token));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(x => x.CreateClient("AuthClient")).Returns(client);

            var service = new AuthService(_settings, _httpClientFactory.Object, _logger);

            var refreshedToken = await service.RefreshTokenAsync();

            Assert.Equal("new-access-token", refreshedToken);
        }

        [Fact]
        public async Task RefreshTokenAsync_GetRefreshToken_UnauthorizedOnGetToken_RefreshToken_ReturnRefreshedToken()
        {
            var token = new AuthToken { AccessToken = "new-access-token", RefreshToken = "new-refresh-token" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/refresh")
                .Respond(HttpStatusCode.Unauthorized);

            mockHttp.When(HttpMethod.Post, "https://fakeapi.com/auth")
                .Respond("application/json", JsonSerializer.Serialize(token));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://fakeapi.com");

            _httpClientFactory.Setup(f => f.CreateClient("AuthClient")).Returns(client);

            var service = new AuthService(_settings, _httpClientFactory.Object, _logger);

            await service.GetTokenAsync();

            var newToken = await service.RefreshTokenAsync();

            Assert.Equal("new-access-token", newToken);
        }
    }
}
