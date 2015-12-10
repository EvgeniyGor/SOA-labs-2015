using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClient
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> InvokeGetRequestAsync(string requestUri, Dictionary<string, string> parameters);
        Task<HttpResponseMessage> InvokePostRequestAsync(string requestUri, string requestBody);
    }
}