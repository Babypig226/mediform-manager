using MediFormManager.Wpf.Services;
using MediFormManager.Wpf.ViewModels;
using MediFormManager.Wpf.Models;
using System.Windows;
using System.Windows.Controls;

namespace MediFormManager.Wpf.Views;

public partial class FormsView : UserControl
{
    private readonly FormsViewModel _viewModel;
    public event Action<Guid, string>? FormOpenRequested;

    public FormsView(FormService formService)
    {
        InitializeComponent();

        _viewModel = new FormsViewModel(formService);
        DataContext = _viewModel;

        Loaded += FormsView_Loaded;
    }

    private async void FormsView_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= FormsView_Loaded;
        await _viewModel.LoadFormsAsync();
    }

    private void FormList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_viewModel.SelectedForm is null)
        {
            FormLibraryColumn.Width = new GridLength(1, GridUnitType.Star);
            VersionColumn.Width = new GridLength(0);
            VersionPanel.Visibility = Visibility.Collapsed;
        }
        else
        {
            FormLibraryColumn.Width = new GridLength(0.65, GridUnitType.Star);
            VersionColumn.Width = new GridLength(0.35, GridUnitType.Star);
            VersionPanel.Visibility = Visibility.Visible;
        }
    }

    private void CloseVersions_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.SelectedForm = null;
    }

    private void EditVersion_Click(object sender, RoutedEventArgs e)
    {
       if (sender is not Button button || button.DataContext is not FormVersionDto version)
            return;

        if (version.Status != "Draft")
            return;

        FormOpenRequested?.Invoke(version.Id, "designer");
    }

    private void PreviewVersion_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.DataContext is not FormVersionDto version)
            return;

        FormOpenRequested?.Invoke(version.Id, "runtime");
    }

    
}