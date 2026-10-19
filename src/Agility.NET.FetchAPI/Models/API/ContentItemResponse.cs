

#nullable enable

namespace Agility.NET.FetchAPI.Models.API
{
    public class ContentItemResponse<T>
    {
        public int ContentID { get; set; }
        public Properties? Properties { get; set; }
        public SEO? SEO { get; set; }
        /// <summary>
        /// The item's own GEO / AEO properties. Only items in a list that backs a dynamic page carry them; null when
        /// the item has none.
        /// </summary>
        public GeoProperties? Geo { get; set; }
        public T? Fields { get; set; }

    }
}
