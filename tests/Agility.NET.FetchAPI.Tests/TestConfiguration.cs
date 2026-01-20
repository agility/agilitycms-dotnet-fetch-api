namespace Agility.NET.FetchAPI.Tests;

/// <summary>
/// Test configuration with credentials for the Agility CMS test instance.
/// Reads from environment variables, with fallback to empty strings for CI.
///
/// Required environment variables:
/// - AGILITY_INSTANCE_GUID
/// - AGILITY_FETCH_API_KEY
/// - AGILITY_PREVIEW_API_KEY
/// </summary>
public static class TestConfiguration
{
    public static string InstanceGUID =>
        Environment.GetEnvironmentVariable("AGILITY_INSTANCE_GUID") ?? "";

    public static string FetchAPIKey =>
        Environment.GetEnvironmentVariable("AGILITY_FETCH_API_KEY") ?? "";

    public static string PreviewAPIKey =>
        Environment.GetEnvironmentVariable("AGILITY_PREVIEW_API_KEY") ?? "";

    public const string Locale = "en-us";
    public const string ChannelName = "website";

    // Test content references
    public const string TestListReferenceName = "posts";
}
