using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

using System.Collections.Generic;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Agility.NET.FetchAPI.Util;
using Agility.NET.FetchAPI.Exceptions;

using Microsoft.Extensions.Options;

namespace Agility.NET.FetchAPI.Services
{

	public class FetchApiService : IApiService
	{
		private readonly HttpClient _httpClient;

		private readonly AppSettings _appSettings;


		private Dictionary<string, GraphQLHttpClient> _previewGqlClients = new Dictionary<string, GraphQLHttpClient>();
		private Dictionary<string, GraphQLHttpClient> _fetchGqlClients = new Dictionary<string, GraphQLHttpClient>();


		public FetchApiService(HttpClient client, IOptions<AppSettings> appSettings)
		{
			_httpClient = client;
			_appSettings = appSettings.Value;
			_httpClient.DefaultRequestHeaders.Add("accept", "application/json");
		}


		/**
		 * Get the graphQL client for the specified locale.  The client is supposed to be long running and re-used for multiple requests.
		 *
		 * @return GraphQLHttpClient
		 */
		private GraphQLHttpClient GetGraphQLClient(string locale)
		{

			string baseUrl = GetBaseUrl();

			var dictionary = IsPreview ? _previewGqlClients : _fetchGqlClients;

			if (dictionary.ContainsKey(locale))
			{
				//use the existing client if we've already got it ready...
				return dictionary[locale];
			}

			//create the client only if we need to
			var url = $"https://{baseUrl}/v1/{_appSettings.InstanceGUID}/fetch/en-us/graphql";

			if (IsPreview)
			{
				url = $"https://{baseUrl}/v1/{_appSettings.InstanceGUID}/preview/en-us/graphql";

			}

			var client = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

			//stash it in the dictionary so we can use it for later requests
			dictionary[locale] = client;

			return client;

		}


		/**
		 * Get a strongly typed Content Item by ID, locale and depth.
		 *
		 * @return ContentItemResponse<T>
		 */
		public async Task<ContentItemResponse<T>> GetTypedContentItem<T>(GetItemParameters getItemParameters)
		{
			try
			{
				var url = $"/{getItemParameters.Locale}/item/{getItemParameters.ContentId}?contentLinkDepth={getItemParameters.ContentLinkDepth}";

				if (getItemParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"expandAllContentLinks={getItemParameters.ExpandAllContentLinks}");
				}

				var msg = new HttpRequestMessage(HttpMethod.Get, url);
				var response = await _httpClient.SendAsync(msg);

				// Deserialize the content item to the specified generic type
				if (!response.IsSuccessStatusCode)
				{
					throw new AgilityResponseException($"There was an error retrieving the typed content with id {getItemParameters.ContentId} in locale {getItemParameters.Locale}: {response.StatusCode} - {response.ReasonPhrase}");
				}

				string responseBody = await response.Content.ReadAsStringAsync();


				var deserializedItem = DynamicHelpers.DeserializeContentItemTo<T>(responseBody);

				return deserializedItem;
			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error retrieving the typed content with id {getItemParameters.ContentId} in locale {getItemParameters.Locale}", ex);
			}
		}

