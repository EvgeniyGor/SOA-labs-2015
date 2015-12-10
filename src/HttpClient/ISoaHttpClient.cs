namespace HttpClient
{
    public interface ISoaHttpClient
    {
        bool Ping();
        string GetInputData();
        bool WriteAnswer(string answer);
    }
}