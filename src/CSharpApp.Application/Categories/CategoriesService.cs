namespace CSharpApp.Application.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Client");
            _restApiSettings = restApiSettings.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync(_restApiSettings.Categories);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Category>>(content);
                return res.AsReadOnly();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<CallResult<Category>> GetCategoryById(int Id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{Id}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var errRes = CallResult<Category>.GetErrorFromResponse("message", errorJson);
                    _logger.LogError("Request [GET] {url} failed: {msg}", response.RequestMessage?.RequestUri?.PathAndQuery.ToString(), errRes.ErrorMessage);
                    return errRes;
                }
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                return CallResult<Category>.Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return CallResult<Category>.Fail(ex.Message);
            }
        }
        public async Task<CallResult<Category>> CreateCategory(CreateCategory createCategory)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Categories, createCategory);
                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    var errRes = CallResult<Category>.GetErrorFromResponse("message", errorJson);
                    _logger.LogError("Request [POST] {url} failed: {msg}", response.RequestMessage?.RequestUri?.PathAndQuery.ToString(), errRes.ErrorMessage);
                    return errRes;
                }
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                return CallResult<Category>.Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return CallResult<Category>.Fail(ex.Message);
            }
        }
    }
}
