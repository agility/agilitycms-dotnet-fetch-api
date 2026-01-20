
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

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService
	{

		/**
		 * Get the url redirections
		 * @param getUrlRedirectionsParameters The parameters for the request
		 * @return The url redirections
		 */
		public async Task<string> GetUrlRedirections(GetUrlRedirectionsParameters getUrlRedirectionsParameters)
		{
			try
			{

				var url = "/urlredirection";

				if (getUrlRedirectionsParameters.LastAccessDate != null)
				{
					var date = getUrlRedirectionsParameters.LastAccessDate.Value.ToString("yyyy-MM-ddTHH:mm:ss.ffK");
					url = UrlHelpers.AppendParameter(url, $"lastAccessDate={HttpUtility.UrlEncode(date)}");
				}

				var msg = BuildRequestMessage(url, HttpMethod.Get, getUrlRedirectionsParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the url redirections", ex);
			}
		}

	}
}