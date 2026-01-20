namespace Agility.NET.FetchAPI.Helpers
{
    public static class SyncHelpers
    {
        public static string BuildSyncRequest(string locale, string type, long syncToken, int pageSize)
        {
            return $@"/{locale}/sync/{type}?SyncToken={syncToken}&pageSize={pageSize}";
        }
    }
}