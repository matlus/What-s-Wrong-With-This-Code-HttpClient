
namespace WWWTCHttpClientFixed
{
    internal sealed class IncidentForCreate
    {
        public string Title { get; }
        public string Verbiage { get; }
        public string MeetingLink { get; }
        public string MeetingPhone { get; }
        public string MeetingCode { get; }
        public string IncidentNumber { get; }
        public string RecipientGroup { get; }

        public IncidentForCreate(string title, string verbiage, string meetingLink, string meetingPhone, string meetingCode, string incidentNumber, string recipientGroup)
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
