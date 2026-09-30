using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Interfaces;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
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


		// One GraphQL client per locale, shared by concurrent requests.
		private readonly ConcurrentDictionary<string, GraphQLHttpClient> _previewGqlClients = new ConcurrentDictionary<string, GraphQLHttpClient>(StringComparer.OrdinalIgnoreCase);
		private readonly ConcurrentDictionary<string, GraphQLHttpClient> _fetchGqlClients = new ConcurrentDictionary<string, GraphQLHttpClient>(StringComparer.OrdinalIgnoreCase);

		/// <summary>This SDK's name and version, sent as X-Agility-SDK (and in the User-Agent) so the API can recognise it.</summary>
		internal static readonly string SdkProduct = $"agility-fetch-sdk-dotnet/{ReadSdkVersion()}";


		public FetchApiService(HttpClient client, IOptions<AppSettings> appSettings)
		{
			_httpClient = client;
			_appSettings = appSettings.Value;
			_httpClient.DefaultRequestHeaders.Add("accept", "application/json");
			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Agility-SDK", SdkProduct);
			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", SdkProduct);
		}

		private static string ReadSdkVersion()
		{
			var version = typeof(FetchApiService).Assembly
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "0.0.0";
			var plus = version.IndexOf('+');
			return plus >= 0 ? version.Substring(0, plus) : version;
		}


		/**
		 * Get the graphQL client for the specified locale.  The client is supposed to be long running and re-used for multiple requests.
		 *
		 * @return GraphQLHttpClient
		 */
		private GraphQLHttpClient GetGraphQLClient(string locale, bool isPreview)
		{

			if (string.IsNullOrWhiteSpace(locale))
			{
				throw new ArgumentException("A locale is required for GraphQL.", nameof(locale));
			}

			var dictionary = isPreview ? _previewGqlClients : _fetchGqlClients;

			return dictionary.GetOrAdd(locale, key =>
			{
				// The GraphQL endpoint is per locale: /v1/{guid}/{fetch|preview}/{locale}/graphql.
				var apiType = isPreview ? Constants.Preview : Constants.Fetch;
				var url = $"{GetBaseUrl()}/v1/{_appSettings.InstanceGUID}/{apiType}/{Uri.EscapeDataString(key.ToLowerInvariant())}/graphql";

				// Share the service's HttpClient, so DI-configured handlers and headers apply to GraphQL too.
				return new GraphQLHttpClient(new GraphQLHttpClientOptions { EndPoint = new Uri(url) }, new NewtonsoftJsonSerializer(), _httpClient);
			});

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
				baseUrl = Constants.BaseUrlCanada;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-e"))
			{
				baseUrl = Constants.BaseUrlEurope;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-a"))
			{
				baseUrl = Constants.BaseUrlAustrailia;
			}
			else if (_appSettings.InstanceGUID.EndsWith("-us2"))
			{
				baseUrl = Constants.BaseUrlUSA2;
			}

			return baseUrl;
		}


		private static async Task<string> EnsureSuccessResult(HttpResponseMessage response)
		{
			var result = await response.Content.ReadAsStringAsync();
			if (response.StatusCode != HttpStatusCode.OK)
			{
				// Keep the API's own error text: it usually says what's wrong (bad key, unknown reference name...).
				var detail = string.IsNullOrWhiteSpace(result) ? "" : $": {(result.Length > 1000 ? result.Substring(0, 1000) : result)}";
				// The message starts exactly as in 3.0 so callers matching on it keep working.
				throw new ApplicationException($"HttpException: {response.StatusCode} - {response.ReasonPhrase} ({(int)response.StatusCode} {response.RequestMessage?.Method} {response.RequestMessage?.RequestUri?.AbsolutePath}){detail}");
			}

			return result;
		}

	}

}

