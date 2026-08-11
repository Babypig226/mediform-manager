using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Forms
{
    public class UpdateFormRequest
    {
        public string FormName { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}
