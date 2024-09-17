using System.Net.Http;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;

public class GraphQLHttpRequestWithHeadersSupport : GraphQLHttpRequest
{

	private string apiKey;

	public GraphQLHttpRequestWithHeadersSupport(string query, string apiKey) : base(query)
	{
		this.apiKey = apiKey;
	}

	public override HttpRequestMessage ToHttpRequestMessage(GraphQLHttpClientOptions options, IGraphQLJsonSerializer serializer)
	{
		var r = base.ToHttpRequestMessage(options, serializer);
		if (!r.Headers.TryAddWithoutValidation("APIKey", apiKey))
		{
			r.Headers.Remove("APIKey");
			r.Headers.Add("APIKey", apiKey);
		}
		return r;
	}
}