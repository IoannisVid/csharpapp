using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Products.Queries;

namespace CSharpApp.Application.Products.Validators
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Product ID must be greater than zero");
        }
    }
}
