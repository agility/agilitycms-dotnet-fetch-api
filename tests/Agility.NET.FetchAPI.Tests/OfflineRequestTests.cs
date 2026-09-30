using System.Net;
using Xunit;
using System.Text;
using Agility.NET.FetchAPI.Exceptions;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.Extensions.Options;

namespace Agility.NET.FetchAPI.Tests;

/// <summary>
/// Checks the requests the SDK builds, against a fake handler: no network or credentials needed.
/// </summary>
public class OfflineRequestTests
{
    private const string Guid = "1234abcd-u";

    private sealed class FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            return Task.FromResult(respond(request));
        }
    }

    private static (FetchApiService Service, FakeHandler Handler) Create(Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        var handler = new FakeHandler(respond);
        var service = new FetchApiService(new HttpClient(handler), Options.Create(new AppSettings
        {
            InstanceGUID = Guid,
            FetchAPIKey = "fetch-key",
            PreviewAPIKey = "preview-key",
        }));
        return (service, handler);
    }

    private static HttpResponseMessage Json(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    [Theory]
    [InlineData("fr-ca", false, "/v1/1234abcd-u/fetch/fr-ca/graphql", "fetch-key")]
    [InlineData("en-us", true, "/v1/1234abcd-u/preview/en-us/graphql", "preview-key")]
    public async Task GraphQL_queries_go_to_the_requested_locale_and_mode(string locale, bool preview, string path, string key)
    {
        var (service, handler) = Create(_ => Json("""{"data":{"posts":[]}}"""));

        var result = await service.GetContentByGraphQL<object>("{ posts { contentID } }", locale, "posts", preview);

        Assert.Empty(result);
        var request = Assert.Single(handler.Requests);
        Assert.Equal($"https://api.aglty.io{path}", request.RequestUri!.ToString());
        Assert.Equal(key, request.Headers.GetValues("APIKey").Single());
    }

    [Fact]
    public async Task Each_locale_gets_its_own_GraphQL_endpoint()
    {
        var (service, handler) = Create(_ => Json("""{"data":{"posts":[]}}"""));

        await service.GetContentByGraphQL<object>("{ posts { contentID } }", "en-us", "posts");
        await service.GetContentByGraphQL<object>("{ posts { contentID } }", "es-us", "posts");

        Assert.Equal(["/v1/1234abcd-u/fetch/en-us/graphql", "/v1/1234abcd-u/fetch/es-us/graphql"],
            handler.Requests.Select(r => r.RequestUri!.AbsolutePath));
    }

    [Fact]
    public async Task Requests_identify_the_SDK()
    {
        var (service, handler) = Create(_ => Json("{}"));

        await service.GetContentItem(new GetItemParameters { Locale = "en-us", ContentId = 5 });

        var request = Assert.Single(handler.Requests);
        Assert.Matches(@"^agility-fetch-sdk-dotnet/\d+\.\d+\.\d+", request.Headers.GetValues("X-Agility-SDK").Single());
        Assert.Contains("agility-fetch-sdk-dotnet/", request.Headers.UserAgent.ToString());
        Assert.Equal("https://api.aglty.io/1234abcd-u/fetch/en-us/item/5?contentLinkDepth=0", request.RequestUri!.ToString());
    }

    [Fact]
    public async Task Errors_keep_the_original_message_prefix_and_add_the_API_response()
    {
        var (service, _) = Create(_ => new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            ReasonPhrase = "Not Found",
            Content = new StringContent("Content item 5 was not found."),
        });

        var ex = await Assert.ThrowsAsync<AgilityResponseException>(() =>
            service.GetContentItem(new GetItemParameters { Locale = "en-us", ContentId = 5 }));

        var inner = Assert.IsType<ApplicationException>(ex.InnerException);
        Assert.StartsWith("HttpException: NotFound - Not Found", inner.Message);
        Assert.Contains("Content item 5 was not found.", inner.Message);
    }
}
