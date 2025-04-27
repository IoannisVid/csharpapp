namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Client");
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        try
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Products);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Product>>(content);
            return res.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<CallResult<Product>> GetProductById(int Id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var errRes = CallResult<Product>.GetErrorFromResponse("message", errorJson);
                _logger.LogError("Request [GET] {url} failed: {msg}", response.RequestMessage?.RequestUri?.PathAndQuery.ToString(), errRes.ErrorMessage);
                return errRes;
            }
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Product>(content);
            return CallResult<Product>.Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return CallResult<Product>.Fail(ex.Message);
        }
    }

    public async Task<CallResult<Product>> CreateProduct(CreateProduct createProduct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_restApiSettings.Products, createProduct);
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                var errRes = CallResult<Product>.GetErrorFromResponse("message", errorJson);
                _logger.LogError("Request [POST] {url} failed: {msg}", response.RequestMessage?.RequestUri?.PathAndQuery.ToString(), errRes.ErrorMessage);
                return errRes;
            }
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Product>(content);
            return CallResult<Product>.Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return CallResult<Product>.Fail(ex.Message);
        }
    }
}