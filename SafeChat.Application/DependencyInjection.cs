using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SafeChat.Application.Interfaces.Services;
using SafeChat.Application.Services;
using SafeChat.Application.Validators;

namespace SafeChat.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}
