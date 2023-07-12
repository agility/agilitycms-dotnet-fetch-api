using System.Collections.Generic;

namespace Agility.NET.FetchAPI.Models.API
{
    public class SyncPageResponse
    {
        public long SyncToken { get; set; }
        public List<PageResponse> Items { get; set; }
    }
}
