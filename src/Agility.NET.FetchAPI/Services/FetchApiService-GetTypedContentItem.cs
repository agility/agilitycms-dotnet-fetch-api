
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Exceptions;

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService
	{
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getItemParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);

				var responseBody = await EnsureSuccessResult(response);

				var deserializedItem = DynamicHelpers.DeserializeContentItemTo<T>(responseBody);

				return deserializedItem;
			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error retrieving the typed content with id {getItemParameters.ContentId} in locale {getItemParameters.Locale}", ex);
			}
		}
	}
}