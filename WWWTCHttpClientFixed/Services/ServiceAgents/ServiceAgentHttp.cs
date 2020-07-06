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

        public async Task<TResult>PostAsJsonAsync<T, TResult>(string requestUrl, T value, Func<HttpResponseMessage, Task> exceptionTranslatorDelegate)
        {
            using var httpResponseMessage = await _httpClient.PostAsync(requestUrl, value, _mediaTypeFormatterCollection.JsonFormatter).ConfigureAwait(false);
            await exceptionTranslatorDelegate(httpResponseMessage);
            return await httpResponseMessage.Content.ReadAsAsync<TResult>();
        }

        public Task<TResult> PostAsJsonAsync<T, TResult>(string requestUrl, T value, string authenticationScheme, string authenticationParameter, Func<HttpResponseMessage, Task> exceptionTranslatorDelegate)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationScheme, authenticationParameter);
            return PostAsJsonAsync<T, TResult>(requestUrl, value, exceptionTranslatorDelegate);
        }

        public async Task<string> PostFormUrlEncodedAsync(string requestUrl, NameValueCollection nameValueCollection, Func<HttpResponseMessage, Task> exceptionTranslatorDelegate)
        {
            var formUrlEncodedContent = GetUrlEncodedFormData(nameValueCollection);
            using var httpResponseMessage = await _httpClient.PostAsync(requestUrl, formUrlEncodedContent).ConfigureAwait(false);
            await exceptionTranslatorDelegate(httpResponseMessage);
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string requestUrl, string content, string mediaType, Func<HttpResponseMessage, Task> exceptionTranslatorDelegate)
        {
            var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
            using var httpResponseMessage = await _httpClient.PostAsync(requestUrl, httpContent);
            await exceptionTranslatorDelegate(httpResponseMessage);
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
