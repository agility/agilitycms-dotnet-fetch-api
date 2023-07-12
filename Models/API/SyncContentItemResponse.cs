using System.Collections.Generic;
using Agility.NET.Shared.Models;

namespace NET5.FetchAPI.Models.API
{
    public class SyncContentItemResponse
    {
        public long SyncToken { get; set; }
        public List<ContentItem> Items { get; set; }
    }
}
