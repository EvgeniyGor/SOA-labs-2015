using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    public class HttpClientBase : IHttpClient, IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        protected HttpClientBase(string address, int port)
        {
            var uri = new Uri($"{address}:{port}");

            _httpClient = new System.Net.Http.HttpClient
            {
                BaseAddress = uri
            };
        }

        public async Task<HttpResponseMessage> InvokeGetRequestAsync(string requestUri, Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return await _httpClient.GetAsync(requestUri);
            }

            var requestUriWithParams = new StringBuilder(requestUri).Append('?');

            foreach (var parameter in parameters.Select(i => $"{i.Key}={i.Value}").ToArray())
            {
                requestUriWithParams.Append(parameter);
            }

            return await _httpClient.GetAsync(requestUriWithParams.ToString());
        }

        public async Task<HttpResponseMessage> InvokePostRequestAsync(string requestUri, string requestBody)
        {
            var content = new StringContent(requestBody);
            return await _httpClient.PostAsync(requestUri, content);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}