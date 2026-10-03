using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities.Orders
{
    public class OrderFormMapping : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid FormId { get; set; }
        public MedicalOrder? Order { get; set; }
        public Form? Form { get; set; }
    }
}
