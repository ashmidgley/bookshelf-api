using FluentValidation;
using Reads.Models;

namespace Reads.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Description).NotNull();
            RuleFor(category => category.Code).NotNull();
        }
    }
}
