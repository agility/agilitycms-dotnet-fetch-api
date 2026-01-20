using System.Collections.Generic;

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
        public string Redirect { get; set; }
        public bool IsFolder { get; set; }
        public List<SitemapPage> Children { get; set; }
        public string Locale { get; set; }
        public int ContentID { get; set; }
    }
}
