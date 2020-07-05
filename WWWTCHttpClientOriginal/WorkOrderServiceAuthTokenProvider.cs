using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WWWTCHttpClientOriginal
{
    public class WorkOrderServiceAuthTokenProvider
    {
        public virtual async Task<string> GetAccessTokenAsync(WorkOrderSettings settings)
        {
            var credentials = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", settings.UserName),
                new KeyValuePair<string, string>("password", settings.Password)
            };
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, settings.BaseUrl + "/login")
            {
                Content = new FormUrlEncodedContent(credentials)
            };
            var response = await client.SendAsync(req);
            var contents = await response.Content.ReadAsStringAsync();

            return contents;
        }

        public virtual async Task Logout(string token, string logoutUrl)
        {
            var req = new HttpRequestMessage();
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.RequestUri = new Uri(logoutUrl);
            req.Method = HttpMethod.Post;
            var client = new HttpClient();
            await client.SendAsync(req);
        }

        public async Task<string> CreateWorkOrder(WorkOrderCreateRequest workOrderCreateRequest, WorkOrderSettings workOrderSettings)
        {
            var req = new HttpRequestMessage();

            string access = await GetAccessTokenAsync(workOrderSettings);

            req.Headers.Authorization = new AuthenticationHeaderValue("AR-JWT", access);
            req.RequestUri = new Uri(workOrderSettings.BaseUrl);
            req.Method = HttpMethod.Post;

            string createRequest = JsonConvert.SerializeObject(workOrderCreateRequest);
            req.Content = new StringContent(createRequest, Encoding.UTF8, "application/json");
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(req);

            await Logout(access, workOrderSettings.BaseUrl + "/logout");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
