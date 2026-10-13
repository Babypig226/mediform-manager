using MediFormManager.Wpf.Services;
using MediFormManager.Wpf.ViewModels;
using System.Windows;

namespace MediFormManager.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private readonly ApiClient _apiClient;

    private readonly FormService _formService;

    public MainWindow(ApiClient apiClient)
    {
        InitializeComponent();

        _apiClient = apiClient;
        _formService = new FormService(apiClient);

        var userService = new UserService(apiClient);
    
        _viewModel = new MainViewModel(userService);
        DataContext = _viewModel;

        MainContent.Content = new DashboardView();

      
    }

    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.LoadUsersAsync();
    }

    private void UsersButton_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new UsersView();
    }

    private void DashboardButton_Click(object sender, RoutedEventArgs e)
    {
        MainContent.Content = new DashboardView();
    }

    private void FormsButton_Click(object sender, RoutedEventArgs e)
    {
        ShowForms();
    }

    private void OnFormOpenRequested(Guid formVersionId, string mode)
    {
        if (formVersionId == Guid.Empty)
            return;

        if (mode is not ("designer" or "runtime"))
            return;

        var webView = new ReactWebView(
         _apiClient,
         formVersionId,
         mode);

        webView.BackRequested += ShowForms;

        MainContent.Content = webView;
    }

    private void ShowForms()
    {
        var formsView = new FormsView(_formService);

        formsView.FormOpenRequested += OnFormOpenRequested;

        MainContent.Content = formsView;
    }

    private async void LogoutButton_Click( object sender, RoutedEventArgs e)
    {
        var confirmation = MessageBox.Show(
            "Are you sure you want to log out?",
            "Logout",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirmation != MessageBoxResult.Yes)
            return;

        if (MainContent.Content is ReactWebView webView)
        {
            await webView.ClearSessionAsync();
        }

        _apiClient.ClearBearerToken();

        var loginWindow = new LoginWindow();
        loginWindow.Show();

        Close();
    }


}