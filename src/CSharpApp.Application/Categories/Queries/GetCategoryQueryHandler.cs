namespace CSharpApp.Application.Categories.Queries
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CallResult<Category>>
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IValidator<GetCategoryQuery> _validator;
        public GetCategoryQueryHandler(ICategoriesService categoriesService, IValidator<GetCategoryQuery> validator)
        {
            _categoriesService = categoriesService;
            _validator = validator;
        }

        public async Task<CallResult<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return CallResult<Category>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));

            return await _categoriesService.GetCategoryById(request.Id);
        }
    }
}
