namespace CSharpApp.Application.Products.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, CallResult<Product>>
    {
        private readonly IProductsService _productsService;
        public GetProductQueryHandler(IProductsService productsService) => _productsService = productsService;

        public async Task<CallResult<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _productsService.GetProductById(request.Id);
        }
    }
}
