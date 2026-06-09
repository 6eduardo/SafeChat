using FluentValidation;
using SafeChat.Application.DTOs.Auth;

namespace SafeChat.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.EmailOrUsername)
            .NotEmpty().WithMessage("O e-mail ou nome de utilizador é obrigatório.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A palavra-passe é obrigatória.");
    }
}
