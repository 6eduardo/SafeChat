using Microsoft.Extensions.Logging;
using SafeChat.Mobile.Configuration;
using SafeChat.Mobile.Services.Api;
using SafeChat.Mobile.Services.Auth;
using SafeChat.Mobile.Services.Crypto;
using SafeChat.Mobile.Services.Interfaces;
using SafeChat.Mobile.Services.Mock;
using SafeChat.Mobile.Services.Navigation;
using SafeChat.Mobile.Services.RealTime;
using SafeChat.Mobile.Services.Storage;
using SafeChat.Mobile.ViewModels;
using SafeChat.Mobile.Views;

namespace SafeChat.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RegisterServices(builder.Services);

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<RegisterPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Configuração
            services.AddSingleton<ApiConfiguration>();

            // Autenticação e tokens
            services.AddSingleton<TokenService>();
            services.AddSingleton<IAuthenticationService, MockAuthenticationService>();

            // Armazenamento seguro
            services.AddSingleton<SecureKeyStorageService>();

            // Criptografia
            services.AddSingleton<RsaKeyService>();
            services.AddSingleton<AesEncryptionService>();
            services.AddSingleton<MessageEncryptionService>();

            // Comunicação REST (comentado — usar quando a API real estiver pronta)
            // services.AddHttpClient<AuthenticationService>();
            // services.AddHttpClient<ConversationService>();
            // services.AddHttpClient<ChatService>();
            // services.AddHttpClient<ContactService>();

            // Tempo real
            services.AddSingleton<SignalRService>();

            // Navegação MVVM
            services.AddSingleton<NavigationService>();
        }
    }
}
