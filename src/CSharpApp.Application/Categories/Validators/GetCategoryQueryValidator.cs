using CSharpApp.Application.Categories.Queries;

namespace CSharpApp.Application.Categories.Validators
{
    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Product ID must be greater than zero");
        }
    }
}
