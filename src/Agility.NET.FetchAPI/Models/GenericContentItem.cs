namespace Agility.NET.FetchAPI.Models
{
    public class GenericContentItem<T>
    {
        public int ContentID { get; set; }
        public Properties Properties { get; set; }
        public T Fields { get; set; }
        public SEO SEO { get; set; }
        /// <summary>
        /// The item's own GEO / AEO properties. Only items in a list that backs a dynamic page carry them; null when
        /// the item has none.
        /// </summary>
        public Geo Geo { get; set; }
    }
}
