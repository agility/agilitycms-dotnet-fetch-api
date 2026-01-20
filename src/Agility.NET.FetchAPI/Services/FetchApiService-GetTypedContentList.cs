
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Exceptions;
using System.Web;
using System.Collections.Generic;
using Agility.NET.FetchAPI.Models;

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService
	{

		/**
		 * Get the typed content list for a given reference name. If the reference name is not found, an empty list will be returned.
		 * @param getListParameters The parameters for the request
		 * @return The typed content list
		 */
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getListParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);

				//if we get a 404, return an empty list
				if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
				{
					return new ContentListResponse<T>
					{
						Items = new List<GenericContentItem<T>>()
					};
				}

				string responseBody = await EnsureSuccessResult(response);
				var t = DynamicHelpers.DeserializeContentListTo<T>(responseBody);

				return t;
			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the typed content list with reference name {getListParameters.ReferenceName} in locale {getListParameters.Locale}", ex);
			}
		}
	}
}