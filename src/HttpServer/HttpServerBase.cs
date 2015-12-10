using System.Text;
using System.IO;
using System.Net;

namespace HttpServer
{
    public abstract class HttpServerBase : IHttpServer
    {
        private static HttpListener _listener;
        private static HttpListenerContext _context;

        protected HttpServerBase(string address, int port)
        {
            var prefix = $"{address}:{port}/";
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix);
        }

        public void Start()
        {
            _listener.Start();

            while (_listener.IsListening)
            {
                try
                {
                    _context = _listener.GetContext();
                    ProcessRequest(_context);
                }
                catch { }
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }

        protected abstract void ProcessRequest(HttpListenerContext context);

        protected static string GetContentFromRequestBody()
        {
            var requestBytes = Encoding.UTF8.GetBytes(_context.Request.ToString());
            var inputStream = new MemoryStream(requestBytes);

            string result;

            using (var reader = new StreamReader(inputStream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        protected static void SendResponse(HttpStatusCode statusCode, string content = "")
        {
            _context.Response.StatusCode = (int)statusCode;
            _context.Response.ContentEncoding = Encoding.UTF8;
            _context.Response.ContentLength64 = content.Length;
            _context.Response.OutputStream.Write(Encoding.UTF8.GetBytes(content), 0, content.Length);
        }
    }
}
