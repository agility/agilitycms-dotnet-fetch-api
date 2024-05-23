
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
		 * Get the flat sitemap for a given name.  Note that the channel name here represents the reference name of the sitemap.
		 * @param getSitemapParameters The parameters for the request
		 * @return The flat sitemap
		 */
		public async Task<string> GetSitemapFlat(GetSitemapParameters getSitemapParameters)
		{

			try
			{

				var url = $"/{getSitemapParameters.Locale}/sitemap/flat/{getSitemapParameters.ChannelName}";

				var msg = BuildRequestMessage(url, HttpMethod.Get, getSitemapParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the flat sitemap with reference name {getSitemapParameters.ChannelName} in locale {getSitemapParameters.Locale}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

	}
}