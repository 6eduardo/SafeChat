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
            builder.Services.AddTransient<ConversationsViewModel>();
            builder.Services.AddTransient<ConversationsPage>();
            builder.Services.AddTransient<ChatsViewModel>();
            builder.Services.AddTransient<ChatsPage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ChatViewModel>();
            builder.Services.AddTransient<ChatPage>();

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
            services.AddSingleton<SignalRService>();
            services.AddHttpClient<AuthenticationService>((sp, client) =>
            {
                var config = sp.GetRequiredService<ApiConfiguration>();
                client.BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = config.RequestTimeout;
            });
            services.AddSingleton<IAuthenticationService>(sp => sp.GetRequiredService<AuthenticationService>());
            services.AddHttpClient<ConversationService>((sp, client) =>
            {
                var config = sp.GetRequiredService<ApiConfiguration>();
                client.BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = config.RequestTimeout;
            });
            services.AddSingleton<IConversationService>(sp => sp.GetRequiredService<ConversationService>());
            services.AddHttpClient<ChatService>((sp, client) =>
            {
                var config = sp.GetRequiredService<ApiConfiguration>();
                client.BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = config.RequestTimeout;
            });
            services.AddSingleton<IChatService>(sp => sp.GetRequiredService<ChatService>());
            services.AddHttpClient<ContactService>((sp, client) =>
            {
                var config = sp.GetRequiredService<ApiConfiguration>();
                client.BaseAddress = new Uri(config.BaseUrl.TrimEnd('/') + "/");
                client.Timeout = config.RequestTimeout;
            });
            services.AddSingleton<IContactService>(sp => sp.GetRequiredService<ContactService>());
            services.AddSingleton<IProfileService, MockProfileService>();

            // Armazenamento seguro
            services.AddSingleton<SecureKeyStorageService>();

            // Criptografia
            services.AddSingleton<RsaKeyService>();
            services.AddSingleton<AesEncryptionService>();
            services.AddSingleton<MessageEncryptionService>();

            // Navegação MVVM
            services.AddSingleton<NavigationService>();
        }
    }
}
