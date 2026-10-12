namespace MediFormManager.Wpf.Models;

public class FormDto
{
    public Guid Id { get; set; }

    public string FormCode { get; set; } = string.Empty;

    public string FormName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}