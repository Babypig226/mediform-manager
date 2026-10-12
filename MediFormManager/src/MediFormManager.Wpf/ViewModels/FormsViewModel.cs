using CommunityToolkit.Mvvm.ComponentModel;
using MediFormManager.Wpf.Models;
using MediFormManager.Wpf.Services;
using System.Collections.ObjectModel;

namespace MediFormManager.Wpf.ViewModels;

public partial class FormsViewModel : ObservableObject
{
    private readonly FormService _formService;

    public ObservableCollection<FormDto> Forms { get; } = new();

    public ObservableCollection<FormVersionDto> Versions { get; } = new();

    [ObservableProperty]
    private FormDto? selectedForm;

    [ObservableProperty]
    private FormVersionDto? selectedVersion;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string? errorMessage;

    public FormsViewModel(FormService formService)
    {
        _formService = formService;
    }

    public async Task LoadFormsAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var forms = await _formService.GetAllAsync();

            Forms.Clear();

            foreach (var form in forms)
                Forms.Add(form);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    partial void OnSelectedFormChanged(FormDto? value)
    {
        SelectedVersion = null;
        Versions.Clear();

        if (value is not null)
            _ = LoadVersionsAsync(value.Id);
    }

    private async Task LoadVersionsAsync(Guid formId)
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var versions = await _formService.GetVersionsAsync(formId);

            // 사용자가 로딩 중 다른 서식을 선택했으면
            // 이전 요청 결과는 버립니다.
            if (SelectedForm?.Id != formId)
                return;

            Versions.Clear();

            foreach (var version in versions.OrderByDescending(v => v.Version))
                Versions.Add(version);
        }
        catch (Exception ex)
        {
            if (SelectedForm?.Id == formId)
                ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }
}