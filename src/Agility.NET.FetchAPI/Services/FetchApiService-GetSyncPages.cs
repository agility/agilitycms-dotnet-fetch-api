
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
		 * Get the sync pages for a given sitemap/channel, locale and token offset.
		 * @param getSyncParameters The parameters for the request
		 * @return The sync pages
		 */
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

				var msg = BuildRequestMessage(url, HttpMethod.Get, getSyncParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);
				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException($"There was an error getting the page sync for locale {getSyncParameters.Locale}", ex);
			}
		}

	}
}