namespace CSharpApp.Application.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CallResult<Category>>
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IValidator<CreateCategoryCommand> _validator;
        public CreateCategoryCommandHandler(ICategoriesService categoriesService, IValidator<CreateCategoryCommand> validator)
        {
            _categoriesService = categoriesService;
            _validator = validator;
        }

        public async Task<CallResult<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return CallResult<Category>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            var createCategory = new CreateCategory
            {
                Name = request.Name,
                Image = request.Image
            };
            return await _categoriesService.CreateCategory(createCategory);
        }
    }
}