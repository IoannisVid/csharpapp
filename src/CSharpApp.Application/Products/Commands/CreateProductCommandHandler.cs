namespace CSharpApp.Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CallResult<Product>>
    {
        private readonly IProductsService _productsService;
        private readonly IValidator<CreateProductCommand> _validator;
        public CreateProductCommandHandler(IProductsService productsService, IValidator<CreateProductCommand> validator)
        {
            _productsService = productsService;
            _validator = validator;
        }

        public async Task<CallResult<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return CallResult<Product>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

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
