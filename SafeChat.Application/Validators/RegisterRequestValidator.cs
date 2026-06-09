using FluentValidation;
using SafeChat.Application.DTOs.Auth;

namespace SafeChat.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("O nome de utilizador é obrigatório.")
            .MaximumLength(50).WithMessage("O nome de utilizador não pode exceder 50 caracteres.")
            .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("O nome de utilizador só pode conter letras, números, pontos e underscores.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório.")
            .EmailAddress().WithMessage("O e-mail não é válido.")
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A palavra-passe é obrigatória.")
            .MinimumLength(8).WithMessage("A palavra-passe deve ter pelo menos 8 caracteres.");
    }
}
