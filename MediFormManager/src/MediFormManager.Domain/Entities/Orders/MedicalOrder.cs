using MediFormManager.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities.Orders
{
    public class MedicalOrder : BaseEntity
    {
        public string OrderCode { get; set; } = string.Empty;

        public string OrderName { get; set; } = string.Empty;
    
    }
}
