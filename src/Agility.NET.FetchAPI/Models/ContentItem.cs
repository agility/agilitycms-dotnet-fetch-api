namespace Agility.NET.FetchAPI.Models
{
    public class ContentItem
    {
        public int ContentID { get; set; }
        public Properties Properties { get; set; }
        public dynamic Fields { get; set; }
        public SEO SEO { get; set; }
        /// <summary>
        /// The item's own GEO / AEO properties. Only items in a list that backs a dynamic page carry them; null when
        /// the item has none.
        /// </summary>
        public Geo Geo { get; set; }
    }
}
