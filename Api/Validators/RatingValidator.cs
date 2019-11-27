using FluentValidation;

namespace Bookshelf
{
    public class RatingValidator : AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(rating => rating.Description).NotNull();
            RuleFor(rating => rating.Code).NotNull();
        }
    }
}
