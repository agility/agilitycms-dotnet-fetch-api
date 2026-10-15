using System.Text.Json;

namespace Agility.NET.FetchAPI.Models
{
    /// <summary>
    /// A page's GEO / AEO (generative and answer engine optimization) properties. The API leaves out any property
    /// that isn't set, so each is null when not set, and the page's <c>Geo</c> is null when it has none.
    /// </summary>
    public class Geo
    {
        /// <summary>
        /// The schema.org page type: <c>WebPage</c>, <c>AboutPage</c>, <c>ContactPage</c>, <c>CollectionPage</c>,
        /// <c>FAQPage</c>, <c>ItemPage</c> or <c>ProfilePage</c>. Treat null as <c>WebPage</c>.
        /// </summary>
        public string SchemaType { get; set; }

        /// <summary>A short, answer-first plain-text summary of the page.</summary>
        public string Summary { get; set; }

        /// <summary>
        /// Editor-supplied JSON-LD, as raw JSON: an object (which may hold an <c>@graph</c>) or an array of objects.
        /// </summary>
        public JsonElement? StructuredData { get; set; }

        /// <summary>The page's AI-engine settings.</summary>
        public GeoAi Ai { get; set; }
    }

    /// <summary>A page's AI-engine settings. Each is null when not set; treat null as <c>inherit</c>.</summary>
    public class GeoAi
    {
        /// <summary>Whether the page is listed in llms.txt: <c>inherit</c> (follow sitemap visibility), <c>include</c> or <c>exclude</c>.</summary>
        public string LlmsInclude { get; set; }

        /// <summary>Whether AI crawlers may crawl the page: <c>inherit</c> (follow the site policy) or <c>block</c>.</summary>
        public string Crawl { get; set; }
    }
}
