using MediFormManager.Wpf.Services;
using MediFormManager.Wpf.ViewModels;
using System.Windows;

namespace MediFormManager.Wpf.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(ApiClient apiClient)
    {
        InitializeComponent();

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

    private void DashboardButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        MainContent.Content = new DashboardView();
    }
}