using System;
using System.Linq;
using Serialization;

namespace HttpClient
{
    public class Program
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        public static void Main(string[] args)
        {
            const string address = "http://127.0.0.1";
            int portNumber;

            if (!int.TryParse(Console.ReadLine(), out portNumber))
            {
                return;
            }

            using (var webHttpClient = SoaSoaHttpClient.Create(address, portNumber))
            {
                if (!webHttpClient.Ping())
                {
                    return;
                }

                var inputData = webHttpClient.GetInputData();
                var outputData = GetSerializedOutputObject(inputData);
                webHttpClient.WriteAnswer(outputData);
            }
        }

        private static string GetSerializedOutputObject(string inputData)
        {
            var inputObj = _serializer.Deserialize<Input>(inputData);

            var outputObj = new Output
            {
                SumResult = inputObj.Sums.Sum() * inputObj.K,
                MulResult = inputObj.Muls.Aggregate((acc, item) => acc * item),
                SortedInputs = inputObj.Sums.Union(inputObj.Muls.Select(item => (decimal)item)).OrderBy(item => item).ToArray()
            };

            return _serializer.Serialize(outputObj);
        }
    }
}