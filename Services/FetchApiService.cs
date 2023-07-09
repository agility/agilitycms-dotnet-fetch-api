using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Interfaces;
using Agility.NET5.FetchAPI.Models.API;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.Shared.Models;
using Agility.NET5.Shared.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Agility.NET.FetchAPI.Services
{

    public class FetchApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _env;

        public FetchApiService(HttpClient client, IOptions<AppSettings> appSettings, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = client;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _env = env;
            _httpClient.BaseAddress = new Uri($"{(_appSettings.InstanceGUID.EndsWith("-d") ? Constants.BaseUrlDev : Constants.BaseUrl)}/{appSettings.Value.InstanceGUID}");
            _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
        }

        public async Task<ContentItemResponse<T>> GetTypedContentItem<T>(GetItemParameters getItemParameters)
        {
            try
            {

                var apiType = GetApiType();
                SetApiKey();

                if(getItemParameters.ContentLinkDepth > 0)
                {
                    return new ContentItemResponse<T>
                    {
                        ResponseMessage = "Content Link Depth for typed items must be 0"
                    };
                }
            

                var url =
                    $@"{_httpClient.BaseAddress}/{apiType}/{getItemParameters.Locale}/item/{getItemParameters.ContentId}?contentLinkDepth={getItemParameters.ContentLinkDepth}";

                if (getItemParameters.ExpandAllContentLinks)
                {
                    url += $@"&expandAllContentLinks={getItemParameters.ExpandAllContentLinks}";
                }

                var response = await _httpClient.GetAsync(url);

                // Deserialize the content item to the specified generic type

                if (!response.IsSuccessStatusCode) 
                {
                    return new ContentItemResponse<T>
                    {
                        ResponseMessage = "There was an error retrieving your content"
                    };
                }

                string responseBody = await response.Content.ReadAsStringAsync();


                var deserializedItem = DynamicHelpers.DeserializeContentItemTo<T>(responseBody);

                return deserializedItem;
            }
            catch (Exception ex)
            {

                return new ContentItemResponse<T>
                {
                    ResponseMessage = $"There was an error retrieving your content: {ex.Message}"
                };
                
            }
        }

        public async Task<string> GetContentItem(GetItemParameters getItemParameters)
        {
            try
            {

                var apiType = GetApiType();
                SetApiKey();

                var url =
                    $@"{_httpClient.BaseAddress}/{apiType}/{getItemParameters.Locale}/item/{getItemParameters.ContentId}?contentLinkDepth={getItemParameters.ContentLinkDepth}";

                if (getItemParameters.ExpandAllContentLinks)
                {
                    url += $@"&expandAllContentLinks={getItemParameters.ExpandAllContentLinks}";
                }

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);
            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        public async Task<ContentListResponse<T>> GetTypedContentList<T>(GetListParameters getListParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                if (getListParameters.ContentLinkDepth > 0)
                {
                    return new ContentListResponse<T>
                    {
                        ResponseMessage = "Content Link Depth for a typed list must be 0"
                    };
                }

                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getListParameters.Locale}/list/{getListParameters.ReferenceName}?ContentLinkDepth={getListParameters.ContentLinkDepth}";

                if (!string.IsNullOrEmpty(getListParameters.Fields))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Fields={HttpUtility.UrlEncode(getListParameters.Fields)}");
                }

                if (getListParameters.Take > 0)
                {
                    url = UrlHelpers.AppendParameter(url, $@"Take={getListParameters.Take}");
                }

                if (getListParameters.Skip > 0)
                {
                    url = UrlHelpers.AppendParameter(url, $@"Skip={getListParameters.Skip}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Filter))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Filter={HttpUtility.UrlEncode(getListParameters.Filter)}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Sort))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Sort={HttpUtility.UrlEncode(getListParameters.Sort)}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Direction))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Direction={HttpUtility.UrlEncode(getListParameters.Direction)}");
                }

                url = UrlHelpers.AppendParameter(url, $@"ContentLinkDepth={getListParameters.ContentLinkDepth}");

                if (getListParameters.ExpandAllContentLinks)
                {
                    url = UrlHelpers.AppendParameter(url, $@"ExpandAllContentLinks={getListParameters.ExpandAllContentLinks}");
                }

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new ContentListResponse<T>
                    {
                        ResponseMessage = "There was an error retrieving your content"
                    };
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                

                var t = DynamicHelpers.DeserializeContentListTo<T>(responseBody);

                return t;
            }
            catch (Exception ex)
            {
                return new ContentListResponse<T>
                {
                    ResponseMessage = $"There was an error retrieving your content: {ex.Message}"
                };
            }
        }

        public async Task<string> GetContentList(GetListParameters getListParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getListParameters.Locale}/list/{getListParameters.ReferenceName}?ContentLinkDepth={getListParameters.ContentLinkDepth}";

                if (!string.IsNullOrEmpty(getListParameters.Fields))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Fields={HttpUtility.UrlEncode(getListParameters.Fields)}");
                }

                if (getListParameters.Take > 0)
                {
                    url = UrlHelpers.AppendParameter(url, $@"Take={getListParameters.Take}");
                }

                if (getListParameters.Skip > 0)
                {
                    url = UrlHelpers.AppendParameter(url, $@"Skip={getListParameters.Skip}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Filter))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Filter={HttpUtility.UrlEncode(getListParameters.Filter)}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Sort))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Sort={HttpUtility.UrlEncode(getListParameters.Sort)}");
                }

                if (!string.IsNullOrEmpty(getListParameters.Direction))
                {
                    url = UrlHelpers.AppendParameter(url, $@"Direction={HttpUtility.UrlEncode(getListParameters.Direction)}");
                }

                url = UrlHelpers.AppendParameter(url, $@"ContentLinkDepth={getListParameters.ContentLinkDepth}");

                if (getListParameters.ExpandAllContentLinks)
                {
                    url = UrlHelpers.AppendParameter(url, $@"ExpandAllContentLinks={getListParameters.ExpandAllContentLinks}");
                }

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);


            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        public async Task<string> GetGallery(GetGalleryParameters getGalleryParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/gallery/{getGalleryParameters.GalleryId}";

                var response = await _httpClient.GetAsync(url);

                return await EnsureSuccessResult(response);


            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        
        public async Task<PageResponse> GetTypedPage(GetPageParameters getPageParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                if (getPageParameters.ContentLinkDepth > 0)
                {
                    return new PageResponse
                    {
                        ResponseMessage = "Content Link Depth must be 0 for typed pages"
                    };
                }

                var url =
                    $@"{_httpClient.BaseAddress}/{apiType}/{getPageParameters.Locale}/page/{getPageParameters.PageId}?contentLinkDepth={getPageParameters.ContentLinkDepth}";

                if (getPageParameters.ExpandAllContentLinks)
                {
                    url += $@"&expandAllContentLinks={getPageParameters.ExpandAllContentLinks}";
                }

                var response = await _httpClient.GetAsync(url);
                
                // Deserialize the content item to the specified generic type

                if (!response.IsSuccessStatusCode)
                {
                    return new PageResponse
                    {
                        ResponseMessage = "There was an error retrieving your content"
                    };
                }

                string responseBody = await response.Content.ReadAsStringAsync();

                
                var dynamicPageResponse = DynamicHelpers.DeserializeTo<PageResponseDynamicZone>(responseBody);
                var pageResponse = new PageResponse(dynamicPageResponse);

                return pageResponse;


            }
            catch (Exception ex)
            {
                return new PageResponse
                {
                    ResponseMessage = $"There was an error retrieving your content: {ex.Message}"
                };
            }
        }

        public async Task<string> GetPage(GetPageParameters getPageParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url =
                    $@"{_httpClient.BaseAddress}/{apiType}/{getPageParameters.Locale}/page/{getPageParameters.PageId}?contentLinkDepth={getPageParameters.ContentLinkDepth}";

                if (getPageParameters.ExpandAllContentLinks)
                {
                    url += $@"&expandAllContentLinks={getPageParameters.ExpandAllContentLinks}";
                }

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);


            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }
        public async Task<List<SitemapPage>> GetTypedSitemapFlat(GetSitemapParameters getSitemapParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();


                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getSitemapParameters.Locale}/sitemap/flat/{getSitemapParameters.ChannelName}";

                var response = await _httpClient.GetAsync(url);

                if(!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseStr = await response.Content.ReadAsStringAsync();

                var deserializedSitemap = DynamicHelpers.DeserializeSitemapFlat(responseStr);

                return deserializedSitemap;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> GetSitemapFlat(GetSitemapParameters getSitemapParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getSitemapParameters.Locale}/sitemap/flat/{getSitemapParameters.ChannelName}";

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);

            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        public async Task<List<SitemapPage>> GetTypedSitemapNested(GetSitemapParameters getSitemapParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getSitemapParameters.Locale}/sitemap/nested/{getSitemapParameters.ChannelName}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }


                var responseStr = await response.Content.ReadAsStringAsync();

                var deserializedSitemap = DynamicHelpers.DeserializeSitemapNested(responseStr);

                return deserializedSitemap;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> GetSitemapNested(GetSitemapParameters getSitemapParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/{getSitemapParameters.Locale}/sitemap/nested/{getSitemapParameters.ChannelName}";

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);

            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        public async Task<string> GetUrlRedirections(GetUrlRedirectionsParameters getUrlRedirectionsParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = $@"{_httpClient.BaseAddress}/{apiType}/urlredirection";

                if (getUrlRedirectionsParameters.LastAccessDate != null)
                {
                    var date = getUrlRedirectionsParameters.LastAccessDate.Value.ToString("o");
                    url += $"?lastAccessDate={HttpUtility.UrlEncode(date)}";
                }

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);

            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        public async Task<string> GetSyncContent(GetSyncParameters getSyncParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = SyncHelpers.BuildSyncRequest(
                    _httpClient.BaseAddress?.ToString(),
                    apiType,
                    getSyncParameters.Locale,
                    "Items",
                    getSyncParameters.SyncToken,
                    getSyncParameters.PageSize
                );

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);

            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        public async Task<string> GetSyncPages(GetSyncParameters getSyncParameters)
        {
            try
            {
                var apiType = GetApiType();
                SetApiKey();

                var url = SyncHelpers.BuildSyncRequest(
                    _httpClient.BaseAddress?.ToString(),
                    apiType,
                    getSyncParameters.Locale,
                    "pages",
                    getSyncParameters.SyncToken,
                    getSyncParameters.PageSize
                );

                var response = await _httpClient.GetAsync(url);
                return await EnsureSuccessResult(response);

            }
            catch (Exception ex)
            {
                return ReturnError(ex);
            }
        }

        private string GetApiType()
        {

            var isPreview = HttpContextHelpers.IsPreview(_httpContextAccessor);

            if (!_env.IsDevelopment() && string.IsNullOrEmpty(isPreview))
            {
                return Constants.Fetch;
            }

            if (string.IsNullOrEmpty(isPreview))
            {
                return Constants.Preview;
            }

            return isPreview switch
            {
                "true" => Constants.Preview,
                "false" => Constants.Fetch,
                _ => Constants.Preview
            };
        }

        private void SetApiKey()
        {
            var isPreview = HttpContextHelpers.IsPreview(_httpContextAccessor);

            if (!_env.IsDevelopment() && string.IsNullOrEmpty(isPreview))
            {
                SetApiKey(_appSettings.FetchAPIKey);
                return;
            }

            switch (isPreview)
            {
                case "true":
                    SetApiKey(_appSettings.PreviewAPIKey);
                    break;
                case "false":
                    SetApiKey(_appSettings.FetchAPIKey);
                    break;
                default:
                    SetApiKey(_appSettings.PreviewAPIKey);
                    break;
            };
        }

        private void SetApiKey(string apiKey)
        {
            if (_httpClient.DefaultRequestHeaders.Contains("APIKey"))
            {
                _httpClient.DefaultRequestHeaders.Remove("APIKey");
            }

            _httpClient.DefaultRequestHeaders.Add("APIKey", apiKey);
        }

        private string ReturnError(Exception ex)
        {

            return JsonSerializer.Serialize(new ErrorResponse()
            {
                ErrorCode = -1,
                ErrorMessage = ex.Message
            });
        }

        private static async Task<string> EnsureSuccessResult(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK) return string.Empty;

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }

}

