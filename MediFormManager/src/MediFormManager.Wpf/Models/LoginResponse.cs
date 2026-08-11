namespace MediFormManager.Wpf.Models;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public string LoginId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}