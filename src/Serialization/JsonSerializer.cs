using Newtonsoft.Json;

namespace Serialization
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T obj) where T : class
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string serializedObj) where T : class
        {
            return JsonConvert.DeserializeObject<T>(serializedObj);
        }
    }
}