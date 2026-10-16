namespace Agility.NET.FetchAPI.Models
{
    public class SEO
    {
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaHTML { get; set; }
        public string MenuVisible { get; set; }
        public string SitemapVisible { get; set; }

        /// <summary>The page's &lt;title&gt;, when it differs from the page title. Pages and dynamic page list items; null when not set.</summary>
        public string Title { get; set; }

        /// <summary>The page's canonical URL override. Pages and dynamic page list items; null when not set.</summary>
        public string CanonicalUrl { get; set; }

        /// <summary>The page's robots directives. Pages and dynamic page list items; null when not set.</summary>
        public Robots Robots { get; set; }

        /// <summary>The URL of the page's social share (Open Graph) image. Pages and dynamic page list items; null when not set.</summary>
        public string OgImage { get; set; }
    }
}
