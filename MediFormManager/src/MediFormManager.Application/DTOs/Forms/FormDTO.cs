using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Forms
{
    public class FormDto
    {
        public Guid Id { get; set; }

        public string FormCode { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
