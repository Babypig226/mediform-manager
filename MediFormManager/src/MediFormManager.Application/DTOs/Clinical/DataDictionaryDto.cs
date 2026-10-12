namespace MediFormManager.Application.DTOs.Clinical;

public class DataDictionaryDto
{
    public Guid Id { get; set; }
    public string DataKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
}