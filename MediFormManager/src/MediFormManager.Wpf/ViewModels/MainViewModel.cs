using CommunityToolkit.Mvvm.ComponentModel;
using MediFormManager.Wpf.Models;
using MediFormManager.Wpf.Services;
using System.Collections.ObjectModel;

namespace MediFormManager.Wpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly UserService _userService;

    public ObservableCollection<UserDto> Users { get; }
        = new();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public MainViewModel(UserService userService)
    {
        _userService = userService;
    }

    public async Task LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var users = await _userService.GetAllAsync();

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load users: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}