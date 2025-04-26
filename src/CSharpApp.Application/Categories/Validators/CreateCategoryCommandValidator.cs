using CSharpApp.Application.Categories.Commands;

namespace CSharpApp.Application.Categories.Validators
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Image).NotEmpty().WithMessage("An image is required");
        }
    }
}
