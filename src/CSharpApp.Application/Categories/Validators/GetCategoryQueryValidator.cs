using CSharpApp.Application.Categories.Queries;

namespace CSharpApp.Application.Categories.Validators
{
    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Category ID must be greater than zero");
        }
    }
}
