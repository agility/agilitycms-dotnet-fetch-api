using System.Collections.Generic;
using Agility.NET.FetchAPI.Helpers;

namespace Agility.NET.FetchAPI.Models.API
{
    public class PageResponseDynamicZone
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
    public class PageResponse
    {
        public PageResponse(PageResponseDynamicZone pageResponse)
        {
            PageID = pageResponse.PageID;
            Name = pageResponse.Name;
            Path = pageResponse.Path;
            Title = pageResponse.Title;
            MenuText = pageResponse.MenuText;
            PageType = pageResponse.PageType;
            TemplateName = pageResponse.TemplateName;
            RedirectUrl = pageResponse.RedirectUrl;
            SecurePage = pageResponse.SecurePage;
            ExcludeFromOutputCache = pageResponse.ExcludeFromOutputCache;
            Visible = pageResponse.Visible;
            SEO = pageResponse.SEO;
            Scripts = pageResponse.Scripts;
            Dynamic = pageResponse.Dynamic;
            IsDynamicPage = pageResponse.IsDynamicPage;
            Properties = pageResponse.Properties;
            Zones = DynamicHelpers.DeserializeContentZones(pageResponse.Zones.ToString());
        }
        public PageResponse() { }
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
        public List<ContentZone> Zones { get; set; }

    }
}
