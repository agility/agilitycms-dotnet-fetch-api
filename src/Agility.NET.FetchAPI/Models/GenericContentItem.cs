namespace Agility.NET.FetchAPI.Models
{
    public class GenericContentItem<T>
    {
        public int ContentID { get; set; }
        public Properties Properties { get; set; }
        public T Fields { get; set; }
        public SEO SEO { get; set; }
    }
}
