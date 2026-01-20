
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Exceptions;
using System.Web;

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService
	{

		/**
		 * Get the typed layout/page for a given id and locale.
		 * @param getPageParameters The parameters for the request
		 * @return The page
		 */
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getPageParameters.IsPreview);
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

	}
}