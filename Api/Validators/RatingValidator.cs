using FluentValidation;

namespace Api
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
