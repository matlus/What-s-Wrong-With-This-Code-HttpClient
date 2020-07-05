using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal sealed class WorkOrderServiceGateway : IDisposable
    {
        private bool _disposed;
        private HttpClient _httpClient;
        private readonly WorkOrderServiceSettings _workOrderServiceSettings;

        public WorkOrderServiceGateway(WorkOrderServiceSettings workOrderServiceSettings)
        {
            _workOrderServiceSettings = workOrderServiceSettings;
            _httpClient = CreateHttpClient(_workOrderServiceSettings.BaseUrl);
        }

        private HttpClient CreateHttpClient(string baseUrl)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl),
            };

            return httpClient;
        }

        public async Task<string> CreateWorkOrder(WorkOrderForCreate workOrderForCreate)
        {
            var workOrderCreateRequest = MapperWorkOrder.MapToWorkOrderCreateRequest(workOrderForCreate);

            var accessToken = await Login(_httpClient, _workOrderServiceSettings.UserName, _workOrderServiceSettings.Password).ConfigureAwait(false);

            var authenticationHeaderValue = new AuthenticationHeaderValue("AR-JWT", accessToken);
            using var httpResponseMessage = await PostJsonAsync(_httpClient, url: string.Empty, workOrderCreateRequest, authenticationHeaderValue).ConfigureAwait(false);
            var workOrderId = await httpResponseMessage.Content.ReadAsAsync<string>().ConfigureAwait(false);
            await Logout(_httpClient, accessToken).ConfigureAwait(false);

            return workOrderId;
        }

        private static async Task<string> Login(HttpClient httpClient, string userName, string password)
        {
            var nameValueCollection = new NameValueCollection { { "username", userName }, { "passowrd", password } };
            using var httpResponseMessage = await PostUrlEncodedAsync(httpClient, "login", nameValueCollection).ConfigureAwait(false);
            await EnsureSuccess(httpResponseMessage.StatusCode, httpResponseMessage.Content).ConfigureAwait(false);
            return await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private static async Task Logout(HttpClient httpClient, string token)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "logout");
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            await EnsureSuccess(httpResponseMessage.StatusCode, httpResponseMessage.Content).ConfigureAwait(false);
        }

        private static async Task EnsureSuccess(HttpStatusCode statusCode, HttpContent content)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                return;
            }

            var httpContent = await content.ReadAsStringAsync().ConfigureAwait(false);

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new WorkOrderServiceNotFoundException("The Remedy Service call resulted in a Not Found Status Code");
                case HttpStatusCode.InternalServerError:
                    {
                        if (httpContent.Contains("User is currently connected from another machine or incompatible session.", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new WorkOrderServiceIncompatibleSessionException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
                        }
                        throw new WorkOrderServiceInternalServerException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
                    }
                default:
                    throw new WorkOrderServiceUnexpectedException($"The Remedy Service call resulted in a status code of: {statusCode}, with body: {httpContent}");
            }
        }

        private static async Task<HttpResponseMessage> PostUrlEncodedAsync(HttpClient httpClient, string url, NameValueCollection nameValueCollection)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequestMessage.Content = GetUrlEncodedFormData(nameValueCollection);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            await EnsureSuccess(httpResponseMessage.StatusCode, httpResponseMessage.Content).ConfigureAwait(false);
            return httpResponseMessage;
        }

        private static async Task<HttpResponseMessage> PostJsonAsync(HttpClient httpClient, string url, object contentData, AuthenticationHeaderValue authenticationHeaderValue)
        {
            using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpRequestMessage.Headers.Authorization = authenticationHeaderValue;
            httpRequestMessage.Content = CreateJsonStreamContent(contentData);
             var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);
            await EnsureSuccess(httpResponseMessage.StatusCode, httpResponseMessage.Content).ConfigureAwait(false);
            return httpResponseMessage;
        }

        private static StreamContent CreateJsonStreamContent(object contentData)
        {
            var memoryStream = new MemoryStream();
            SerializeJsonIntoStream(memoryStream, contentData);
            memoryStream.Position = 0;
            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return streamContent;
        }

        private static void SerializeJsonIntoStream(Stream stream, object value)
        {
            using var utf8JsonWriter = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = false });
            JsonSerializer.Serialize(utf8JsonWriter, value);
            utf8JsonWriter.Flush();
        }

        private static FormUrlEncodedContent GetUrlEncodedFormData(NameValueCollection nameValueCollection)
        {
            var formDataKeyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (var key in nameValueCollection.AllKeys)
            {
                formDataKeyValuePairs.Add(new KeyValuePair<string, string>(key, nameValueCollection[key]));
            }

            return new FormUrlEncodedContent(formDataKeyValuePairs);
        }

        [ExcludeFromCodeCoverage]
        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed && _httpClient != null)
            {
                var localHttpClient = _httpClient;
                localHttpClient.Dispose();
                _httpClient = null;                
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
