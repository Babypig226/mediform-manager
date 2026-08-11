using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Forms
{
    public class CreateFormRequest
    {
        public string FormCode { get; set; } = string.Empty;
        public string FormName { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
    }
}
