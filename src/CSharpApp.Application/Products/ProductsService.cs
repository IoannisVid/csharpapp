using CSharpApp.Core.Common;

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
        var response = await _httpClient.GetAsync(_restApiSettings.Products);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Product>>(content);

        return res.AsReadOnly();
    }

    public async Task<CallResult<Product>> GetProductById(int Id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                return CallResult<Product>.GetErrorFromResponse("message", errorJson);
            }
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Product>(content);

            return CallResult<Product>.Ok(res);
        }
        catch (Exception ex)
        {
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
                return CallResult<Product>.GetErrorFromResponse("message", errorJson);
            }
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Product>(content);

            return CallResult<Product>.Ok(res);
        }
        catch (Exception ex)
        {
            return CallResult<Product>.Fail(ex.Message);
        }
    }
}