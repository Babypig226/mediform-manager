using MediFormManager.Wpf.Services;
using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace MediFormManager.Wpf.Views
{
    /// <summary>
    /// ReactWebView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReactWebView : UserControl
    {
        private const string WebOrigin = "http://localhost:5173";
        private readonly ApiClient _apiClient;
        private readonly Guid _formVersionId;
        private readonly string _mode;
        public event Action? BackRequested;
        public ReactWebView(ApiClient apiClient, Guid formVersionId, string mode)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _formVersionId = formVersionId;
            _mode = mode;

            Loaded += ReactWebView_Loaded;
        }

        private async void ReactWebView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Loaded -= ReactWebView_Loaded;

            try
            {
                await WebBrowser.EnsureCoreWebView2Async();
                var core = WebBrowser.CoreWebView2;
                core.NavigationStarting += (_, args) =>
                {
                    if (!IsTrustedOrigin(args.Uri))
                    {
                        args.Cancel = true;
                    }
                };

                core.WebMessageReceived += WebMessageReceived;

                var url =
                            $"{WebOrigin}/" +
                            $"?formVersionId={_formVersionId:D}" +
                            $"&mode={Uri.EscapeDataString(_mode)}";

                core.Navigate(url);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Failed to initialize WebView2:\n{ex.Message}");
            }
        }

        private static bool IsTrustedOrigin(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            return string.Equals(
                uri.GetLeftPart(UriPartial.Authority),
                WebOrigin,
                StringComparison.OrdinalIgnoreCase);
        }

        private void WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (!IsTrustedOrigin(e.Source))
                return;

            try
            {
                using var message = JsonDocument.Parse(e.WebMessageAsJson);

                if (!message.RootElement.TryGetProperty("type", out var type))
                    return;

                if (type.GetString() != "MEDIFORM_READY")
                    return;

                var token = _apiClient.AccessToken;

                if (string.IsNullOrWhiteSpace(token))
                {
                    MessageBox.Show("WPF authentication token is missing.");
                    return;
                }

                var payload = JsonSerializer.Serialize(new
                {
                    type = "MEDIFORM_INIT",
                    accessToken = token,
                    formVersionId = _formVersionId.ToString("D"),
                    mode = _mode
                });

                WebBrowser.CoreWebView2.PostWebMessageAsJson(payload);
            }
            catch (JsonException)
            {
                // Ignore malformed messages.
            }
        }

        private void BackToForms_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke();
        }

        public async Task ClearSessionAsync()
        {
            if (WebBrowser.CoreWebView2 is null)
                return;

            try
            {
                await WebBrowser.CoreWebView2.ExecuteScriptAsync(
                    "sessionStorage.removeItem('accessToken');");

                WebBrowser.CoreWebView2.Navigate("about:blank");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Failed to clear WebView2 session: {ex.Message}");
            }
        }
    }
}
