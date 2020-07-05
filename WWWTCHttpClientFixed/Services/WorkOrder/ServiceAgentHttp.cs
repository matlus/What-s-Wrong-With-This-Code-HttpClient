using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WWWTCHttpClientFixed
{
    internal sealed class ServiceAgentHttp : IDisposable
    {
        private bool _disposed;
        private HttpClient _httpClient;
        private readonly MediaTypeFormatterCollection _mediaTypeFormatterCollection = new MediaTypeFormatterCollection();

        public ServiceAgentHttp(string baseUrl)
        {
            _httpClient = CreateHttpClient(baseUrl);
        }

        private HttpClient CreateHttpClient(string baseUrl)
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl),
            };

            return httpClient;
        }

        public async Task<TResult>PostAsJsonAsync<T, TResult>(string requestUrl, T value)
        {
            var httpResponseMessage = await _httpClient.PostAsync(requestUrl, value, _mediaTypeFormatterCollection.JsonFormatter).ConfigureAwait(false);            
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsAsync<TResult>();
        }

        public Task<TResult> PostAsJsonAsync<T, TResult>(string requestUrl, T value, string authenticationScheme, string authenticationParameter)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationScheme, authenticationParameter);
            return PostAsJsonAsync<T, TResult>(requestUrl, value);
        }

        public async Task<string> PostFormUrlEncodedAsync(string requestUrl, NameValueCollection nameValueCollection)
        {
            var formUrlEncodedContent = GetUrlEncodedFormData(nameValueCollection);
            var httpResponseMessage = await _httpClient.PostAsync(requestUrl, formUrlEncodedContent).ConfigureAwait(false);
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string requestUrl, string content, string mediaType)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            var httpResponseMessage = await _httpClient.PostAsync(requestUrl, httpContent);
            httpResponseMessage.EnsureSuccessStatusCode();
            return await httpResponseMessage.Content.ReadAsStringAsync();
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
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
