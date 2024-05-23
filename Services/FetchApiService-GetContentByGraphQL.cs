
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

		public async Task<List<ContentItemResponse<T>>> GetContentByGraphQL<T>(string query, string locale, string objName, bool isPreview = false)
		{

			var apiKey = isPreview ? _appSettings.PreviewAPIKey : _appSettings.FetchAPIKey;

			if (query == null || string.IsNullOrEmpty(query))
			{
				throw new ApplicationException("Query is required for GraphQL");
			}

			GraphQLHttpRequestWithHeadersSupport req = new GraphQLHttpRequestWithHeadersSupport(query, apiKey)
			{
				Query = query
			};

			try
			{

				var graphQLHttpClient = GetGraphQLClient(locale, isPreview);


				var graphQLResponse = await graphQLHttpClient.SendQueryAsync<Dictionary<object, List<ContentItemResponse<T>>>>(req);

				var data = graphQLResponse.Data[objName];

				return data;

			}
			catch (Exception ex)
			{
				throw new AgilityResponseException("There was an error getting the content via GraphQL", ex);
			}

		}


	}
}