namespace Agility.NET.FetchAPI.Helpers
{
    public class AppSettings
    {
        public string InstanceGUID { get; set; }
        public string SecurityKey { get; set; }
        public string WebsiteName { get; set; }
        public string FetchAPIKey { get; set; }
        public string PreviewAPIKey { get; set; }
        public string Locales { get; set; }
        public string ChannelName { get; set; }
        public int CacheInMinutes { get; set; }
    }
}