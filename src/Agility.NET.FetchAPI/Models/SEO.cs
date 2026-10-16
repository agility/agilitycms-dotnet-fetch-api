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
        public SeoRobots Robots { get; set; }

        /// <summary>
        /// The page's social share (Open Graph) image: an absolute http(s) URL or a site-relative path. Pages and dynamic
        /// page list items; null when not set.
        /// </summary>
        public string OgImage { get; set; }
    }
}
