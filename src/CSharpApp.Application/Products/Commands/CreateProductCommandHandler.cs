namespace CSharpApp.Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CallResult<Product>>
    {
        private readonly IProductsService _productsService;
        public CreateProductCommandHandler(IProductsService productsService) => _productsService = productsService;

        public async Task<CallResult<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createProduct = new CreateProduct
            {
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Images = request.Images
            };
            return await _productsService.CreateProduct(createProduct);
        }
    }
}
