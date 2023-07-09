using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.FetchAPI.Models.API;
using System.Collections.Generic;

namespace Agility.NET5.FetchAPI.Interfaces
{
    public interface IApiService
    {
        Task<ContentItemResponse<T>> GetTypedContentItem<T>(GetItemParameters getItemParameters);
        Task<string> GetContentItem(GetItemParameters getItemParameters);

        Task<ContentListResponse<T>> GetTypedContentList<T>(GetListParameters getListParameters);
        Task<string> GetContentList(GetListParameters getListParameters);

        Task<string> GetGallery(GetGalleryParameters getGalleryParameters);

        Task<PageResponse> GetTypedPage(GetPageParameters getPageParameters);
        Task<string> GetPage(GetPageParameters getPageParameters);
        Task<List<SitemapPage>> GetTypedSitemapFlat(GetSitemapParameters getSitemapParameters);
        Task<string> GetSitemapFlat(GetSitemapParameters getSitemapParameters);
        Task<List<SitemapPage>> GetTypedSitemapNested(GetSitemapParameters getSitemapParameters);
        Task<string> GetSitemapNested(GetSitemapParameters getSitemapParameters);

        Task<string> GetUrlRedirections(GetUrlRedirectionsParameters getUrlRedirectionsParameters);

        Task<string> GetSyncContent(GetSyncParameters getSyncParameters);

        Task<string> GetSyncPages(GetSyncParameters getSyncParameters);
    }
}