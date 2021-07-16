using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Models.Data;

namespace Agility.NET5.FetchAPI.Interfaces
{
    public interface IApiService
    {
        Task<string> GetContentItem(GetItemParameters getItemParameters);

        Task<string> GetContentList(GetListParameters getListParameters);

        Task<string> GetGallery(GetGalleryParameters getGalleryParameters);

        Task<string> GetPage(GetPageParameters getPageParameters);

        Task<string> GetSitemapFlat(GetSitemapParameters getSitemapParameters);

        Task<string> GetSitemapNested(GetSitemapParameters getSitemapParameters);

        Task<string> GetUrlRedirections(GetUrlRedirectionsParameters getUrlRedirectionsParameters);

        Task<string> GetSyncContent(GetSyncParameters getSyncParameters);

        Task<string> GetSyncPages(GetSyncParameters getSyncParameters);
    }
}