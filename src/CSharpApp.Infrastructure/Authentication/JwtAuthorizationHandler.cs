namespace CSharpApp.Infrastructure.Authentication
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;

        public JwtAuthorizationHandler(IAuthService authService) => _authService = authService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _authService.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = await _authService.RefreshTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await base.SendAsync(request, cancellationToken);
            }
            return response;
        }
    }
}
