using System;

namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderCreateRequest
    {
        public string Description { get; }
        public DateTime CreationDate { get; }
        public string CustomerEmail { get; }

        public WorkOrderCreateRequest(string description, DateTime creationDate, string customerEmail)
        {
            Description = description;
            CreationDate = creationDate;
            CustomerEmail = customerEmail;
        }
    }
}
