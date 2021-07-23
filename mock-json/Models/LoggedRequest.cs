namespace mock_json.Models
{
    public class LoggedRequest
    {
        public string IpRequested { get; init; }
        public string RequestTo { get; init; }
        public string HttpMethod { get; init; }
        public string Path { get; init; }
        public string QueryParams { get; init; }
    }
}
