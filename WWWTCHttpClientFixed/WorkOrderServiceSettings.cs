
namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderServiceSettings
    {
        public string UserName { get; }
        public string Password { get; }
        public string BaseUrl { get; }

        public WorkOrderServiceSettings(string userName, string password, string baseUrl)
        {
            UserName = userName;
            Password = password;
            BaseUrl = baseUrl;
        }
    }
}
