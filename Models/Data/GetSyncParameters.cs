namespace Agility.NET.FetchAPI.Models.Data
{
    public class GetSyncParameters
    {
        public string Locale { get; set; }
        public long SyncToken { get; set; }
        public int PageSize { get; set; }
    }
}
