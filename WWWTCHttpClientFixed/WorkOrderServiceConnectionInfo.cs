
namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderServiceConnectionInfo
    {
        public string UserName { get; }
        public string Password { get; }
        public string BaseUrl { get; }

        public WorkOrderServiceConnectionInfo(string userName, string password, string baseUrl)
        {
            UserName = userName;
            Password = password;
            BaseUrl = baseUrl;
        }
    }
}
