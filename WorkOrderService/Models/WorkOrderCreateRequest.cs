using System;

namespace WorkOrderService.Models
{
    public sealed class WorkOrderCreateRequest
    {
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomerEmail { get; set; }
    }
}
