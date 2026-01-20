namespace Agility.NET.FetchAPI.Models
{
    public class ContentItem
    {
        public int ContentID { get; set; }
        public Properties Properties { get; set; }
        public dynamic Fields { get; set; }
        public SEO SEO { get; set; }
    }
}
