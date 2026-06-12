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
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        return services;
    }
}
