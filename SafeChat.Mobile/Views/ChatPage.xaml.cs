using SafeChat.Mobile.ViewModels;

namespace SafeChat.Mobile.Views;

public partial class ChatPage : ContentPage
{
  public ChatPage(ChatViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}
