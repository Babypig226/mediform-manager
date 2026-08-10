using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class FormCategory : BaseEntity
    {
        public string CategoryCode { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Form> Forms { get; set; } = new List<Form>();
    }
}
