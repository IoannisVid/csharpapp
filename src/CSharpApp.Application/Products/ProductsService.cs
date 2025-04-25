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

    public async Task<Product> GetProductById(int Id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{Id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<Product>(content);

            return res;
        }
        catch (HttpRequestException ex)
        {            
            return null;
        }
        catch (Exception)
        {

            throw;
        }

    }
}