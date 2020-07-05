
using System;

namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderForCreate
    {
        public string Title { get; }
        public string Description { get; }
        public DateTime DateTime { get; }
        public string Email { get; set; }
        public WorkOrderForCreate(string title, string description, DateTime dateTime, string email)
        {
            Title = title;
            Description = description;
            DateTime = dateTime;
            Email = email;
        }
    }
}
