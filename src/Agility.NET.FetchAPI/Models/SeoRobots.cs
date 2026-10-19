namespace Agility.NET.FetchAPI.Models
{
    /// <summary>A page's robots directives. Each is null when not set.</summary>
    public class SeoRobots
    {
        /// <summary>Whether search engines may index the page.</summary>
        public bool? Index { get; set; }

        /// <summary>Whether search engines may follow the page's links.</summary>
        public bool? Follow { get; set; }
    }
}
