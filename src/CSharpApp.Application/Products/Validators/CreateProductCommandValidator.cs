using CSharpApp.Application.Products.Commands;

namespace CSharpApp.Application.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category ID must be greater than zero");

            RuleFor(x => x.Images).NotEmpty().WithMessage("At least one image is required");
        }
    }
}
