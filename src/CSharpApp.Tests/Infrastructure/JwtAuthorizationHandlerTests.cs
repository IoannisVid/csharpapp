using CSharpApp.Infrastructure.Authentication;
using Moq.Protected;

namespace CSharpApp.Tests.Infrastructure
{
    public class JwtAuthorizationHandlerTests
    {
        [Fact]
        public async Task SendAsync_FirstSuccess_AddToken()
        {
            var authService = new Mock<IAuthService>();
            authService.Setup(s => s.GetTokenAsync())
                           .ReturnsAsync("mock-token");

            var httpMsgHandler = new MockHttpMessageHandler();
            httpMsgHandler.Expect(HttpMethod.Get, "https://fakeapi.com")
                     .WithHeaders("Authorization", "Bearer mock-token")
                     .Respond(HttpStatusCode.OK);

            var jwtAuthHandler = new JwtAuthorizationHandler(authService.Object)
            {
                InnerHandler = httpMsgHandler
            };
            var client = new HttpClient(jwtAuthHandler);
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://fakeapi.com"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            httpMsgHandler.VerifyNoOutstandingExpectation();
        }

        [Fact]
        public async Task SendAsync_FirstUnauthorized_RefreshToken_ThenAddToken()
        {
            var authService = new Mock<IAuthService>();
            authService.SetupSequence(s => s.GetTokenAsync())
                           .ReturnsAsync("mock-token");
            authService.Setup(s => s.RefreshTokenAsync())
                           .ReturnsAsync("refresh-token");


            var httpMsgHandler = new MockHttpMessageHandler();
            httpMsgHandler.Expect(HttpMethod.Get, "https://fakeapi.com")
                    .WithHeaders("Authorization", "Bearer mock-token")
                    .Respond(HttpStatusCode.Unauthorized);

            httpMsgHandler.Expect(HttpMethod.Get, "https://fakeapi.com")
                    .WithHeaders("Authorization", "Bearer refresh-token")
                    .Respond(HttpStatusCode.OK);

            var jwtAuthHandler = new JwtAuthorizationHandler(authService.Object)
            {
                InnerHandler = httpMsgHandler
            };
            var client = new HttpClient(jwtAuthHandler);
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://fakeapi.com"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            httpMsgHandler.VerifyNoOutstandingExpectation();
        }
    }
}
