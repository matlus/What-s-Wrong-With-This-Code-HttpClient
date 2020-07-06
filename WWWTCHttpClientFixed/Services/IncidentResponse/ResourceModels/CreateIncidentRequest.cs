
namespace WWWTCHttpClientFixed.Services.IncidentResponse.ResourceModels
{
    internal sealed class CreateIncidentRequest
    {
        public string Title { get; }
        public string Verbiage { get; }
        public string MeetingLink { get; }
        public string MeetingPhone { get; }
        public string MeetingCode { get; }
        public string IncidentNumber { get; }
        public string RecipientGroup { get; }

        public CreateIncidentRequest(string title, string verbiage, string meetingLink, string meetingPhone, string meetingCode, string incidentNumber, string recipientGroup)
        {
            Title = title;
            Verbiage = verbiage;
            MeetingLink = meetingLink;
            MeetingPhone = meetingPhone;
            MeetingCode = meetingCode;
            IncidentNumber = incidentNumber;
            RecipientGroup = recipientGroup;
        }
    }
}
