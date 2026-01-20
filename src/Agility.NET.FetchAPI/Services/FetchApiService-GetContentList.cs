
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Exceptions;
using System.Web;
using Agility.NET.FetchAPI.Models;
using Newtonsoft.Json;

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService
	{

		/**
		 * Get the content list for a given reference name.  If the reference name is not found, an empty list will be returned.
		 * @param getListParameters The parameters for the request
		 * @return The content list
		 */
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getListParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);

				//if we get a 404, return an empty list
				if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					return JsonConvert.SerializeObject(new ContentListResponse<EmptyContent>());
				}

				return await EnsureSuccessResult(response);


			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the content list for reference name {getListParameters.ReferenceName} in locale {getListParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}
	}
}