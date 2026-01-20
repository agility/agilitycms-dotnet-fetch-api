using System.Collections.Generic;
using System.Text.Json.Serialization;
using Agility.NET.FetchAPI.Converters;

namespace Agility.NET.FetchAPI.Models.API
{
    public class SitemapPage
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int PageID { get; set; }
        public string MenuText { get; set; }
        public Visible Visible { get; set; }
        public string Path { get; set; }

        [JsonConverter(typeof(RedirectUrlConverter))]
        public RedirectUrl RedirectUrl { get; set; }

        public bool IsFolder { get; set; }
        public List<SitemapPage> Children { get; set; }
        public string Locale { get; set; }
        public int ContentID { get; set; }
    }
}
