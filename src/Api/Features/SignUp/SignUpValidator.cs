using FluentValidation;

namespace Api.Features.SignUp;

public class SignUpValidator : AbstractValidator<SignUpRequest>
{
    public SignUpValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ErrorMessages.UserNameIsEmpty)
            .MaximumLength(50).WithMessage(ErrorMessages.UserNameIsTooLong)
            .Matches("^[a-zA-Z0-9_]*$").WithMessage(ErrorMessages.UserNameContainsInvalidCharacters);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ErrorMessages.EmailIsEmpty)
            .MaximumLength(50).WithMessage(ErrorMessages.EmailIsTooLong)
            .EmailAddress().WithMessage(ErrorMessages.EmailIsInvalid);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ErrorMessages.PasswordIsEmpty)
            .MinimumLength(8).WithMessage(ErrorMessages.PasswordIsTooShort)
            .MaximumLength(50).WithMessage(ErrorMessages.PasswordIsTooLong)
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$").WithMessage(ErrorMessages.PasswordMustBeValid);
    }
}
