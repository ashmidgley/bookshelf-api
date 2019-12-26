using FluentValidation;

namespace Api
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(dto => dto.CategoryId).NotNull();
            RuleFor(dto => dto.RatingId).NotNull();
            RuleFor(dto => dto.Image).NotNull();
            RuleFor(dto => dto.Title).NotNull();
            RuleFor(dto => dto.Author).NotNull();
            RuleFor(dto => dto.StartedOn).NotNull();
            RuleFor(dto => dto.FinishedOn).NotNull();
            RuleFor(dto => dto.Year).NotNull();
            RuleFor(dto => dto.PageCount).NotNull();
            RuleFor(dto => dto.Summary).NotNull();
        }
    }
}
