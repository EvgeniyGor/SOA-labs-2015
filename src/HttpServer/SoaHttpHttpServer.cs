using System;
using System.Linq;
using System.Net;
using Serialization;

namespace HttpServer
{
    public class SoaHttpHttpServer : HttpServerBase, ISoaHttpServer
    {
        private readonly JsonSerializer _serializer;
        private Input _inputData;

        private SoaHttpHttpServer(string address, int port) : base(address, port)
        {
            _serializer = new JsonSerializer();
        }

        public static SoaHttpHttpServer Create(string address, int portNumber)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            return new SoaHttpHttpServer(address, portNumber);
        }

        public void Ping()
        {
            SendResponse(HttpStatusCode.OK);
        }

        public void PostInputData()
        {
            var requestContent = GetContentFromRequestBody();

            try
            {
                _inputData = _serializer.Deserialize<Input>(requestContent);
                SendResponse(HttpStatusCode.OK);
            }
            catch
            {
                SendResponse(HttpStatusCode.BadRequest);
            }
        }

        public void GetAnswer()
        {
            if (_inputData == null)
            {
                SendResponse(HttpStatusCode.BadRequest);
                return;
            }

            var responseContent = _serializer.Serialize(GetAnswerObject());
            SendResponse(HttpStatusCode.OK, responseContent);
        }

        protected override void ProcessRequest(HttpListenerContext context)
        {
            var rawUrl = context.Request.RawUrl;
            var methodName = GetMethodName(rawUrl);
            var type = typeof (SoaHttpHttpServer);
            var methodInfo = type.GetMethod(methodName);
            methodInfo?.Invoke(Activator.CreateInstance(type, null), null);
        }

        private static string GetMethodName(string rawUrl)
        {
            var trimmedRarUrl = rawUrl.Trim('/');
            var indexOf = trimmedRarUrl.IndexOf("?", StringComparison.Ordinal);
            return indexOf != -1 ? trimmedRarUrl.Substring(0, indexOf) : rawUrl;
        }

        private Output GetAnswerObject()
        {
            return new Output
            {
                SumResult = _inputData.Sums.Sum() * _inputData.K,
                MulResult = _inputData.Muls.Aggregate((acc, item) => acc * item),
                SortedInputs = _inputData.Sums.Union(_inputData.Muls.Select(item => (decimal)item)).OrderBy(item => item).ToArray()
            };
        }
    }
}
