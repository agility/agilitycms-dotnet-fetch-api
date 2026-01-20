namespace Agility.NET.FetchAPI.Models
{
    public class UrlRedirection
    {
        public int Id { get; set; }
        public string OriginUrl { get; set; }
        public string DestinationUrl { get; set; }
        public int StatusCode { get; set; }
    }
}
