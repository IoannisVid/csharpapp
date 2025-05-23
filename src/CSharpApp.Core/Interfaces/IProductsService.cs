namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProducts();
    Task<CallResult<Product>> GetProductById(int Id);
    Task<CallResult<Product>> CreateProduct(CreateProduct createProduct);
}