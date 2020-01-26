using FluentValidation;

namespace Api
{
    public class NewBookValidator : AbstractValidator<NewBookDto>
    {
        public NewBookValidator()
        {
            RuleFor(book => book.ISBN).NotNull();
            RuleFor(book => book.UserId).NotNull();
            RuleFor(book => book.CategoryId).NotNull();
            RuleFor(book => book.RatingId).NotNull();
            RuleFor(book => book.FinishedOn).NotNull();
        }
    }
}
