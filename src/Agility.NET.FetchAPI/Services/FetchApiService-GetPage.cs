
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
		 * Get the layout/page for a given id
		 * @param getPageParameters The parameters for the request
		 * @return The page
		 */
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getPageParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);


			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the page with id {getPageParameters.PageId} in locale {getPageParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

	}
}