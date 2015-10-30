using System;
using System.Collections.Generic;
using System.Linq;

namespace Serialization
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var inputData = GetInputData();
            var outputData = GetOutputData(inputData);
            Console.WriteLine(outputData);
        }

        private static IReadOnlyList<string> GetInputData()
        {
            const int linesCount = 2;
            return Enumerable.Range(0, linesCount).Select(i => Console.ReadLine()).ToList();
        }

        private static string GetOutputData(IReadOnlyList<string> inputData)
        {
            string serializationType = inputData[0];
            string serializationData = inputData[1];

            return serializationType == "Json" ? GetSerializedOutputObject(new JsonSerializer(), serializationData) : 
                                                 GetSerializedOutputObject(new XmlSerializer(), serializationData);
        }

        private static string GetSerializedOutputObject(ISerializer serializer, string serializationData)
        {
            var inputObj = serializer.Deserialize<Input>(serializationData);
            
            var outputObj = new Output
            {
                SumResult = inputObj.Sums.Sum() * inputObj.K,
                MulResult = inputObj.Muls.Aggregate((acc, item) => acc * item),
                SortedInputs = inputObj.Sums.Union(inputObj.Muls.Select(item => (decimal)item)).OrderBy(item => item).ToArray()
            };

            return serializer.Serialize(outputObj);
        }
    }
}