		public async Task<string> GetContentItem(GetItemParameters getItemParameters)
		{

			try
			{

				var url = $"/{getItemParameters.Locale}/item/{getItemParameters.ContentId}?contentLinkDepth={getItemParameters.ContentLinkDepth}";

				if (getItemParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"expandAllContentLinks={getItemParameters.ExpandAllContentLinks}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);
			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the content with id {getItemParameters.ContentId} in locale {getItemParameters.Locale}";

				throw new AgilityResponseException(errorMsg, ex);
			}
		}
		public async Task<ContentListResponse<T>> GetTypedContentList<T>(GetListParameters getListParameters)
		{
			try
			{

				var url = $"/{getListParameters.Locale}/list/{getListParameters.ReferenceName}?ContentLinkDepth={getListParameters.ContentLinkDepth}";

				if (!string.IsNullOrEmpty(getListParameters.Fields))
				{
					url = UrlHelpers.AppendParameter(url, $"Fields={HttpUtility.UrlEncode(getListParameters.Fields)}");
				}

				if (getListParameters.Take > 0)
				{
					url = UrlHelpers.AppendParameter(url, $"Take={getListParameters.Take}");
				}

				if (getListParameters.Skip > 0)
				{
					url = UrlHelpers.AppendParameter(url, $"Skip={getListParameters.Skip}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Filter))
				{
					url = UrlHelpers.AppendParameter(url, $"Filter={HttpUtility.UrlEncode(getListParameters.Filter)}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Sort))
				{
					url = UrlHelpers.AppendParameter(url, $"Sort={HttpUtility.UrlEncode(getListParameters.Sort)}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Direction))
				{
					url = UrlHelpers.AppendParameter(url, $"Direction={HttpUtility.UrlEncode(getListParameters.Direction)}");
				}

				url = UrlHelpers.AppendParameter(url, $"ContentLinkDepth={getListParameters.ContentLinkDepth}");

				if (getListParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"ExpandAllContentLinks={getListParameters.ExpandAllContentLinks}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				string responseBody = await EnsureSuccessResult(response);
				var t = DynamicHelpers.DeserializeContentListTo<T>(responseBody);

				return t;
			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the typed content list with reference name {getListParameters.ReferenceName} in locale {getListParameters.Locale}", ex);
			}
		}

		public async Task<string> GetContentList(GetListParameters getListParameters)
		{

			try
			{
				var url = $"/{getListParameters.Locale}/list/{getListParameters.ReferenceName}?ContentLinkDepth={getListParameters.ContentLinkDepth}";

				if (!string.IsNullOrEmpty(getListParameters.Fields))
				{
					url = UrlHelpers.AppendParameter(url, $"Fields={HttpUtility.UrlEncode(getListParameters.Fields)}");
				}

				if (getListParameters.Take > 0)
				{
					url = UrlHelpers.AppendParameter(url, $"Take={getListParameters.Take}");
				}

				if (getListParameters.Skip > 0)
				{
					url = UrlHelpers.AppendParameter(url, $"Skip={getListParameters.Skip}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Filter))
				{
					url = UrlHelpers.AppendParameter(url, $"Filter={HttpUtility.UrlEncode(getListParameters.Filter)}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Sort))
				{
					url = UrlHelpers.AppendParameter(url, $"Sort={HttpUtility.UrlEncode(getListParameters.Sort)}");
				}

				if (!string.IsNullOrEmpty(getListParameters.Direction))
				{
					url = UrlHelpers.AppendParameter(url, $"Direction={HttpUtility.UrlEncode(getListParameters.Direction)}");
				}

				url = UrlHelpers.AppendParameter(url, $"ContentLinkDepth={getListParameters.ContentLinkDepth}");

				if (getListParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"ExpandAllContentLinks={getListParameters.ExpandAllContentLinks}");
				}

				var response = await _httpClient.GetAsync(url);
				return await EnsureSuccessResult(response);


			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the content list for reference name {getListParameters.ReferenceName} in locale {getListParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}
		public async Task<List<ContentItemResponse<T>>> GetContentByGraphQL<T>(string query, string locale, string objName)
		{
			var apiKey = IsPreview ? _appSettings.PreviewAPIKey : _appSettings.FetchAPIKey;

			if (query == null || string.IsNullOrEmpty(query))
			{
				throw new ApplicationException("Query is required for GraphQL");
			}

			GraphQLHttpRequestWithHeadersSupport req = new GraphQLHttpRequestWithHeadersSupport(query, apiKey)
			{
				Query = query
			};

			try
			{

				var graphQLHttpClient = GetGraphQLClient(locale);


				var graphQLResponse = await graphQLHttpClient.SendQueryAsync<Dictionary<object, List<ContentItemResponse<T>>>>(req);

				var data = graphQLResponse.Data[objName];

				return data;

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException("There was an error getting the content via GraphQL", ex);
			}

		}

		public async Task<string> GetGallery(GetGalleryParameters getGalleryParameters)
		{


			try
			{
				var url = $"/gallery/{getGalleryParameters.GalleryId}";

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);

				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the gallery with id {getGalleryParameters.GalleryId}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

		public async Task<PageResponse> GetTypedPage(GetPageParameters getPageParameters)
		{
			try
			{
				if (getPageParameters.ContentLinkDepth > 0)
				{
					throw new ApplicationException("Content Link Depth must be 0 for typed pages");
				}

				var url = $"/{getPageParameters.Locale}/page/{getPageParameters.PageId}?contentLinkDepth={getPageParameters.ContentLinkDepth}";

				if (getPageParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"expandAllContentLinks={getPageParameters.ExpandAllContentLinks}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);

				string responseBody = await EnsureSuccessResult(response);

				var dynamicPageResponse = DynamicHelpers.DeserializeTo<PageResponseDynamicZone>(responseBody);
				var pageResponse = new PageResponse(dynamicPageResponse);

				return pageResponse;

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the typed page with id {getPageParameters.PageId} in locale {getPageParameters.Locale}", ex);
			}
		}

		public async Task<string> GetPage(GetPageParameters getPageParameters)
		{

			try
			{
				var url =
					$"/{getPageParameters.Locale}/page/{getPageParameters.PageId}?contentLinkDepth={getPageParameters.ContentLinkDepth}";

				if (getPageParameters.ExpandAllContentLinks)
				{
					url = UrlHelpers.AppendParameter(url, $"expandAllContentLinks={getPageParameters.ExpandAllContentLinks}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);


			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the page with id {getPageParameters.PageId} in locale {getPageParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}
		public async Task<List<SitemapPage>> GetTypedSitemapFlat(GetSitemapParameters getSitemapParameters)
		{
			try
			{
				var url = $"/{getSitemapParameters.Locale}/sitemap/flat/{getSitemapParameters.ChannelName}";

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				var responseStr = await EnsureSuccessResult(response);

				var deserializedSitemap = DynamicHelpers.DeserializeSitemapFlat(responseStr);

				return deserializedSitemap;

			}
			catch (Exception)
			{
				throw new AgilityResponseException($"There was an error getting the flat, typed sitemap with reference name {getSitemapParameters.ChannelName} in locale {getSitemapParameters.Locale}");
			}
		}
		public async Task<string> GetSitemapFlat(GetSitemapParameters getSitemapParameters)
		{

			try
			{

				var url = $"/{getSitemapParameters.Locale}/sitemap/flat/{getSitemapParameters.ChannelName}";

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the flat sitemap with reference name {getSitemapParameters.ChannelName} in locale {getSitemapParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

		public async Task<List<SitemapPage>> GetTypedSitemapNested(GetSitemapParameters getSitemapParameters)
		{
			try
			{

				var url = $"/{getSitemapParameters.Locale}/sitemap/nested/{getSitemapParameters.ChannelName}";

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				var responseStr = await EnsureSuccessResult(response);

				var deserializedSitemap = DynamicHelpers.DeserializeSitemapNested(responseStr);

				return deserializedSitemap;

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the nested, typed sitemap with reference name {getSitemapParameters.ChannelName} in locale {getSitemapParameters.Locale}", ex);
			}
		}
		public async Task<string> GetSitemapNested(GetSitemapParameters getSitemapParameters)
		{

			try
			{

				var url = $"/{getSitemapParameters.Locale}/sitemap/nested/{getSitemapParameters.ChannelName}";

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the nested sitemap with reference name {getSitemapParameters.ChannelName} in locale {getSitemapParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

		public async Task<string> GetUrlRedirections(GetUrlRedirectionsParameters getUrlRedirectionsParameters)
		{
			try
			{

				var url = "/urlredirection";

				if (getUrlRedirectionsParameters.LastAccessDate != null)
				{
					var date = getUrlRedirectionsParameters.LastAccessDate.Value.ToString("o");
					url = UrlHelpers.AppendParameter(url, $"lastAccessDate={HttpUtility.UrlEncode(date)}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the url redirections", ex);
			}
		}

		public async Task<string> GetSyncContent(GetSyncParameters getSyncParameters)
		{
			try
			{
				var url = SyncHelpers.BuildSyncRequest(
					getSyncParameters.Locale,
					"Items",
					getSyncParameters.SyncToken,
					getSyncParameters.PageSize
				);

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the content sync for locale {getSyncParameters.Locale}", ex);
			}
		}

		public async Task<string> GetSyncPages(GetSyncParameters getSyncParameters)
		{
			try
			{
				var url = SyncHelpers.BuildSyncRequest(
					getSyncParameters.Locale,
					"pages",
					getSyncParameters.SyncToken,
					getSyncParameters.PageSize
				);

				var msg = BuildRequestMessage(url, HttpMethod.Get);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the page sync for locale {getSyncParameters.Locale}", ex);
			}
		}

		/**
		 * Returns true if Preview mode is enabled.  Use the SetMode() method to set the mode (Preview or Fetch)
		 *
		 * @return string
		 */
		public bool IsPreview
		{
			get;
			set;
		}



		private HttpRequestMessage BuildRequestMessage(string url, HttpMethod method)
		{
			string baseUrl = GetBaseUrl();

			var apiType = GetApiType();

			var fullUrl = $"{baseUrl}/{apiType}{url}";

			var msg = new HttpRequestMessage(method, fullUrl);

			var apiKey = IsPreview ? _appSettings.PreviewAPIKey : _appSettings.FetchAPIKey;
			msg.Headers.Add("APIKey", apiKey);
			return msg;
		}

		private string GetApiType()
		{
			return IsPreview ? Constants.Preview : Constants.Fetch;
		}

		private string GetBaseUrl()
		{
			var baseUrl = Constants.BaseUrl;
			if (_appSettings.InstanceGUID.EndsWith("-d"))
			{
				baseUrl = Constants.BaseUrlDev;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-ca"))
			{
				baseUrl = Constants.BaseUrl;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-eu"))
			{
				baseUrl = Constants.BaseUrlEurope;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-aus"))
			{
				baseUrl = Constants.BaseUrlAustrailia;
			}

			return baseUrl;
		}


		private static async Task<string> EnsureSuccessResult(HttpResponseMessage response)
		{
			if (response.StatusCode != HttpStatusCode.OK) throw new ApplicationException($"HttpException: {response.StatusCode} - {response.ReasonPhrase}");

			var result = await response.Content.ReadAsStringAsync();
			return result;
		}

	}

}

