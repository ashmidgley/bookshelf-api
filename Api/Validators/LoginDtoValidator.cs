using FluentValidation;

namespace Api
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(dto => dto.Email).NotNull();
            RuleFor(dto => dto.Password).NotNull();
        }
    }
}
