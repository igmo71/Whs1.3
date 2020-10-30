namespace Whs.Server.Data
{
    public class HttpClientSettings
    {
        public const string HttpClient = "HttpClient";

        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseAddress { get; set; }
        public Service Service { get; set; }
    }
    public class Service
    {
        public string OData { get; set; }
        public string HttpService { get; set; }
    }

    public class HttpBitrixClientSettings
    {
        public const string HttpBitrixClient = "HttpBitrixClient";

        public string BaseAddress { get; set; }
        public BitrixService Service { get; set; }
    }
    public class BitrixService
    {
        public string Error { get; set; }
        public string Siren { get; set; }
    }
}
