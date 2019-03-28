using FluentValidation;
using Reads.Models;

namespace Reads.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.CategoryId).NotNull();
            RuleFor(book => book.Image).NotNull();
            RuleFor(book => book.Title).NotNull();
            RuleFor(book => book.Author).NotNull();
            RuleFor(book => book.StartedOn).NotNull();
            RuleFor(book => book.FinishedOn).NotNull();
            RuleFor(book => book.PageCount).NotNull();
            RuleFor(book => book.Summary).NotNull();
        }
    }
}
