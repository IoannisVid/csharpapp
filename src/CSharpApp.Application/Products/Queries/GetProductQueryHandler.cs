namespace CSharpApp.Application.Products.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, CallResult<Product>>
    {
        private readonly IProductsService _productsService;
        private readonly IValidator<GetProductQuery> _validator;
        public GetProductQueryHandler(IProductsService productsService, IValidator<GetProductQuery> validator)
        {
            _productsService = productsService;
            _validator = validator;
        }

        public async Task<CallResult<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return CallResult<Product>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            return await _productsService.GetProductById(request.Id);
        }
    }
}
