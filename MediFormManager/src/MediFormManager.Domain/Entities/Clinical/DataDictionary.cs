using MediFormManager.Domain.Entities.Common;

namespace MediFormManager.Domain.Entities.Clinical;

public class DataDictionary : BaseEntity
{
    public string DataKey { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string DataType { get; set; } = "String";
}