
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
		 * Get the gallery for a given gallery id
		 * @param getGalleryParameters The parameters for the request
		 * @return The gallery
		 */
		public async Task<string> GetGallery(GetGalleryParameters getGalleryParameters)
		{


			try
			{
				var url = $"/gallery/{getGalleryParameters.GalleryId}";

				var msg = BuildRequestMessage(url, HttpMethod.Get, getGalleryParameters.IsPreview);
				var response = await _httpClient.SendAsync(msg);

				return await EnsureSuccessResult(response);

			}
			catch (Exception ex)
			{
				var errorMsg = $"There was an error getting the gallery with id {getGalleryParameters.GalleryId}";
				throw new AgilityResponseException(errorMsg, ex);
			}
		}

	}
}