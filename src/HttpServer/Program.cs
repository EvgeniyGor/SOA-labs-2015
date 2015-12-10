using System;

namespace HttpServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string address = "http://127.0.0.1";
            int portNumber;

            if (!int.TryParse(Console.ReadLine(), out portNumber))
            {
                return;
            }

            var server = SoaHttpHttpServer.Create(address, portNumber);
            server.Start();
        }
    }
}
