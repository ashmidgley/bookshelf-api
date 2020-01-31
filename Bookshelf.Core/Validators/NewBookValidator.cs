using FluentValidation;

namespace Bookshelf.Core
{
    public class NewBookValidator : AbstractValidator<NewBookDto>
    {
        public NewBookValidator()
        {
            RuleFor(book => book.Title).NotNull();
            RuleFor(book => book.Author).NotNull();
            RuleFor(book => book.UserId).NotNull();
            RuleFor(book => book.CategoryId).NotNull();
            RuleFor(book => book.RatingId).NotNull();
            RuleFor(book => book.FinishedOn).NotNull();
        }
    }
}
