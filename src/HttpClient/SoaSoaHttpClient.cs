using System;

namespace HttpClient
{
    public class SoaSoaHttpClient : HttpClientBase, ISoaHttpClient
    {
        private SoaSoaHttpClient(string address, int port) : base(address, port)
        {
        }

        public static SoaSoaHttpClient Create(string address, int portNumber)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            return new SoaSoaHttpClient(address, portNumber);
        }

        public bool Ping()
        {
            var response = InvokeGetRequestAsync("/Ping", null);
            return response.Result.IsSuccessStatusCode;
        }

        public string GetInputData()
        {
            var response = InvokeGetRequestAsync("/GetInputData", null);
            return response.Result.Content.ToString();
        }

        public bool WriteAnswer(string answer)
        {
            var response = InvokePostRequestAsync("/WriteAnswer", answer);
            return response.Result.IsSuccessStatusCode;
        }
    }
}