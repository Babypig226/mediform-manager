

namespace MediFormManager.Application.DTOs.FormVersions
{
    public class FormVersionDto
    {
        public Guid Id { get; set; }

        public Guid FormId { get; set; }

        public int Version { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
