using MediFormManager.Wpf.Services;
using MediFormManager.Wpf.ViewModels;
using System.Windows;

namespace MediFormManager.Wpf;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(ApiClient apiClient)
    {
        InitializeComponent();

        var userService = new UserService(apiClient);

        _viewModel = new MainViewModel(userService);
        DataContext = _viewModel;

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await _viewModel.LoadUsersAsync();
    }
}