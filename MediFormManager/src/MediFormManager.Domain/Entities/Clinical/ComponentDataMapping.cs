using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Entities.Forms;

namespace MediFormManager.Domain.Entities.Clinical;

public class ComponentDataMapping : BaseEntity
{
    public Guid FormComponentId { get; set; }

    public Guid DataDictionaryId { get; set; }

    public FormComponent? FormComponent { get; set; }

    public DataDictionary? DataDictionary { get; set; }
}