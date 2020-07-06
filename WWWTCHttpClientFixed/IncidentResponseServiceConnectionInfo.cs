
namespace WWWTCHttpClientFixed
{
    internal sealed class IncidentResponseServiceConnectionInfo
    {
        public string Username { get; }
        public string Password { get; }

        public string BaseUrl { get; }

        public IncidentResponseServiceConnectionInfo(string usename, string password, string baseUrl)
        {
            Username = usename;
            Password = password;
            BaseUrl = baseUrl;
        }

    }
}
