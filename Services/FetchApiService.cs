using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

using System.Collections.Generic;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Agility.NET.FetchAPI.Util;
using Agility.NET.FetchAPI.Exceptions;

using Microsoft.Extensions.Options;

namespace Agility.NET.FetchAPI.Services
{

	public partial class FetchApiService : IApiService
	{
		private readonly HttpClient _httpClient;

		private readonly AppSettings _appSettings;


		private Dictionary<string, GraphQLHttpClient> _previewGqlClients = new Dictionary<string, GraphQLHttpClient>();
		private Dictionary<string, GraphQLHttpClient> _fetchGqlClients = new Dictionary<string, GraphQLHttpClient>();


		public FetchApiService(HttpClient client, IOptions<AppSettings> appSettings)
		{
			_httpClient = client;
			_appSettings = appSettings.Value;
			_httpClient.DefaultRequestHeaders.Add("accept", "application/json");
		}


		/**
		 * Get the graphQL client for the specified locale.  The client is supposed to be long running and re-used for multiple requests.
		 *
		 * @return GraphQLHttpClient
		 */
		private GraphQLHttpClient GetGraphQLClient(string locale, bool isPreview)
		{

			string baseUrl = GetBaseUrl();

			var dictionary = isPreview ? _previewGqlClients : _fetchGqlClients;

			if (dictionary.ContainsKey(locale))
			{
				//use the existing client if we've already got it ready...
				return dictionary[locale];
			}

			//create the client only if we need to
			var url = $"{baseUrl}/v1/{_appSettings.InstanceGUID}/fetch/en-us/graphql";

			if (isPreview)
			{
				url = $"{baseUrl}/v1/{_appSettings.InstanceGUID}/preview/en-us/graphql";

			}

			var client = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

			//stash it in the dictionary so we can use it for later requests
			dictionary[locale] = client;

			return client;

		}



		private HttpRequestMessage BuildRequestMessage(string url, HttpMethod method, bool isPreview)
		{
			string baseUrl = GetBaseUrl();

			var apiType = isPreview ? Constants.Preview : Constants.Fetch;

			var fullUrl = $"{baseUrl}/{_appSettings.InstanceGUID}/{apiType}{url}";

			var msg = new HttpRequestMessage(method, fullUrl);

			var apiKey = isPreview ? _appSettings.PreviewAPIKey : _appSettings.FetchAPIKey;
			msg.Headers.Add("APIKey", apiKey);


			return msg;
		}

		private string GetBaseUrl()
		{
			var baseUrl = Constants.BaseUrl;
			if (_appSettings.InstanceGUID.EndsWith("-d"))
			{
				baseUrl = Constants.BaseUrlDev;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-c"))
			{
				baseUrl = Constants.BaseUrl;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-e"))
			{
				baseUrl = Constants.BaseUrlEurope;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-a"))
			{
				baseUrl = Constants.BaseUrlAustrailia;
			}

			return baseUrl;
		}


		private static async Task<string> EnsureSuccessResult(HttpResponseMessage response)
		{
			if (response.StatusCode != HttpStatusCode.OK) throw new ApplicationException($"HttpException: {response.StatusCode} - {response.ReasonPhrase}");

			var result = await response.Content.ReadAsStringAsync();
			return result;
		}

	}

}

