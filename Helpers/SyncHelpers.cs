namespace Agility.NET5.FetchAPI.Helpers
{
    public static class SyncHelpers
    {
        public static string BuildSyncRequest(string baseAddress, string apiType, string locale, string type, long syncToken, int pageSize)
        {
            return $@"{baseAddress}/{apiType}/{locale}/sync/{type}?SyncToken={syncToken}&pageSize={pageSize}";
        }
    }
}