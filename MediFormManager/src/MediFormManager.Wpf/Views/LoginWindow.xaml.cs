using MediFormManager.Wpf.Services;
using MediFormManager.Wpf.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MediFormManager.Wpf.Views
{
    /// <summary>
    /// LoginWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginWindow : Window
    {

        private readonly ApiClient _apiClient;
        public LoginWindow()
        {
            InitializeComponent();

            _apiClient = new ApiClient();
            var authService = new AuthService(_apiClient);
            var viewModel= new LoginViewModel(authService);

            viewModel.LoginSucceeded += OnLoginSucceeded;

            DataContext = viewModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteLoginAsync();
        }

        private async Task ExecuteLoginAsync()
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = PasswordInput.Password;

                if (viewModel.LoginCommand.CanExecute(null))
                {
                    await viewModel.LoginCommand.ExecuteAsync(null);
                }
            }
        }

        private async void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ExecuteLoginAsync();
            }
        }

        private void OnLoginSucceeded()
        {
            var mainWindow = new MainWindow(_apiClient);
            mainWindow.Show();

            Close();
        }
    }
}
