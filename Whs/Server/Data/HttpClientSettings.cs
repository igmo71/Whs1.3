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


    public class HttpNotficationClientSettings
    {
        public const string HttpNotficationClient = "HttpNotficationClient";

        public string BaseAddress { get; set; }
    }
}
