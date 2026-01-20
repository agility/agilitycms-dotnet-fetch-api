using Xunit;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Services;
using Microsoft.Extensions.Options;

namespace Agility.NET.FetchAPI.Tests;

/// <summary>
/// Shared fixture that creates a configured FetchApiService for use across tests.
/// </summary>
public class ServiceFixture : IDisposable
{
    public FetchApiService FetchService { get; }
    public FetchApiService PreviewService { get; }
    private readonly HttpClient _fetchHttpClient;
    private readonly HttpClient _previewHttpClient;

    public ServiceFixture()
    {
        // Create HttpClient for fetch mode
        _fetchHttpClient = new HttpClient();

        var fetchSettings = new AppSettings
        {
            InstanceGUID = TestConfiguration.InstanceGUID,
            FetchAPIKey = TestConfiguration.FetchAPIKey,
            PreviewAPIKey = TestConfiguration.PreviewAPIKey,
            Locales = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        FetchService = new FetchApiService(_fetchHttpClient, Options.Create(fetchSettings));

        // Create HttpClient for preview mode
        _previewHttpClient = new HttpClient();

        var previewSettings = new AppSettings
        {
            InstanceGUID = TestConfiguration.InstanceGUID,
            FetchAPIKey = TestConfiguration.FetchAPIKey,
            PreviewAPIKey = TestConfiguration.PreviewAPIKey,
            Locales = TestConfiguration.Locale,
            ChannelName = TestConfiguration.ChannelName
        };

        PreviewService = new FetchApiService(_previewHttpClient, Options.Create(previewSettings));
    }

    public void Dispose()
    {
        _fetchHttpClient.Dispose();
        _previewHttpClient.Dispose();
    }
}

/// <summary>
/// Collection definition to share the ServiceFixture across test classes.
/// </summary>
[CollectionDefinition("Agility API")]
public class AgilityApiCollection : ICollectionFixture<ServiceFixture>
{
}
