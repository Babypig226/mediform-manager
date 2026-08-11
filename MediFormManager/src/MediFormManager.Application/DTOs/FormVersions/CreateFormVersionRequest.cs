
namespace MediFormManager.Application.DTOs.FormVersions
{
    public class CreateFormVersionRequest
    {
        public Guid FormId { get; set; }

        public int Version { get; set; }
    }
}
