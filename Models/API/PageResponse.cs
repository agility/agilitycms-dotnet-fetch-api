using Agility.NET5.Shared.Models;
using NET5.FetchAPI.Models.API;

namespace Agility.NET5.FetchAPI.Models.API
{
    public class PageResponse
    {
        public int PageID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string MenuText { get; set; }
        public string PageType { get; set; }
        public string TemplateName { get; set; }
        public string RedirectUrl { get; set; }
        public bool SecurePage { get; set; }
        public bool ExcludeFromOutputCache { get; set; }
        public Visible Visible { get; set; }
        public SEO SEO { get; set; }
        public Scripts Scripts { get; set; }
        public Dynamic Dynamic { get; set; }
        public bool IsDynamicPage { get; set; }
        public Properties Properties { get; set; }
        public dynamic Zones { get; set; }

    }
}
