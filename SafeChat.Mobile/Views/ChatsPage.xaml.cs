using SafeChat.Mobile.ViewModels;

namespace SafeChat.Mobile.Views;

public partial class ChatsPage : ContentPage
{
    private readonly ChatsViewModel _viewModel;

    public ChatsPage(ChatsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshHeaderCommand.Execute(null);
    }
}
