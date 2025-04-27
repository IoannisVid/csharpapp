namespace CSharpApp.Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
        private string? _accessToken;
        private string? _refreshToken;
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;

        public AuthService(IOptions<RestApiSettings> restApiSettings, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("AuthClient");
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Auth, new { email = _restApiSettings.Username, password = _restApiSettings.Password });

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<AuthToken>(content);
            _accessToken = res!.AccessToken;
            _refreshToken = res.RefreshToken;
            return _accessToken!;
        }

        public async Task<string> RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_refreshToken))
            {
                _accessToken = string.Empty;
                return await GetTokenAsync();
            }

            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Refresh, new { refreshToken = _refreshToken });
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _accessToken = string.Empty;
                _refreshToken = string.Empty;
                return await GetTokenAsync();
            }

            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<AuthToken>(content);
            _accessToken = res!.AccessToken;
            _refreshToken = res.RefreshToken;
            return _accessToken!;
        }
    }
}
