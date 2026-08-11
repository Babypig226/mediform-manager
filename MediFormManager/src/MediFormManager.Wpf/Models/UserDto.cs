namespace MediFormManager.Wpf.Models;

public class UserDto
{
    public Guid Id { get; set; }

    public string LoginId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public Guid JobPositionId { get; set; }
    public string JobPositionName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}