using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediFormManager.Wpf.Services;

namespace MediFormManager.Wpf.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _authService;

    public event Action? LoginSucceeded;

    [ObservableProperty]
    private string loginId = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isLoading;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(LoginId) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter your ID and password.";
            return;
        }

        try
        {
            IsLoading = true;

            var result = await _authService.LoginAsync(LoginId, Password);

            if (result is null)
            {
                ErrorMessage = "Invalid ID or password.";
                return;
            }

            LoginSucceeded?.Invoke();


        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

   
}