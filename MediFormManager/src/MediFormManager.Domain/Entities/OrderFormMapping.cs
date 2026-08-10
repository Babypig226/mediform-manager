using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class OrderFormMapping : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid FormId { get; set; }
        public MedicalOrder? Order { get; set; }
        public Form? Form { get; set; }
    }
}
