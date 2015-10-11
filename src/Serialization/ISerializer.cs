namespace Serialization
{
    public interface ISerializer
    {
        string Serialize<T>(T obj) where T: class;
        T Deserialize<T>(string serializedObj) where T : class;
    }
}