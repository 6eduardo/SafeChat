using SafeChat.Mobile.ViewModels;

namespace SafeChat.Mobile.Views;

public partial class ConversationsPage : ContentPage
{
  private readonly ConversationsViewModel _viewModel;

  public ConversationsPage(ConversationsViewModel viewModel)
  {
    InitializeComponent();
    _viewModel = viewModel;
    BindingContext = viewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();
    _viewModel.LoadConversationsCommand.Execute(null);
  }
}
