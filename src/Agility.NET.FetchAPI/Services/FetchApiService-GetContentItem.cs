
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

		public async Task<string> GetContentItem(GetItemParameters getItemParameters)
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
				return await EnsureSuccessResult(response);
			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the content with id {getItemParameters.ContentId} in locale {getItemParameters.Locale}";

				throw new AgilityResponseException(errorMsg, ex);
			}
		}

	}
}