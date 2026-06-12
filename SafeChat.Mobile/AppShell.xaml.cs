using SafeChat.Mobile.Views;

namespace SafeChat.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("Login", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("Chat", typeof(ChatPage));
        }
    }
}
